using Economy.UI.Dtos;
using HotelMultiTenant.Multitenancy;

namespace HotelMultiTenant.Services
{
    public class InMemoryContentService : IContentService
    {
        private readonly IApiClient _apiClient;

        public InMemoryContentService(IApiClient apiClient)
        {
            _apiClient = apiClient;
        }

        public async Task<TenantDto> GetTanentAsync(CancellationToken ct = default)
        {
            var result = await _apiClient.GetAsync<TenantDto>("/api/Content/tanent", ct);
            return result;
        }

        public async Task<List<MenuNodeDto>> GetMenusAsync(string lang, CancellationToken ct = default)
        {
            var result = await _apiClient.GetAsync<List<MenuNodeDto>>($"/api/Content/menus?lang={lang}", ct);
            return result;
        }

        public async Task<PageDetailDto> GetHomeAsync(string lang, CancellationToken ct = default)
        {
            ///api/Content/homepage? lang = tr
            var result = await _apiClient.GetAsync<PageDetailDto>($"/api/Content/homepage?lang={lang}", ct);
            return result;
        }

        public async Task<SiteMetaDto> GetSeoMetaAsync(string lang, CancellationToken ct = default)
        {
            var result = await _apiClient.GetAsync<SiteMetaDto>($"/api/Content/sitemeta?lang={lang}", ct);
            return result;
        }

        public async Task<PageDetailDto> GetPageAsync(string lang, string slug, CancellationToken ct = default)
        {
            var result = await _apiClient.GetAsync<PageDetailDto>($"/api/Content/page/{slug}?lang={lang}", ct);
            return result;
        }
    }
}
