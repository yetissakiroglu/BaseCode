using Economy.Application.ApplicationUI.Dtos;
using Economy.Application.ApplicationUI.Interfaces;
using Economy.Core.Interfaces;
using Economy.Domain.Entites.AdminEntity.EntityApp;
using Economy.Domain.Entites.TenantEntity.EntityAppLanguages;
using Economy.Domain.Entites.TenantEntity.EntityAppMenus;
using Economy.Domain.Entites.TenantEntity.EntityAppSettings;
using Economy.UI.Dtos;
using Microsoft.EntityFrameworkCore;

namespace Economy.Persistence.ApplicationUI
{
    public class ApplicationMenuService : IApplicationMenuService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IEntityRepository<AppLanguage, int> _appLanguageRepository;
        private readonly IEntityRepository<AppMenu, int> _appMenuRepository;
        private readonly IEntityRepository<AppSetting, int> _appSettingRepository;
        private readonly IEntityRepository<AppTechnicalSetting, int> _appTechnicalSettingRepository;
        private readonly IEntityRepository<AppSettingLogo, int> _appSettingLogoRepository;
        private readonly IEntityRepository<App, int> _appRepository;

        public ApplicationMenuService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _appMenuRepository = unitOfWork.HotelEntityRepository<AppMenu>();
            _appLanguageRepository = unitOfWork.HotelEntityRepository<AppLanguage>();
            _appSettingRepository = unitOfWork.HotelEntityRepository<AppSetting>();
            _appTechnicalSettingRepository = unitOfWork.HotelEntityRepository<AppTechnicalSetting>();
            _appSettingLogoRepository = unitOfWork.HotelEntityRepository<AppSettingLogo>();
            _appRepository = unitOfWork.DefaultEntityRepository<App>();


        }
        public async Task<TenantDto> GetTenantAsync(string xtanent,CancellationToken ct)
        {
            var app = await _appRepository.DataSet
                .AsNoTracking()
                .FirstOrDefaultAsync(x => !x.IsDeleted && (x.Domain == xtanent), ct);
            if (app is null)
            {
                return null;
            }

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


            TenantDto? technicalDto = null;
            if (t != null)
            {
                technicalDto = new TenantDto
                {   ThemeKey= app.Theme,
                    DefaultLanguage = defaultLang,
                    SupportedLanguages = supportedLanguages ?? Array.Empty<string>(),
                    Settings = new TenantSettingsDto
                    {
                        CdnBaseUrl = t.CdnBaseUrl,
                        EnableCdn = t.CdnEnabled,
                        OutputCacheEnabled = t.EnableOutputCache,
                        OutputCacheTtlSeconds = t.OutputCacheTtlSeconds,
                        MaintenanceMode = t.MaintenanceModeEnabled,
                        MaintenanceAllowedIpList = t.MaintenanceAllowedIpList ?? new List<string>(),
                        ShowCookieBanner = t.CookieBannerEnabled,
                        EnableDebugMode = t.EnableDebugMode,
                        ForceHttps = t.ForceSSL,
                        MaintenanceMessage = t.MaintenanceMessage,
                        CanonicalHost = t.DomainName,
                        Domain = app.Domain
                    },

                };
            }
            return technicalDto;
        }





        public async Task<List<MenuNodeDto>> GetMenuAsync(string lang, CancellationToken ct)
        {
            const int MaxDepth = 3;

            // --- 1. Dil Çözümü ---
            lang = (lang ?? "").Trim().ToLowerInvariant();
            if (string.IsNullOrWhiteSpace(lang))
                return []; // dil belirtilmediyse da boş dön

            var langId = await _appLanguageRepository.DataSet
                .AsNoTracking()
                .Where(x => !x.IsDeleted && x.IsActive && x.Code.ToLower() == lang)
                .Select(x => (int?)x.Id)
                .FirstOrDefaultAsync(ct);

            if (langId is null)
                return []; // sistemde böyle bir dil yoksa, fallback yapma

            // --- 2. Menüleri Çek (çeviriyle birlikte) ---
            var menus = await _appMenuRepository.DataSet
                .AsNoTracking()
                .Where(m => !m.IsDeleted && m.IsActive)
                .Select(m => new
                {
                    m.Id,
                    m.ParentId,
                    m.PageId,
                    m.IsExternal,
                    Order = m.SortOrder,
                    Title = m.Translations
                        .Where(t => !t.IsDeleted && t.AppLanguageId == langId)
                        .Select(t => t.Title)
                        .FirstOrDefault() ?? "",
                    Url = m.Translations
                        .Where(t => !t.IsDeleted && t.AppLanguageId == langId)
                        .Select(t => t.Url)
                        .FirstOrDefault()
                })
                .OrderBy(x => x.ParentId)
                .ThenBy(x => x.Order)
                .ToListAsync(ct);

            if (menus.Count == 0)
                return []; // o dilde hiç menü çevirisi yoksa boş dön

            // --- 3. Ağaç Yapısını Kur ---
            var byParent = menus.ToLookup(m => m.ParentId);

            async Task<MenuNodeDto> MapAsync(dynamic m, int level)
            {
                string url;

                if (m.IsExternal && !string.IsNullOrWhiteSpace(m.Url))
                    url = m.Url!;
                else if (m.PageId is int)
                    url = string.IsNullOrWhiteSpace(m.Url) ? "#" : m.Url!;
                else
                    url = string.IsNullOrWhiteSpace(m.Url) ? "#" : m.Url!;

                var node = new MenuNodeDto(m.Title ?? "", url, m.IsExternal, true, new List<MenuNodeDto>());

                if (level < MaxDepth)
                {
                    foreach (var c in byParent[m.Id])
                        node.Children.Add(await MapAsync(c, level + 1));
                }

                node.Selected = false;
                node.BranchSelected = false;
                return node;
            }

            var tree = new List<MenuNodeDto>();
            foreach (var root in byParent[null])
                tree.Add(await MapAsync(root, 1));

            return tree;
        }
        public async Task<SiteMetaDto?> GetSiteMetaAsync(string lang, CancellationToken ct)
        {
            // 0) Dil gelmemişse hiç dönme
            if (string.IsNullOrWhiteSpace(lang))
                return null;

            var normLang = lang.Trim().ToLowerInvariant();

            // 1) Logolar (opsiyonel)
            var logo = await _appSettingLogoRepository.DataSet
                .AsNoTracking()
                .Where(x => !x.IsDeleted)
                .Select(x => new { x.LogoPath, x.FaviconPath, x.ShareImagePath })
                .FirstOrDefaultAsync(ct);

            // 2) Ayarlar + sadece gerekli çeviri
            var setting = await _appSettingRepository.DataSet
                .AsNoTracking()
                .Where(x => !x.IsDeleted)
                .Select(x => new
                {
                    Translations = x.Translations!
                        .Where(t => !t.IsDeleted)
                        .Select(t => new
                        {
                            LangCode = t.AppLanguage.Code,
                            t.Title,
                            t.Description,
                            t.MetaTitle,
                            t.MetaDescription,
                            t.MetaSlogan
                        })
                        .ToList()
                })
                .FirstOrDefaultAsync(ct);

            if (setting is null)
                return null;

            // 3) Yalnızca istenen dil
            var tr = setting.Translations
                .FirstOrDefault(t => string.Equals(t.LangCode, normLang, StringComparison.OrdinalIgnoreCase));

            if (tr is null)
                return null; // dil yoksa yoktur

            // 4) DTO
            return new SiteMetaDto
            {
                SiteTitle = tr.Title,
                Description = tr.Description,
                MetaTitle = tr.MetaTitle,
                MetaDescription = tr.MetaDescription,
                MetaSlogan = tr.MetaSlogan,
                LogoPath = logo?.LogoPath,
                FaviconPath = logo?.FaviconPath,
                ShareImagePath = logo?.ShareImagePath
            };
        }
    }
}
