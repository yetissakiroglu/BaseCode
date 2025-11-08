namespace HotelMultiTenant.Multitenancy
{

    public class Tenant
    {
        public int Id { get; set; }
        public string Name { get; set; } = default!;
        public string ThemeKey { get; set; } = "Classic";
        public List<TenantDomain> Domains { get; set; } = new();
        public TenantSettings Settings { get; set; } = new();
    }
    public class TenantDomain { public string Hostname { get; set; } = default!; public bool IsPrimary { get; set; } }

    public class TenantSettings
    {
        public string? CanonicalHost { get; set; }
        public bool ForceHttps { get; set; }
        public bool ShowCookieBanner { get; set; }
        public bool EnableCdn { get; set; }
        public string? CdnBaseUrl { get; set; }
        public bool MaintenanceMode { get; set; }
        public bool OutputCacheEnabled { get; set; }
        public int OutputCacheTtlSeconds { get; set; } = 300;
    }
}
