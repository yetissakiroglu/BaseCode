using Economy.Application.ApplicationUI.Dtos;
using Economy.UI.Dtos;

namespace Economy.Application.ApplicationUI.Interfaces
{
  

    public interface IApplicationMenuService
    {
        Task<TenantDto> GetTenantAsync(string xtanent,CancellationToken ct);

        Task<List<MenuNodeDto>> GetMenuAsync(string lang, CancellationToken ct);
        Task<SiteMetaDto> GetSiteMetaAsync(string lang, CancellationToken ct);
        Task<PageDetailDto?> GetHomepageAsync(string lang, CancellationToken ct);
        Task<PageDetailDto?> GetBySlugAsync(string lang, string slug, CancellationToken ct);

    }
}
