using Economy.UI.Dtos;

namespace HotelMultiTenant.Multitenancy
{
    public class TenantContext { public TenantDto? Current { get; init; } }
    public class SeoMetaContext { public SiteMetaDto? Current { get; init; } }

    

    public static class TenantHttpExtensions
    {
        private const string Key = "__TENANT_CTX__";
        private const string SeoKey = "__SEOMETA_CTX__";

        public static void SetTenant(this HttpContext ctx, TenantContext val) => ctx.Items[Key] = val;
        public static void SetSeoMetaTenant(this HttpContext ctx, SeoMetaContext val) => ctx.Items[SeoKey] = val;

        public static TenantContext? GetTenant(this HttpContext ctx) => ctx.Items[Key] as TenantContext;
        public static SeoMetaContext? GetSeoMetaTenant(this HttpContext ctx) => ctx.Items[SeoKey] as SeoMetaContext;
    }
}
