using Economy.Application.ApplicationUI.Dtos;
using Economy.Application.ApplicationUI.Interfaces;
using Economy.Core.Interfaces;
using Economy.Domain.Entites.TenantEntity.EntityAppLanguages;
using Economy.Domain.Entites.TenantEntity.EntityAppMenus;
using Microsoft.EntityFrameworkCore;

namespace Economy.Persistence.ApplicationUI
{
    public class ApplicationMenuService : IApplicationMenuService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IEntityRepository<AppLanguage, int> _appLanguageRepository;
        private readonly IEntityRepository<AppMenu, int> _appMenuRepository;

        public ApplicationMenuService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _appMenuRepository = unitOfWork.HotelEntityRepository<AppMenu>();
            _appLanguageRepository = unitOfWork.HotelEntityRepository<AppLanguage>();
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
    }
}
