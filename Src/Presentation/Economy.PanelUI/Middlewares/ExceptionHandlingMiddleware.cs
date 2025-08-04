namespace Economy.Panel.UI.Middlewares
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;

        public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context); // normal işlemlere devam
            }
            catch (InvalidOperationException ex) when (ex.Message.Contains("HotelDbContext has not been initialized"))
            {
                _logger.LogError(ex, "DbContext Initialization Exception");

                context.Response.Redirect("/Error/DbContextNotInitialized");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unhandled Exception");

                context.Response.Redirect("/Error/General");
            }
        }
    }

}
