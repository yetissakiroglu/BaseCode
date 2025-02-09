using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Economy.AuthServer.API.ExceptionMiddleware
{
    public class ValidationExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ValidationExceptionMiddleware> _logger;

        public ValidationExceptionMiddleware(RequestDelegate next, ILogger<ValidationExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (ValidationException ex)
            {
                _logger.LogError(ex, "Validation error occurred.");

                context.Response.StatusCode = StatusCodes.Status400BadRequest;
                context.Response.ContentType = "application/json";

                var errors = ex.Errors
                    .GroupBy(e => e.PropertyName)
                    .ToDictionary(
                        g => g.Key,
                        g => g.Select(e => e.ErrorMessage).ToArray()
                    );

                var problemDetails = new
                {
                    type = "https://tools.ietf.org/html/rfc9110#section-15.5.1",
                    title = "One or more validation errors occurred.",
                    status = 400,
                    traceId = context.TraceIdentifier,
                    errors
                };

                await context.Response.WriteAsJsonAsync(problemDetails);
            }
        }
    }
}
