using Microsoft.Extensions.Caching.Memory;
using MyHotelSite.Models;
using MyHotelSite.Repositories;

namespace MyHotelSite.Services;

public interface ISiteConfigAccessor
{
    Task<(SiteSettingDto? Setting, SiteTechnicalDto? Technical)> GetAsync(int appId);
}

public class SiteConfigAccessor : ISiteConfigAccessor
{
    private readonly IAppSettingRepository _s;
    private readonly IAppSettingTechnicalRepository _t;
    private readonly IMemoryCache _cache;
    public SiteConfigAccessor(IAppSettingRepository s, IAppSettingTechnicalRepository t, IMemoryCache cache)
    { _s = s; _t = t; _cache = cache; }

    public async Task<(SiteSettingDto? Setting, SiteTechnicalDto? Technical)> GetAsync(int appId)
    {
        var key = $"sitecfg:{appId}";
        return await _cache.GetOrCreateAsync(key, async e =>
        {
            e.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5);
            var s = await _s.GetAsync(appId);
            var t = await _t.GetAsync(appId);
            return (s is null ? null : new SiteSettingDto
            {
                AppId = s.AppId,
                Title = s.Title,
                Description = s.Description,
                LogoPath = s.LogoPath,
                FaviconPath = s.FaviconPath,
                ShareImage = s.ShareImage
            },
            t is null ? null : new SiteTechnicalDto
            {
                AppId = t.AppId,
                DefaultLanguage = t.DefaultLanguage,
                SupportedLanguages = t.SupportedLanguages,
                CdnBaseUrl = t.CdnBaseUrl,
                CdnEnabled = t.CdnEnabled,
                EnableOutputCache = t.EnableOutputCache,
                OutputCacheTtlSeconds = t.OutputCacheTtlSeconds,
                MaintenanceModeEnabled = t.MaintenanceModeEnabled,
                MaintenanceAllowedIpList = t.MaintenanceAllowedIpList,
                CookieBannerEnabled = t.CookieBannerEnabled,
                GoogleAnalyticsId = t.GoogleAnalyticsId,
                GoogleTagManagerId = t.GoogleTagManagerId,
                HreflangDomainMap = t.HreflangDomainMap
            });
        })!;
    }
}
