namespace HotelMultiTenant.Multitenancy
{
    public class TenantContext { public Tenant? Current { get; init; } }

    public static class TenantHttpExtensions
    {
        private const string Key = "__TENANT_CTX__";
        public static void SetTenant(this HttpContext ctx, TenantContext val) => ctx.Items[Key] = val;
        public static TenantContext? GetTenant(this HttpContext ctx) => ctx.Items[Key] as TenantContext;
    }
}
