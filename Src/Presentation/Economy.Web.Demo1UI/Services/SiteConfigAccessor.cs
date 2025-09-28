using Economy.UI.Models;
using Economy.UI.Models.PageDtos;
using Economy.Web.Demo1UI.Helpers;
using Microsoft.Extensions.Caching.Memory;
using static System.Net.WebRequestMethods;

namespace MyHotelSite.Services;

public interface ISiteConfigAccessor
{
    Task<(SiteSettingDto? Setting, SiteTechnicalDto? Technical)> GetAsync(string lang);
    Task<PageUnifiedVm?> GetPageAsync(string lang, string slug, CancellationToken ct = default);

}

public class SiteConfigAccessor : ISiteConfigAccessor
{
    private readonly IApiClientHelper _apiClient;
    private readonly IMemoryCache _cache;
    public SiteConfigAccessor(IMemoryCache cache, IApiClientHelper apiClient)
    {
        _cache = cache;
        _apiClient = apiClient;
    }

    public async Task<(SiteSettingDto? Setting, SiteTechnicalDto? Technical)> GetAsync(string lang)
    {
        var key = $"sitecfg:{lang}";
        return await _cache.GetOrCreateAsync(key, async e =>
        {
            e.AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(10);

            var appConfig = await _apiClient.GetAsync<SiteConfigResponse>("/api/siteconfig", lang);

            var s = appConfig.Setting;
            var t = appConfig.Technical;
            return (s is null ? null : new SiteSettingDto
            {
                SiteTitle = s.SiteTitle,
                MetaDescription = s.MetaDescription,
                MetaSlogan = s.MetaSlogan,
                MetaTitle = s.MetaTitle,
                ShareImagePath = s.ShareImagePath,
                Description = s.Description,
                LogoPath = s.LogoPath,
                FaviconPath = s.FaviconPath
            },
            t is null ? null : new SiteTechnicalDto
            {
                DefaultLanguage = t.DefaultLanguage,
                SupportedLanguages = t.SupportedLanguages,
                CdnBaseUrl = t.CdnBaseUrl,
                CdnEnabled = t.CdnEnabled,
                EnableOutputCache = t.EnableOutputCache,
                OutputCacheTtlSeconds = t.OutputCacheTtlSeconds,
                MaintenanceModeEnabled = t.MaintenanceModeEnabled,
                MaintenanceAllowedIpList = t.MaintenanceAllowedIpList,
                CookieBannerEnabled = t.CookieBannerEnabled,
                DomainName = t.DomainName,
                EnableDebugMode = t.EnableDebugMode,
                ForceSSL = t.ForceSSL,
                MaintenanceMessage = t.MaintenanceMessage
            });
        })!;
    }

    public async Task<PageUnifiedVm?> GetPageAsync(string lang, string slug, CancellationToken ct = default)
    {
        // /api/pages/{lang}/{slug}
        var url = $"/api/pages/{lang}/{slug}";
        var resp = await _apiClient.GetAsync<PageUnifiedVm>(url, lang);
 
        return resp;
    }
}
