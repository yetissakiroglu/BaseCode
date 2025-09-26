using Economy.Application.ApplicationUI.Dtos;
using Economy.Application.ApplicationUI.Interfaces;
using Economy.Core.Interfaces;
using Economy.Domain.Entites.EntityAppSettings;
using Economy.Persistence.UnitOfWorks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Economy.Persistence.PersistenceUI.Services
{
    public class SiteConfigAccessor : ISiteConfigAccessor
    {
        private readonly IEntityRepository<AppSetting, int> _appSettingRepository;
        private readonly IEntityRepository<AppTechnicalSetting, int> _appTechnicalSettingRepository;
        private readonly IMemoryCache _cache;
        private readonly IUnitOfWork _unitOfWork;
        public SiteConfigAccessor(IMemoryCache cache, IUnitOfWork unitOfWork)
        {
            _cache = cache;
            _unitOfWork = unitOfWork;
            _appSettingRepository = unitOfWork.HotelEntityRepository<AppSetting>();
            _appTechnicalSettingRepository = unitOfWork.HotelEntityRepository<AppTechnicalSetting>();
        }



        public async Task<(SiteSettingDto? Setting, SiteTechnicalDto? Technical)> GetAsync(int appId)
        {
            var key = $"sitecfg:{appId}";
            return await _cache.GetOrCreateAsync(key, async e =>
            {
                e.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5);
                var s = await _appSettingRepository.DataSet.Include(i=>i.Translations).FirstOrDefaultAsync(x => !x.IsDeleted);
                var t = await _appTechnicalSettingRepository.DataSet.FirstOrDefaultAsync(x=>!x.IsDeleted);

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

}
