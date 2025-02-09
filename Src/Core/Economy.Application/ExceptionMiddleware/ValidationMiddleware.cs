using Economy.Persistence.Services;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Controllers;
using System.Net;
using System.Text;
using System.Text.Json;

namespace Economy.AuthServer.API.ExceptionMiddleware
{

    public class ValidationMiddleware
    {
        private readonly RequestDelegate _next;

        public ValidationMiddleware(RequestDelegate next)
        {
            _next = next ?? throw new ArgumentNullException(nameof(next));
        }

        public async Task InvokeAsync(HttpContext context)
        {
            context.Request.EnableBuffering();

            var requestBody = await ReadRequestBodyAsync(context);
            if (string.IsNullOrWhiteSpace(requestBody))
            {
                await _next(context);
                return;
            }

            var requestType = GetRequestModelType(context);
            if (requestType == null)
            {
                await _next(context);
                return;
            }

            if (!TryDeserializeRequest(requestBody, requestType, out var requestObject))
            {
                await RespondWithErrorAsync(context, StatusCodes.Status400BadRequest, "Invalid JSON format.");
                return;
            }

            var validator = GetValidator(context, requestType);
            if (validator != null)
            {
                var validationResult = await validator.ValidateAsync(new ValidationContext<object>(requestObject));
                if (!validationResult.IsValid)
                {
                    await RespondWithValidationErrorsAsync(context, validationResult);
                    return;
                }
            }

            await _next(context);
        }

        private static async Task<string> ReadRequestBodyAsync(HttpContext context)
        {
            context.Request.Body.Position = 0;
            using var memoryStream = new MemoryStream();
            await context.Request.Body.CopyToAsync(memoryStream);
            context.Request.Body.Position = 0;
            return Encoding.UTF8.GetString(memoryStream.ToArray());
        }

        private static Type GetRequestModelType(HttpContext context)
        {
            var endpoint = context.GetEndpoint();
            return endpoint?.Metadata.GetMetadata<ControllerActionDescriptor>()?.Parameters.FirstOrDefault()?.ParameterType;
        }

        private static bool TryDeserializeRequest(string requestBody, Type requestType, out object requestObject)
        {
            try
            {
                requestObject = JsonSerializer.Deserialize(requestBody, requestType, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }) ?? throw new JsonException();
                return true;
            }
            catch (JsonException)
            {
                requestObject = null;
                return false;
            }
        }

        private static IValidator GetValidator(HttpContext context, Type requestType)
        {
            var validatorType = typeof(IValidator<>).MakeGenericType(requestType);
            return context.RequestServices.GetService(validatorType) as IValidator;
        }

        private static async Task RespondWithValidationErrorsAsync(HttpContext context, ValidationResult validationResult)
        {
            context.Response.StatusCode = StatusCodes.Status400BadRequest;
            context.Response.ContentType = "application/json";
       
            // PropertyName'lere göre hata mesajlarını gruplayarak her property için bir liste oluşturuyoruz.
            var errors = validationResult.Errors
                .GroupBy(e => e.PropertyName)
                .ToDictionary(
                    g => g.Key,
                    g => g.Select(e => e.ErrorMessage).ToList()
                );

            var resultModel = ServiceResult.Fail("", HttpStatusCode.NotFound);
            await context.Response.WriteAsJsonAsync(resultModel);
        }

        private static async Task RespondWithErrorAsync(HttpContext context, int statusCode, string message)
        {
            context.Response.StatusCode = statusCode;
            context.Response.ContentType = "application/json";

            await context.Response.WriteAsJsonAsync(JsonSerializer.Serialize(new { Message = message }));
        }
    }
}
