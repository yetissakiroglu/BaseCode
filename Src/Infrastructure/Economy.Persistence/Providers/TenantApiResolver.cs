using Economy.Core.Helpers;
using Economy.Core.Interfaces;
using Economy.Domain.Entites.AdminEntity.EntityApp;
using Economy.Persistence.Contexts;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace Economy.Persistence.Providers
{
    public static class TenantApiResolver
    {
        public static async Task<bool> ResolveAndBindAsync(
            HttpContext http, DefaultDbContext defaultDb, IUnitOfWork uow, CancellationToken ct)
        {
            var tenantKey = http.Request.Headers["X-TENANT"].FirstOrDefault();
            var host = http.Request.Host.Host;

            var q = defaultDb.Set<App>().AsQueryable();

            App? app = null;
            if (!string.IsNullOrWhiteSpace(tenantKey))
            {
                if (int.TryParse(tenantKey, out var appId))
                    app = await q.FirstOrDefaultAsync(a => a.Id == appId, ct);
                else
                    app = await q.FirstOrDefaultAsync(a => a.Domain == tenantKey, ct);
            }

            if (app is null && !string.IsNullOrWhiteSpace(host))
                app = await q.FirstOrDefaultAsync(a => a.Domain == host, ct);

            if (app is null)
                return false;

            var cs = ConnectionStringHelper.Build(app.ServerName, app.DatabaseName, app.IsPassword, app.UserName, app.Password);
            uow.SetHotelConnectionString(cs);
            return true;
        }
    }
}
