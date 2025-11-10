using Economy.Application.Providers;
using Microsoft.AspNetCore.Http;

namespace Economy.Infrastructure
{
    public sealed class TenantContextAccessor : ITenantContextAccessor
    {

        private readonly IHttpContextAccessor _http;
        public TenantContextAccessor(IHttpContextAccessor http) => _http = http;

        public TenantContext Current
        {
            get
            {
                var items = _http.HttpContext!.Items;
                if (!items.TryGetValue("__tenant_ctx", out var obj) || obj is not TenantContext ctx)
                {
                    ctx = new TenantContext();
                    items["__tenant_ctx"] = ctx;
                }
                return ctx;
            }
        }
    }
}
