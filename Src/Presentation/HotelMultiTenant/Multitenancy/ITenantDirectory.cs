namespace HotelMultiTenant.Multitenancy
{
    public interface ITenantDirectory
    {
        // Host name → Tenant
        Task<Tenant?> ResolveByHostAsync(string host, CancellationToken ct = default);
    }
}
