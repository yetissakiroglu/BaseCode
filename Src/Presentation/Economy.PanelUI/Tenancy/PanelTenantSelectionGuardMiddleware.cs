namespace Economy.Panel.UI.Tenancy
{

    public sealed class PanelTenantSelectionGuardMiddleware
    {
        private readonly RequestDelegate _next;
        public PanelTenantSelectionGuardMiddleware(RequestDelegate next) => _next = next;

        public async Task InvokeAsync(HttpContext ctx)
        {
            var isTenantArea = ctx.Request.Path.StartsWithSegments("/tenant", StringComparison.OrdinalIgnoreCase);
            if (!isTenantArea || ctx.User?.Identity?.IsAuthenticated != true || !ctx.User.IsInRole("Tenant Admin"))
            { await _next(ctx); return; }

            var isSelectionPage =
                ctx.Request.Path.StartsWithSegments("/tenant/select", StringComparison.OrdinalIgnoreCase) ||
                ctx.Request.Path.StartsWithSegments("/account/login", StringComparison.OrdinalIgnoreCase) ||
                ctx.Request.Path.StartsWithSegments("/account/logout", StringComparison.OrdinalIgnoreCase);

            if (isSelectionPage) { await _next(ctx); return; }

            var hasAppId = !string.IsNullOrWhiteSpace(ctx.User.FindFirst("appId")?.Value);
            if (!hasAppId)
            {
                ctx.Response.Redirect("/tenant/select");
                return;
            }

            await _next(ctx);
        }
    }


}
