using Economy.Core.Interfaces;
using Economy.Core.Services.Providers;

namespace Economy.Panel.UI.Middlewares
{
    public class HotelConnectionMiddleware
    {
        private readonly RequestDelegate _next;

        public HotelConnectionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, IUnitOfWork unitOfWork, TenantProvider tenantProvider)
        {
            var user = context.User;
            if (user.Identity?.IsAuthenticated == true)
            {
                var connectionString = await tenantProvider.GetConnectionStringAsync();

                if (!string.IsNullOrWhiteSpace(connectionString))
                {
                    unitOfWork.SetHotelConnectionString(connectionString);
                }
            }
            else
            {
                //var connectionString = await tenantProvider.GetConnectionStringAsync("HotelDb2");
                //if (!string.IsNullOrWhiteSpace(connectionString))
                //{
                //    unitOfWork.SetHotelConnectionString(connectionString);
                //}

                //var apiKey = context.Request.Headers["X-API-KEY"].FirstOrDefault();
                //if (!string.IsNullOrWhiteSpace(apiKey))
                //{
                //    var connectionString = await tenantProvider.GetConnectionStringAsync(apiKey);
                //    if (!string.IsNullOrWhiteSpace(connectionString))
                //    {
                //        unitOfWork.SetHotelConnectionString(connectionString);
                //    }
                //}
            }





            await _next(context);
        }
    }
}
