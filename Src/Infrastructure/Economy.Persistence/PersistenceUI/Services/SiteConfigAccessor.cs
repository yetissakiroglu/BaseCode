using Economy.Application.ApplicationUI.Interfaces;
using Economy.Core.Interfaces;
using Economy.Domain.Entites.EntityAppLanguage;
using Economy.Domain.Entites.EntityAppSettings;
using Economy.Domain.Entites.EntitySlides;
using Economy.UI.Models;
using Microsoft.EntityFrameworkCore;

namespace Economy.Persistence.PersistenceUI.Services
{
    public class SiteConfigAccessor : ISiteConfigAccessor
    {
        private readonly IEntityRepository<AppSetting, int> _appSettingRepository;
        private readonly IEntityRepository<AppTechnicalSetting, int> _appTechnicalSettingRepository;
        private readonly IEntityRepository<AppLanguage, int> _appLanguageRepository;
        private readonly IEntityRepository<AppSettingLogo, int> _appSettingLogoRepository;
        private readonly IEntityRepository<AppSlide, int> _appSlideRepository;


        public SiteConfigAccessor(IUnitOfWork unitOfWork)
        {
            _appSettingRepository = unitOfWork.HotelEntityRepository<AppSetting>();
            _appTechnicalSettingRepository = unitOfWork.HotelEntityRepository<AppTechnicalSetting>();
            _appLanguageRepository = unitOfWork.HotelEntityRepository<AppLanguage>();
            _appSettingLogoRepository = unitOfWork.HotelEntityRepository<AppSettingLogo>();
            _appSlideRepository = unitOfWork.HotelEntityRepository<AppSlide>();
        }
        public (SiteSettingDto? Setting, SiteTechnicalDto? Technical) GetAsync(string lang)
        {
            // 1) Teknik ayarlar
            var t = _appTechnicalSettingRepository.DataSet
                .AsNoTracking()
                .FirstOrDefault(x => !x.IsDeleted);

            // 2) Default dil AppLanguage’den
            var defaultLang = _appLanguageRepository.DataSet
                .AsNoTracking()
                .Where(x => !x.IsDeleted && x.IsDefault)
                .Select(x => x.Code)
                .FirstOrDefault();

            var supportedLanguages = _appLanguageRepository.DataSet
              .AsNoTracking()
              .Where(x => !x.IsDeleted)
              .Select(x => x.Code).ToArray();


            if (string.IsNullOrEmpty(defaultLang))
            {
                return (null, null);
            }

            var logoSite = _appSettingLogoRepository.DataSet
              .AsNoTracking()
              .Where(x => !x.IsDeleted)
              .FirstOrDefault();

            // 3) Site ayarları + çeviriler
            var s = _appSettingRepository.DataSet
            .AsNoTracking()
            .Include(i => i.Translations)
            .ThenInclude(tr => tr.AppLanguage) // Dil koduna erişim için
            .FirstOrDefault(x => !x.IsDeleted);

            SiteSettingDto? settingDto = null;
            if (s != null)
            {
                // İstenen dil yoksa → default dil, o da yoksa → ana değerler
                var tr = s.Translations?
                            .FirstOrDefault(x => x.AppLanguage.Code == lang)
                         ?? s.Translations?
                            .FirstOrDefault(x => x.AppLanguage.Code == defaultLang);

                settingDto = new SiteSettingDto
                {
                    SiteTitle = tr?.SiteTitle,
                    Description = tr?.Description,
                    LogoPath = logoSite.LogoPath,
                    MetaDescription = tr.MetaDescription,
                    MetaSlogan = tr.MetaSlogan,
                    MetaTitle =tr.MetaSlogan,
                    FaviconPath = logoSite.FaviconPath,
                    ShareImagePath = logoSite.ShareImagePath,
                };
            }

            SiteTechnicalDto? technicalDto = null;
            if (t != null)
            {
                technicalDto = new SiteTechnicalDto
                {
                    DefaultLanguage = defaultLang,
                    SupportedLanguages = supportedLanguages ?? Array.Empty<string>(),
                    CdnBaseUrl = t.CdnBaseUrl,
                    CdnEnabled = t.CdnEnabled,
                    EnableOutputCache = t.EnableOutputCache,
                    OutputCacheTtlSeconds = t.OutputCacheTtlSeconds,
                    MaintenanceModeEnabled = t.MaintenanceModeEnabled,
                    MaintenanceAllowedIpList = t.MaintenanceAllowedIpList ?? new List<string>(),
                    CookieBannerEnabled = t.CookieBannerEnabled,
                    DomainName = t.DomainName,
                    EnableDebugMode = t.EnableDebugMode,
                    ForceSSL = t.ForceSSL,
                    MaintenanceMessage = t.MaintenanceMessage
                };




            }
            return (settingDto, technicalDto);

        }

        public async Task<List<SlideVm>> GetSlidesAsync(string lang)
        {
            lang = (lang ?? "tr").ToLowerInvariant();

            var langId = await _appLanguageRepository.DataSet
                .Where(l => !l.IsDeleted && l.IsActive && l.Code.ToLower() == lang)
                .Select(l => (int?)l.Id)
                .FirstOrDefaultAsync()
                ?? await _appLanguageRepository.DataSet.Where(l => l.IsDefault).Select(l => l.Id).FirstAsync();

            var data = await _appSlideRepository.DataSet
                .Where(s => !s.IsDeleted && s.IsActive)
                .OrderBy(s => s.SortOrder)
                .Select(s => new
                {
                    s.Id,
                    s.ImagePath,
                    s.IsExternal,
                    s.LinkUrl,
                    s.OpenTarget,
                    T = s.Translations
                        .Where(t => !t.IsDeleted && t.AppLanguageId == langId)
                        .Select(t => new { t.Title, t.Description, t.ButtonText })
                        .FirstOrDefault()
                })
                .Select(x => new SlideVm
                {
                    Id = x.Id,
                    Image = x.ImagePath,
                    Title = x.T != null ? x.T.Title : "",
                    Description = x.T != null ? x.T.Description : "",
                    ButtonText = x.T != null ? x.T.ButtonText : null,
                    IsExternal = x.IsExternal,
                    Url = x.IsExternal ? x.LinkUrl : null, // iç link üretimini UI/route tarafında yap
                    Target = x.OpenTarget // 0:_self, 1:_blank ...
                })
                .ToListAsync();

            return data;
        }
    }

}
