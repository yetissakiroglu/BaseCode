using Economy.Core.Enums;
using Economy.Core.Helpers;
using Economy.Core.Interfaces;
using Economy.Domain.Entites.AdminEntity.EntityApp;
using Economy.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using System.Text;

namespace Economy.Panel.UI.Tenancy
{

    public sealed class HotelConnectionMiddleware
    {
        private readonly RequestDelegate _next;
        public HotelConnectionMiddleware(RequestDelegate next) => _next = next;

        public async Task InvokeAsync(HttpContext ctx, IUnitOfWork uow, DefaultDbContext defaultDb)
        {
            var path = ctx.Request.Path.Value ?? string.Empty;

            // 🔹 1) Tenant Area kuralı (senin mevcut haliyle)
            if (path.StartsWith("/tenant", StringComparison.OrdinalIgnoreCase))
            {
                if (ctx.User?.Identity?.IsAuthenticated == true && ctx.User.IsInRole("Tenant Admin"))
                {
                    var appIdStr = ctx.User.FindFirst("appId")?.Value;
                    if (int.TryParse(appIdStr, out var appId))
                    {
                        var app = await defaultDb.Set<App>()
                            .Where(a => a.Id == appId)
                            .Select(a => new
                            {
                                a.ServerName,
                                a.DatabaseName,
                                a.IsPassword,
                                a.UserName,
                                a.Password
                            })
                            .FirstOrDefaultAsync(ctx.RequestAborted);

                        if (app != null)
                        {
                            var conn = ConnectionStringHelper.Build(
                                app.ServerName, app.DatabaseName, app.IsPassword, app.UserName, app.Password);
                            if (!string.IsNullOrWhiteSpace(conn))
                                uow.SetHotelConnectionString(conn);
                        }
                    }
                }

                await _next(ctx);
                return;
            }

            // 🔹 2) API Area: X-TENANT veya Host + AccessMode kontrolü
            if (IsApiRequest(ctx))
            {
                var tenantKey = ctx.Request.Headers["X-TENANT"].FirstOrDefault();
                var host = ctx.Request.Host.Host;
                var key = tenantKey ?? host;

                if (!string.IsNullOrWhiteSpace(key))
                {
                    var app = await defaultDb.Set<App>()
                        .Where(a => a.Domain == key)
                        .Select(a => new
                        {
                            a.ServerName,
                            a.DatabaseName,
                            a.IsPassword,
                            a.UserName,
                            a.Password,
                            a.AccessMode
                        })
                        .FirstOrDefaultAsync(ctx.RequestAborted);
                    if (app is null)
                    {
                        ctx.Response.StatusCode = StatusCodes.Status403Forbidden;
                        ctx.Response.ContentType = "text/plain; charset=utf-8";
                        await ctx.Response.WriteAsync("Access to this API is blocked. (App record not found)", Encoding.UTF8);
                        return;
                    }
                    if (app is not null)
                    {
                        // 🔸 AccessMode kontrolü
                        if (app.AccessMode == AppAccessMode.Blocked)
                        {
                            ctx.Response.StatusCode = StatusCodes.Status403Forbidden;
                            await ctx.Response.WriteAsync("Access to this API is blocked.");
                            return;
                        }

                        if (app.AccessMode == AppAccessMode.LocalOnly)
                        {
                            var remoteIp = ctx.Connection.RemoteIpAddress?.ToString() ?? "";
                            if (!remoteIp.StartsWith("127.") && remoteIp != "::1")
                            {
                                ctx.Response.StatusCode = StatusCodes.Status403Forbidden;
                                await ctx.Response.WriteAsync("This API is only accessible from localhost.");
                                return;
                            }
                        }

                        // 🔸 Erişim uygunsa bağlantıyı ayarla
                        var conn = ConnectionStringHelper.Build(
                            app.ServerName,
                            app.DatabaseName,
                            app.IsPassword,
                            app.UserName,
                            app.Password);

                        if (!string.IsNullOrWhiteSpace(conn))
                            uow.SetHotelConnectionString(conn);
                    }
                }
            }

            // 🔹 3) Diğer tüm istekler normal akış
            await _next(ctx);
        }

        private static bool IsApiRequest(HttpContext ctx)
        {
            var area = ctx.GetRouteValue("area")?.ToString();
            if (!string.IsNullOrEmpty(area) && area.Equals("api", StringComparison.OrdinalIgnoreCase))
                return true;

            var path = ctx.Request.Path.Value ?? string.Empty;
            return path.StartsWith("/api", StringComparison.OrdinalIgnoreCase);
        }
    }
}