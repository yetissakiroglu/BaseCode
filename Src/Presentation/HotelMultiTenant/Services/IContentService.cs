namespace HotelMultiTenant.Services
{
    public interface IContentService
    {
        Task<HomeVm> GetHomeAsync(int tenantId, CancellationToken ct = default);
        Task<AboutVm> GetAboutAsync(int tenantId, CancellationToken ct = default);
        Task<RoomsVm> GetRoomsAsync(int tenantId, CancellationToken ct = default);
        Task<ServicesVm> GetServicesAsync(int tenantId, CancellationToken ct = default);
        Task<ContactVm> GetContactAsync(int tenantId, CancellationToken ct = default);
    }
}
