using Economy.UI.Dtos;

namespace HotelMultiTenant.Multitenancy
{
    public interface ITenantDirectory
    {
        // Host name → Tenant
        Task<TenantDto?> ResolveByHostAsync(string host, CancellationToken ct = default);
    }
}
