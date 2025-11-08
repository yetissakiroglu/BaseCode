using HotelMultiTenant.Multitenancy;

namespace HotelMultiTenant.Services
{

    public class InMemoryTenantDirectory : ITenantDirectory
    {
        // Burayı ileride gerçek HTTP/DB kaynağına çevirebilirsin.
        // Şimdilik demo için host→tenant eşlemesi:
        private readonly List<Tenant> _tenants =
        [
            new Tenant
        {
            Id = 1, Name = "X Otel", ThemeKey = "Classic",
            Domains = new() { new TenantDomain { Hostname = "xotel.local", IsPrimary = true } },
            Settings = new TenantSettings { CanonicalHost = "xotel.local", OutputCacheEnabled = true }
        },
        new Tenant
        {
            Id = 2, Name = "Y Otel", ThemeKey = "Modern",
            Domains = new() { new TenantDomain { Hostname = "yotel.local", IsPrimary = true } },
            Settings = new TenantSettings { CanonicalHost = "yotel.local", OutputCacheEnabled = true }
        },
        new Tenant
        {
            Id = 3, Name = "Z Otel", ThemeKey = "Seaside",
            Domains = new() { new TenantDomain { Hostname = "zotel.local", IsPrimary = true } },
            Settings = new TenantSettings { CanonicalHost = "zotel.local", OutputCacheEnabled = true }
        }
        ];

        public Task<Tenant?> ResolveByHostAsync(string host, CancellationToken ct = default)
        {
            var t = _tenants.FirstOrDefault(x => x.Domains.Any(d =>
                d.Hostname.Equals(host, StringComparison.OrdinalIgnoreCase)));
            return Task.FromResult<Tenant?>(t);
        }
    }
}
