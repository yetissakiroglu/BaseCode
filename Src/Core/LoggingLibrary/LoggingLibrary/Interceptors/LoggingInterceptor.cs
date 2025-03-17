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

        void IInterceptor.Intercept(IInvocation invocation)
        {
            var method = invocation.MethodInvocationTarget ?? invocation.Method;

            // Eğer metodda [Log] attribute'u yoksa devam et
            var logAttribute = method.GetCustomAttribute<LogAttribute>();
            if (logAttribute == null)
            {
                invocation.Proceed();
                return;
            }

            var httpContext = _httpContextAccessor.HttpContext;
            string ipAddress = httpContext?.Connection.RemoteIpAddress?.ToString() ?? "Unknown IP";
            string userId = httpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "Anonymous";

            string logMessage = logAttribute.Message ?? "Method executed";
            string parameters = JsonSerializer.Serialize(invocation.Arguments);

            _logger.LogInformation($"[SERVICE] User: {userId}, Class: {method.DeclaringType?.Name}, Method: {method.Name}, Parameters: {parameters}");

            object response = null;
            Exception exception = null;

            try
            {
                invocation.Proceed();

                if (invocation.Method.ReturnType == typeof(Task))
                {
                    ((Task)invocation.ReturnValue).GetAwaiter().GetResult();
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
            }
            catch (Exception ex)
            {
                exception = ex;
                _logger.LogError(ex, $"Exception in {method.DeclaringType?.Name}.{method.Name}");
            }

            string responseText = exception != null ? $"Exception: {exception.Message}" : JsonSerializer.Serialize(response);

            var logEntry = new LogEntry
            {
                UserId = userId,
                Layer = method.DeclaringType?.Name ?? "Unknown",
                Method = method.Name,
                Parameters = parameters,
                Response = responseText,
                LogMessage = logMessage,
                IpAddress = ipAddress,
                //Exception = exception?.ToString()
            };

            _dbContext.Logs.Add(logEntry);
            _dbContext.SaveChanges(); // Asenkron yerine senkron çağrı
        }
    }
}
