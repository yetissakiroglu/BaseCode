using Microsoft.AspNetCore.Http;
using MyHotelSite.Repositories;

namespace MyHotelSite.Middlewares;

public class MaintenanceMiddleware
{
    private readonly RequestDelegate _next;
    public MaintenanceMiddleware(RequestDelegate next) => _next = next;

    public async Task Invoke(HttpContext ctx, IAppSettingTechnicalRepository techRepo)
    {
        var ip = ctx.Connection.RemoteIpAddress?.ToString() ?? "";
        var tech = await techRepo.GetAsync(1);
        if (tech?.MaintenanceModeEnabled == true)
        {
            var allowed = tech.MaintenanceAllowedIpList?.Contains(ip) ?? false;
            if (!allowed)
            {
                ctx.Response.Headers["X-Robots-Tag"] = "noindex, nofollow";
                ctx.Response.StatusCode = 503;
                await ctx.Response.WriteAsync("Site bakımda. Lütfen daha sonra tekrar deneyin.");
                return;
            }
            else
            {
                ctx.Response.Headers["X-Robots-Tag"] = "noindex, nofollow";
            }
        }
        await _next(ctx);
    }
}
