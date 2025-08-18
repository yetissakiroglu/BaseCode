using Economy.Core.Interfaces;
using Economy.Domain.Entites.AppEntities;

namespace Economy.Panel.UI.Middlewares
{
    public class ErrorLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ErrorLoggingMiddleware> _logger;
        //private readonly IUnitOfWork _uow;

        public ErrorLoggingMiddleware(RequestDelegate next, ILogger<ErrorLoggingMiddleware> logger)
        {
            _next = next; _logger = logger; 
        }

        public async Task Invoke(HttpContext ctx)
        {
            try
            {
                await _next(ctx);
            }
            catch (Exception ex)
            {
                try
                {


                    //var repo = _uow.DefaultEntityRepository<AppErrorLog>();
                    //var log = new AppErrorLog
                    //{
                    //    CreatedAt = DateTime.UtcNow,
                    //    UserId = null, // Claims'ten alabilirsin
                    //    UserName = ctx.User?.Identity?.IsAuthenticated == true ? ctx.User.Identity!.Name : null,
                    //    HttpMethod = ctx.Request.Method,
                    //    RequestPath = ctx.Request.Path.Value,
                    //    QueryString = ctx.Request.QueryString.HasValue ? ctx.Request.QueryString.Value : null,
                    //    IpAddress = ctx.Connection.RemoteIpAddress?.ToString(),
                    //    UserAgent = ctx.Request.Headers.UserAgent.ToString(),
                    //    CorrelationId = ctx.TraceIdentifier,
                    //    StatusCode = 500,
                    //    ExceptionType = ex.GetType().FullName,
                    //    Message = ex.Message,
                    //    StackTrace = ex.ToString(),
                    //    Source = ex.Source,
                    //    TargetSite = ex.TargetSite?.Name
                    //};
                    //repo.Add(log);
                    //await _uow.SaveDefaultChangesAsync();
                }
                catch (Exception logEx)
                {
                    _logger.LogError(logEx, "Error while writing AppErrorLog");
                }

                // tekrar fırlat
                throw;
            }
        }
    }

}
