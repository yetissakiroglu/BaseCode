﻿using Economy.Application.Dtos;
using Economy.Application.Exceptions;
using Economy.Persistence.Services;
using System.Net;

namespace Economy.AuthServer.API.ExceptionMiddleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;

        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (NotFoundException ex)
            {
                _logger.LogWarning(ex, ex.Message);
                context.Response.StatusCode = (int)HttpStatusCode.NotFound;
               var resultModel = ServiceResult.Fail(ex.Message, HttpStatusCode.NotFound);
                await context.Response.WriteAsJsonAsync(resultModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Beklenmeyen bir hata oluştu.");
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                await context.Response.WriteAsJsonAsync(new { message = "Sunucu hatası." });
            }
        }
    }
}
