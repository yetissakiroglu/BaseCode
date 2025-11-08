namespace HotelMultiTenant.Multitenancy
{
    public class TenantMiddleware : IMiddleware
    {
        private readonly ITenantDirectory _directory;
        public TenantMiddleware(ITenantDirectory directory) => _directory = directory;

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            var host = context.Request.Host.Host ?? "";
            var tenant = await _directory.ResolveByHostAsync(host, context.RequestAborted);
            context.SetTenant(new TenantContext { Current = tenant });
            await next(context);
        }
    }
}
