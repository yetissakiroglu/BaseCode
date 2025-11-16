using Economy.UI.Dtos;

namespace HotelMultiTenant.Services
{
    public interface IContentService
    {

        Task<TenantDto> GetTanentAsync(CancellationToken ct = default);
        Task<List<MenuNodeDto>> GetMenusAsync(string lang, CancellationToken ct = default);
        Task<PageDetailDto> GetHomeAsync(string lang, CancellationToken ct = default);
        Task<PageDetailDto> GetPageAsync(string lang, string slug, CancellationToken ct = default);

        Task<SiteMetaDto> GetSeoMetaAsync(string lang, CancellationToken ct = default);

        ///api/Content/homepage? lang = tr

    }
}
