using Economy.Application.Interfaces;
using Economy.Domain.Entites.AppEntities;

namespace Economy.Panel.UI.Middlewares
{
    public class ErrorLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        //private readonly IPanelErrorLogService _panelErrorLogService;
        public ErrorLoggingMiddleware(RequestDelegate next)
        {
            _next = next;
            //_panelErrorLogService = panelErrorLogService;
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

                    //await _panelErrorLogService.Create(log);
                }
                catch (Exception logEx)
                {

                }

            }
        }
    }

}
