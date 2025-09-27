using Economy.Application.ApplicationUI.Interfaces;
using Economy.Core.Interfaces;
using Economy.Domain.Entites.AppEntities;
using Economy.Domain.Entites.EntityAppLanguage;
using Economy.Domain.Entites.EntityAppSettings;
using Economy.Domain.Entites.EntityMenuItems;
using Economy.UI.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Economy.Persistence.PersistenceUI.Services
{

    public class MenuAccessor : IMenuAccessor
    {
        private readonly IEntityRepository<AppLanguage, int> _appLanguageRepository;
        private readonly IEntityRepository<AppMenu, int> _appMenuRepository;

        public MenuAccessor(IUnitOfWork unitOfWork)
        {
            _appMenuRepository = unitOfWork.HotelEntityRepository<AppMenu>();
            _appLanguageRepository = unitOfWork.HotelEntityRepository<AppLanguage>();
        }

        public async Task<List<MenuItem>> GetAsync(string lang)
        {
            // 1) Dil Id’sini bul (yoksa 'tr' fallback)
            lang = (lang ?? "tr").ToLowerInvariant();

            var langId = await _appLanguageRepository.DataSet
                .Where(x => !x.IsDeleted && x.IsActive && x.Code.ToLower() == lang)
                .Select(x => (int?)x.Id)
                .FirstOrDefaultAsync();

            if (langId is null)
            {
                langId = await _appLanguageRepository.DataSet
                    .Where(x => !x.IsDeleted && x.IsActive && x.IsDefault)
                    .Select(x => (int?)x.Id)
                    .FirstOrDefaultAsync() ?? 1;

                lang = await _appLanguageRepository.DataSet
                    .Where(x => x.Id == langId)
                    .Select(x => x.Code)
                    .FirstOrDefaultAsync() ?? "tr";
            }

            // 2) Menüler + ilgili dilde tek çeviri (subquery ile)
            var items = await _appMenuRepository.DataSet
                .Where(m => !m.IsDeleted && m.IsActive)
                .Select(m => new
                {
                    m.Id,
                    m.ParentId,
                    m.PageId,
                    m.IsExternal,
                    m.OpenTarget,
                    Order = m.SortOrder,
                    T = m.Translations                 // navigation üzerinden
                        .Where(t => !t.IsDeleted && t.AppLanguageId == langId)
                        .Select(t => new { t.Title, t.Url })
                        .FirstOrDefault()
                })
                .Select(x => new MenuItem
                {
                    Id = x.Id,
                    Lang = lang,
                    Title = x.T != null ? x.T.Title : string.Empty,
                    IsExternal = x.IsExternal,
                    Url = x.IsExternal? (x.T != null ? x.T.Url : null): (x.PageId == null ? x.T.Url : null),
                    PageId = x.PageId,
                    ParentId = x.ParentId,
                    Order = x.Order,
                    IsActive = false
                })
                .OrderBy(x => x.ParentId)
                .ThenBy(x => x.Order)
                .ToListAsync();

            return items;
        }
    }
}

