using Castle.DynamicProxy;
using LoggingLibrary.Attributes;
using LoggingLibrary.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Reflection;
using System.Security.Claims;
using System.Text.Json;

namespace LoggingLibrary.Interceptors
{
    public class LoggingInterceptor : IInterceptor
    {
        private readonly LoggingDbContext _dbContext;
        private readonly ILogger<LoggingInterceptor> _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;

     
        public LoggingInterceptor(LoggingDbContext dbContext, ILogger<LoggingInterceptor> logger, IHttpContextAccessor httpContextAccessor)
        {
            _dbContext = dbContext;
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
        }

        public async void Intercept(IInvocation invocation)
        {
            var method = invocation.MethodInvocationTarget ?? invocation.Method;

            // Eğer metodda [Log] attribute yoksa devam et
            if (method.GetCustomAttribute<LogAttribute>() == null)
            {
                invocation.Proceed();
                return;
            }


            var httpContext = _httpContextAccessor.HttpContext;
            string ipAddress = httpContext?.Connection.RemoteIpAddress?.ToString() ?? "Unknown IP";
            string userId = httpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "Anonymous";

            // Attribute'u kontrol et
            var logAttribute = method.GetCustomAttribute<LogAttribute>();

            string logMessage = logAttribute != null ? logAttribute.Message : "Method executed";



            var parameters = JsonSerializer.Serialize(invocation.Arguments);

            _logger.LogInformation($"[SERVICE] User: {userId}, Class: {method.DeclaringType.Name}, Method: {method.Name}, Parameters: {parameters}");

            // Metodu çalıştır
            invocation.Proceed();

            object response = null;

            // Eğer dönüş değeri bir Task ise, sonucu bekle
            if (invocation.Method.ReturnType == typeof(Task))
            {
                await (Task)invocation.ReturnValue;
                response = "Task Completed";
            }
            else if (invocation.Method.ReturnType.IsGenericType && invocation.Method.ReturnType.GetGenericTypeDefinition() == typeof(Task<>))
            {
                var resultProperty = invocation.Method.ReturnType.GetProperty("Result");
                response = resultProperty?.GetValue(invocation.ReturnValue);
            }
            else
            {
                response = invocation.ReturnValue;
            }

            var responseText = response is not null ? JsonSerializer.Serialize(response) : "null";

            _dbContext.Logs.Add(new LogEntry
            {
                UserId = userId,
                Layer = method.DeclaringType.Name,
                Method = method.Name,
                Parameters = parameters,
                Response = responseText,
                LogMessage = logMessage,
                IpAddress = ipAddress
            });

            _dbContext.SaveChanges();
        }
    }
}
