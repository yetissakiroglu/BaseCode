using Economy.Application.ApplicationUI.Interfaces;
using Economy.Application.Extensions;
using Economy.Core.Enums;
using Economy.Core.Interfaces;
using Economy.Core.PagingModels;
using Economy.Domain.Entites.AdminEntity.EntityApp;
using Economy.Domain.Entites.TenantEntity.EntityAppBlocks;
using Economy.Domain.Entites.TenantEntity.EntityAppLanguages;
using Economy.Domain.Entites.TenantEntity.EntityAppMenus;
using Economy.Domain.Entites.TenantEntity.EntityAppPages;
using Economy.Domain.Entites.TenantEntity.EntityAppSettings;
using Economy.UI.Dtos;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using static Economy.Application.Extensions.AppPageExtensions;

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
        private readonly IEntityRepository<AppPage, int> _appPageRepository;
        private readonly IEntityRepository<AppPageTranslation, int> _appPageTranslationRepository;

        private readonly IEntityRepository<PageBlock, int> _pageBlockRepository;
        private readonly IEntityRepository<AppBlockGroup, int> _appBlockGroupRepository;
        private readonly IEntityRepository<AppBlockGroupBlock, int> _appBlockGroupBlockRepository;
        private readonly IEntityRepository<AppBlock, int> _appBlockRepository;


        public ApplicationMenuService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _appMenuRepository = unitOfWork.HotelEntityRepository<AppMenu>();
            _appLanguageRepository = unitOfWork.HotelEntityRepository<AppLanguage>();
            _appSettingRepository = unitOfWork.HotelEntityRepository<AppSetting>();
            _appTechnicalSettingRepository = unitOfWork.HotelEntityRepository<AppTechnicalSetting>();
            _appSettingLogoRepository = unitOfWork.HotelEntityRepository<AppSettingLogo>();
            _appRepository = unitOfWork.DefaultEntityRepository<App>();
            _appPageRepository = unitOfWork.HotelEntityRepository<AppPage>();
            _pageBlockRepository = unitOfWork.HotelEntityRepository<PageBlock>();
            _appBlockGroupRepository = unitOfWork.HotelEntityRepository<AppBlockGroup>();
            _appBlockGroupBlockRepository = unitOfWork.HotelEntityRepository<AppBlockGroupBlock>();
            _appBlockRepository = unitOfWork.HotelEntityRepository<AppBlock>();
            _appPageTranslationRepository = unitOfWork.HotelEntityRepository<AppPageTranslation>();


        }
        public async Task<TenantDto> GetTenantAsync(string xtanent, CancellationToken ct)
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
                .Where(x => !x.IsDeleted && x.IsDefault && x.IsActive)
                .Select(x => x.Code)
                .FirstOrDefault();

            var supportedLanguages = _appLanguageRepository.DataSet
              .AsNoTracking()
              .Where(x => !x.IsDeleted && x.IsActive)
              .Select(x => x.Code).ToArray();


            TenantDto? technicalDto = null;
            if (t != null)
            {
                technicalDto = new TenantDto
                {
                    ThemeKey = app.Theme,
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

                technicalDto.Languages = await _appLanguageRepository.DataSet
                    .AsNoTracking()
                    .Where(x => !x.IsDeleted && x.IsActive)
                    .Select(x => new LanguagesDto
                    {
                        Code = x.Code,
                        Lang = x.Code,
                        Name = x.Name,
                        IsDefault = x.IsDefault,
                        IsRTL = x.IsRTL
                    })
                    .ToListAsync(ct);

            }
            return technicalDto;
        }
        public async Task<List<MenuNodeDto>> GetMenuAsync(string lang, CancellationToken ct)
        {
            const int MaxDepth = 3;

            // --- 1. Dil Çözümü ---
            lang = (lang ?? string.Empty).Trim().ToLowerInvariant();
            if (string.IsNullOrWhiteSpace(lang))
                return new List<MenuNodeDto>();

            var langId = await _appLanguageRepository.DataSet
                .AsNoTracking()
                .Where(x => !x.IsDeleted && x.IsActive && x.Code.ToLower() == lang)
                .Select(x => (int?)x.Id)
                .FirstOrDefaultAsync(ct);

            if (langId == null)
                return new List<MenuNodeDto>();

            // --- 2. Menüleri al ---
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
                        .Where(t => !t.IsDeleted && t.AppLanguageId == langId.Value)
                        .Select(t => t.Title)
                        .FirstOrDefault() ?? string.Empty,

                    MenuUrl = m.Translations
                        .Where(t => !t.IsDeleted && t.AppLanguageId == langId.Value)
                        .Select(t => t.Url)
                        .FirstOrDefault()
                })
                .OrderBy(x => x.ParentId)
                .ThenBy(x => x.Order)
                .ToListAsync(ct);

            if (menus.Count == 0)
                return new List<MenuNodeDto>();

            // --- 3. Page'leri al ve dictionary'ye çevir ---
            var pageIds = menus
                .Where(x => x.PageId.HasValue && x.PageId.Value > 0)
                .Select(x => x.PageId.Value)
                .Distinct()
                .ToList();

            var pageEntities = await _appPageRepository.DataSet.Include(x => x.Translations)
                .AsNoTracking()
                .Where(p => pageIds.Contains(p.Id))
                .ToListAsync(ct);

            // AppPage için flatten dictionary (Id -> PageFlat)
            var pageDict = pageEntities.ToPageFlatDictionary(langId.Value);

            // --- 4. Ağaç için lookup ---
            var byParent = menus.ToLookup(m => m.ParentId);

            async Task<MenuNodeDto> MapAsync(dynamic m, int level)
            {
                string url;

                // (1) External link
                if (m.IsExternal && !string.IsNullOrWhiteSpace(m.MenuUrl))
                {
                    url = m.MenuUrl;
                }
                // (2) Page bağlı ise (PageId > 0 ve dictionary'de varsa)
                else if (m.PageId != null && m.PageId > 0 && pageDict.ContainsKey((int)m.PageId))
                {
                    var slugList = pageDict.BuildSlugPath((int)m.PageId);
                    // /{lang}/slug1/slug2/slug3
                    url = "/" + lang + "/" + string.Join("/", slugList);
                }
                // (3) Menü çevirisinde URL varsa
                else if (!string.IsNullOrWhiteSpace(m.MenuUrl))
                {
                    url = m.MenuUrl;
                }
                else
                {
                    url = "#";
                }

                var node = new MenuNodeDto(
                    m.Title ?? string.Empty,
                    url,
                    m.IsExternal,
                    true,
                    new List<MenuNodeDto>()
                );

                if (level < MaxDepth)
                {
                    foreach (var c in byParent[m.Id])
                        node.Children.Add(await MapAsync(c, level + 1));
                }

                node.Selected = false;
                node.BranchSelected = false;
                return node;
            }

            // --- 5. Root nodları oluştur ---
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






        // -------- Helpers --------

        private static readonly JsonSerializerOptions _jsonOpts = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            ReadCommentHandling = JsonCommentHandling.Skip,
            AllowTrailingCommas = true
        };

        private static T? SafeDeserialize<T>(string? json)
        {
            if (string.IsNullOrWhiteSpace(json)) return default(T);
            try { return JsonSerializer.Deserialize<T>(json, _jsonOpts); }
            catch { return default(T); } // Bozuk JSON UI’yı kırmasın
        }

        private static object? MapShared(BlockType type, string sharedJson)
        {
            switch (type)
            {
                case BlockType.Hero:
                    return SafeDeserialize<HeroSharedVm>(sharedJson) ?? new HeroSharedVm();
                case BlockType.ImageGallery:
                    return SafeDeserialize<GallerySharedVm>(sharedJson) ?? new GallerySharedVm();
                case BlockType.HeroGallery:
                    return SafeDeserialize<GalleryHeroSharedVm>(sharedJson) ?? new GalleryHeroSharedVm();
                default:
                    return SafeDeserialize<object>(sharedJson) ?? new { };
            }
        }

        private static object? MapLocalized(BlockType type, string localizedJson)
        {
            switch (type)
            {
                case BlockType.Hero:
                    return SafeDeserialize<HeroLocVm>(localizedJson) ?? new HeroLocVm();
                case BlockType.Text:
                    return SafeDeserialize<TextLocVm>(localizedJson) ?? new TextLocVm();
                case BlockType.AmenityGroup:
                    return SafeDeserialize<AmenityLocVm>(localizedJson) ?? new AmenityLocVm();
                case BlockType.HeroGallery:
                    return SafeDeserialize<GalleryHeroLocVm>(localizedJson) ?? new GalleryHeroLocVm();

                default:
                    return SafeDeserialize<object>(localizedJson) ?? new { };
            }
        }

        private async Task<int> ResolveLangIdAsync(string? lang, CancellationToken ct)
        {
            lang = (lang ?? "tr").ToLowerInvariant();

            var langId = await _appLanguageRepository.DataSet
                .AsNoTracking()
                .Where(x => !x.IsDeleted && x.IsActive && x.Code.ToLower() == lang)
                .Select(x => (int?)x.Id)
                .FirstOrDefaultAsync(ct);

            if (langId.HasValue) return langId.Value;

            // Fallback: varsayılan dil
            return await _appLanguageRepository.DataSet.AsNoTracking()
                .Where(x => !x.IsDeleted && x.IsActive && x.IsDefault)
                .Select(x => x.Id)
                .FirstAsync(ct);
        }

        private static IQueryable<AppPage> BaseQuery(IEntityRepository<AppPage, int> repo)
        {
            var now = DateTime.UtcNow;

            return repo.DataSet
                .AsNoTracking()
                .Where(p =>
                    p.IsActive &&
                    !p.IsDeleted &&
                    //p.Stage == PageStage.Published &&
                    (p.PublishAtUtc == null || p.PublishAtUtc <= now))
                .Include(p => p.Translations)
                    .ThenInclude(t => t.AppLanguage)
                      .Include(p => p.Medias)
                         .ThenInclude(m => m.Translations);
        }

        private static PageDetailDto ProjectToDto(AppPage p, AppPageTranslation t, string langCode, IReadOnlyList<BlockGroupDto> groups, IReadOnlyList<HreflangVm> Hreflangs, IReadOnlyDictionary<int, PageFlat> pageDict
)
        {
            // Medyaları dil-özgül alt/caption ile eşle
            var list = new List<PageMediaDto>();
            foreach (var m in p.Medias.Where(m => m.IsActive && !m.IsDeleted).OrderBy(m => m.SortOrder))
            {
                var mt = m.Translations.FirstOrDefault(x => x.AppLanguageId == t.AppLanguageId && !x.IsDeleted);
                list.Add(new PageMediaDto(
                    Url: m.MediaUrl,
                    Alt: mt != null ? mt.Alt : null,
                    Caption: mt != null ? mt.Caption : null,
                    IsCover: m.IsCover,
                    SortOrder: m.SortOrder
                ));
            }



            var breadcrumb = BuildBreadcrumbs(p, pageDict, langCode );


            return new PageDetailDto(
                Id: p.Id,
                Lang: langCode,
                Slug: t.Slug,
                IsHomepage: p.IsHomepage,
                Title: t.Title,
                Summary: t.Summary,
                Body: t.Body,
                MetaTitle: t.MetaTitle,
                MetaDescription: t.MetaDescription,
                CoverImageUrl: p.CoverImageUrl,
                OgImageUrl: p.OgImageUrl,
                CoverImageMobilUrl: p.CoverImageMobilUrl,
                Medias: list,
                Groups: groups,
                Hreflangs: Hreflangs,
                Breadcrumbs: breadcrumb

            );
        }

        private async Task<IReadOnlyList<BlockGroupDto>> GetGroupsForPageAsync(int pageId, int langId, CancellationToken ct)
        {
            var langs = await _appLanguageRepository.DataSet.AsNoTracking()
                .Where(x => !x.IsDeleted && x.IsActive)
                .Select(x => new { x.Id, x.IsDefault })
                .ToListAsync(ct);

            var defaultLangId = langs.First(x => x.IsDefault).Id;

            // Page -> PageBlock (grup bağları)
            var pageGroups = await _pageBlockRepository.DataSet
                .AsNoTracking()
                .Where(pb => pb.PageId == pageId)
                .OrderBy(pb => pb.SortOrder)
                .Select(pb => new
                {
                    pb.SortOrder,
                    Group = _appBlockGroupRepository.DataSet
                        .Where(g => g.Id == pb.BlockGroupId && g.IsActive)
                        .Select(g => new
                        {
                            g.Id,
                            g.Columns,
                            g.GroupType,
                            g.ShowSectionTitle,
                            g.ShowTitle,
                            g.ShowDescription,
                            Tr = g.Translations
                                 .Where(tr => tr.AppLanguageId == langId)
                                 .Select(tr => new {tr.SubHeadingTile ,tr.Title, tr.Description, tr.AppLanguageId })
                                 .FirstOrDefault()
                                 ?? g.Translations
                                      .Where(tr => tr.AppLanguageId == defaultLangId)
                                      .Select(tr => new { tr.SubHeadingTile, tr.Title, tr.Description, tr.AppLanguageId })
                                      .FirstOrDefault()
                        })
                        .FirstOrDefault()
                })
                .Where(x => x.Group != null)
                .ToListAsync(ct);

            if (pageGroups.Count == 0)
                return Array.Empty<BlockGroupDto>();

            var groupIds = pageGroups.Select(x => x.Group.Id).Distinct().ToArray();

            // Grup -> Bloklar
            var groupBlocks = await _appBlockGroupBlockRepository.DataSet
                .AsNoTracking()
                .Where(bb => groupIds.Contains(bb.AppBlockGroupId) && bb.IsActive)
                .OrderBy(bb => bb.SortOrder)
                .Select(bb => new
                {
                    bb.AppBlockGroupId,
                    bb.SortOrder,
                    bb.Column,
                    Block = _appBlockRepository.DataSet
                        .Where(b => b.Id == bb.AppBlockId && b.IsActive)
                        .Select(b => new
                        {
                            b.Id,
                            b.Type,
                            b.Tag,
                            b.SharedJson,
                            Loc = b.Translations
                                 .Where(t => t.AppLanguageId == langId)
                                 .Select(t => new { t.LocalizedJson, t.AppLanguageId })
                                 .FirstOrDefault()
                                 ?? b.Translations
                                      .Where(t => t.AppLanguageId == defaultLangId)
                                      .Select(t => new { t.LocalizedJson, t.AppLanguageId })
                                      .FirstOrDefault()
                        })
                        .FirstOrDefault()
                })
                .Where(x => x.Block != null)
                .ToListAsync(ct);

            var blocksByGroup = new Dictionary<int, List<BlockDto>>();
            foreach (var it in groupBlocks)
            {
                var b = it.Block;

                var shared = MapShared(b.Type, b.SharedJson ?? "{}");
                var localizedJson = b.Loc != null ? b.Loc.LocalizedJson : "{}";
                var localized = MapLocalized(b.Type, localizedJson);

                var dto = new BlockDto(
                    Id: b.Id,
                    Type: b.Type,
                    Tag: b.Tag,
                    Column: it.Column,
                    SortOrder: it.SortOrder,
                    Shared: shared,
                    Localized: localized
                );

                List<BlockDto> list;
                if (!blocksByGroup.TryGetValue(it.AppBlockGroupId, out list))
                {
                    list = new List<BlockDto>();
                    blocksByGroup[it.AppBlockGroupId] = list;
                }
                list.Add(dto);
            }

            var result = new List<BlockGroupDto>(pageGroups.Count);
            foreach (var pg in pageGroups)
            {
                var g = pg.Group;

                List<BlockDto> list;
                if (!blocksByGroup.TryGetValue(g.Id, out list))
                    list = new List<BlockDto>();

                // SortOrder korunuyor (PageBlock.SortOrder)
                result.Add(new BlockGroupDto(
                    Id: g.Id,
                    Columns: g.Columns,
                    GroupType: g.GroupType,
                    ShowSectionTitle: g.ShowSectionTitle,
                    ShowTitle: g.ShowTitle,
                    ShowDescription: g.ShowDescription,
                    SubHeadingTile: g.Tr != null ? g.Tr.SubHeadingTile : null,
                    Title: g.Tr != null ? g.Tr.Title : null,
                    Description: g.Tr != null ? g.Tr.Description : null,
                    SortOrder: pg.SortOrder,
                    Blocks: list.OrderBy(x => x.SortOrder).ToList()
                ));
            }

            return result;
        }
        private static string Join2(string lang, string slug) => "/" + lang + "/" + slug;
        private static string Join3(string lang, string parent, string slug) => "/" + lang + "/" + parent + "/" + slug;

        private static IReadOnlyList<BreadcrumbItemDto> BuildBreadcrumbs(AppPage page, IReadOnlyDictionary<int, AppPageExtensions.PageFlat> dict, string lang)
        {
            var items = new List<BreadcrumbItemDto>();

            // 1) Parent zinciri slug listesi
            var slugList = dict.BuildSlugPath(page.Id);

            // 2) Tek tek breadcrumb item üret
            // Her slug için URL: /{lang}/{slug1}/{slug2}/...
            for (int i = 0; i < slugList.Count; i++)
            {
                var slug = slugList[i];
                var url = "/" + lang + "/" + string.Join("/", slugList.Take(i + 1));

                // Title dictionary'den (PageFlat Slug → Title çekmemiz lazım)
                // Dil çevirisi üzerinden Title alacağız:
                var pg = dict.Values.FirstOrDefault(x => x.Slug == slug);
                var title = pg?.Slug ?? slug; // istersen Title alanı ekleyebilirim

                items.Add(new BreadcrumbItemDto(title, url, false));
            }

            // 3) Son breadcrumb aktif olsun
            if (items.Count > 0)
            {
                var last = items.Last();
                items[items.Count - 1] = last with { Active = true };
            }

            return items;
        }


        public async Task<PageDetailDto?> GetHomepageAsync(string lang, CancellationToken ct)
        {
            var langId = await ResolveLangIdAsync(lang, ct);

            var query = BaseQuery(_appPageRepository)
                .Where(p => p.IsHomepage)
                .Select(p => new
                {
                    Page = p,
                    Tr = p.Translations
                         .Where(tr => tr.AppLanguageId == langId)
                         .FirstOrDefault()
                });

            var item = await query.FirstOrDefaultAsync(ct);
            if (item == null || item.Tr == null)
                return null;

            var langCode = await _appLanguageRepository.DataSet.AsNoTracking()
                .Where(x => x.Id == item.Tr.AppLanguageId)
                .Select(x => x.Code)
                .FirstAsync(ct);

            var groups = await GetGroupsForPageAsync(item.Page.Id, item.Tr.AppLanguageId, ct);


            var hreflangs = await (from t in _appPageTranslationRepository.DataSet
                                   where !t.IsDeleted && t.AppPageId == item.Page.Id
                                   join la in _appLanguageRepository.DataSet on t.AppLanguageId equals la.Id
                                   where !la.IsDeleted && la.IsActive
                                   orderby la.Code
                                   select new HreflangVm
                                   {
                                       Title = t.Title,
                                       Lang = la.Code,
                                       Slug = t.Slug ?? "",
                                       Url = Join2(la.Code, t.Slug ?? "")
                                   })
                          .ToListAsync(ct);

                                var pageDict = await _appPageRepository.DataSet
                        .AsNoTracking()
                        .Where(p => p.Id == item.Page.Id)
                        .Select(p => new PageFlat
                        {
                            Id = p.Id,
                            ParentId = p.AppPageId,
                            Slug = p.Translations
                                    .Where(t => !t.IsDeleted && t.AppLanguageId == langId)
                                    .Select(t => t.Slug)
                                    .FirstOrDefault(),
                            Title = p.Translations
                                     .Where(t => !t.IsDeleted && t.AppLanguageId == langId)
                                    .Select(t => t.Title)
                                    .FirstOrDefault()
                        })
                        .ToDictionaryAsync(x => x.Id, x => x, ct);



            return ProjectToDto(item.Page, item.Tr, langCode, groups, hreflangs, pageDict);


        }

        public async Task<PageDetailDto?> GetBySlugAsync(string lang, string slug, CancellationToken ct)
        {
            var langId = await ResolveLangIdAsync(lang, ct);
            slug = (slug ?? string.Empty).Trim().ToLowerInvariant();

            // Önce istenen dilde slug
            var queryExact = BaseQuery(_appPageRepository)
                .SelectMany(p => p.Translations
                    .Where(tr => tr.AppLanguageId == langId && tr.Slug.ToLower() == slug),
                    (p, tr) => new { Page = p, Tr = tr });

            var selected = await queryExact.FirstOrDefaultAsync(ct);

            if (selected == null)
            {
                // Fallback: varsayılan dilde slug
                var defaultLangId = await _appLanguageRepository.DataSet.AsNoTracking()
                    .Where(x => x.IsDefault)
                    .Select(x => x.Id)
                    .FirstAsync(ct);

                var queryFallback = BaseQuery(_appPageRepository)
                    .SelectMany(p => p.Translations
                        .Where(tr => tr.AppLanguageId == defaultLangId && tr.Slug.ToLower() == slug),
                        (p, tr) => new { Page = p, Tr = tr });

                selected = await queryFallback.FirstOrDefaultAsync(ct);
                if (selected == null)
                    return null;
            }

            var langCode = await _appLanguageRepository.DataSet.AsNoTracking()
                .Where(x => x.Id == selected.Tr.AppLanguageId)
                .Select(x => x.Code)
                .FirstAsync(ct);

            var groups = await GetGroupsForPageAsync(selected.Page.Id, selected.Tr.AppLanguageId, ct);


            var hreflangs = await (from t in _appPageTranslationRepository.DataSet
                                   where !t.IsDeleted && t.AppPageId == selected.Page.Id
                                   join la in _appLanguageRepository.DataSet on t.AppLanguageId equals la.Id
                                   where !la.IsDeleted && la.IsActive
                                   orderby la.Code
                                   select new HreflangVm
                                   {
                                       Lang = la.Code,
                                       Slug = t.Slug ?? "",
                                       Url = Join2(la.Code, t.Slug ?? "")
                                   })
                           .ToListAsync(ct);

            var pageDict = await _appPageRepository.DataSet
.AsNoTracking()
.Where(p => p.Id == selected.Page.Id)
.Select(p => new PageFlat
{
Id = p.Id,
ParentId = p.AppPageId,
Slug = p.Translations
        .Where(t => !t.IsDeleted && t.AppLanguageId == langId)
        .Select(t => t.Slug)
        .FirstOrDefault(),
Title = p.Translations
         .Where(t => !t.IsDeleted && t.AppLanguageId == langId)
        .Select(t => t.Title)
        .FirstOrDefault()
})
.ToDictionaryAsync(x => x.Id, x => x, ct);



            return ProjectToDto(selected.Page, selected.Tr, langCode, groups, hreflangs, pageDict);
        }
    }
}

