using AutoMapper;
using Economy.Application.Interfaces;
using Economy.Application.TenantUI.Dtos.AppMenuDtos;
using Economy.Core.Helpers;
using Economy.Core.Interfaces;
using Economy.Core.Tools.Result;
using Economy.Domain.Entites.TenantEntity.EntityAppLanguages;
using Economy.Domain.Entites.TenantEntity.EntityAppMenus;
using Economy.Panel.Application.Extensions;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace Economy.Persistence.Services
{
    public sealed class PanelAppMenuService : IPanelAppMenuService
    {
        private readonly IEntityRepository<AppMenu, int> _repo;
        private readonly IEntityRepository<AppLanguage, int> _repoLang;

        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IValidator<MenuItemDto> _validatorMenuItemDto;

        public PanelAppMenuService(IUnitOfWork unitOfWork, IFileImageHelperService fileImageHelperService, IValidator<MenuItemDto> validatorMenuItemDto, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _repo = unitOfWork.HotelEntityRepository<AppMenu>();
            _repoLang = unitOfWork.HotelEntityRepository<AppLanguage>();

            _validatorMenuItemDto = validatorMenuItemDto;
            _mapper = mapper;
        }


        public async Task<ServiceResult<bool>> DeleteAsync(int id)
        {
            var hasChildren = await _repo.DataSet.AnyAsync(x => x.ParentId == id);
            if (hasChildren)
                return ServiceResult<bool>.Failure("Önce alt menüleri silmelisiniz.", errorCode: "MENU_HAS_CHILDREN");

            var e = await _repo.DataSet.FirstOrDefaultAsync(x => x.Id == id);
            if (e is null) return ServiceResult<bool>.Failure("Menü bulunamadı.", statusCode: StatusCodes.Status404NotFound);

            _repo.DataSet.Remove(e);
            await _unitOfWork.SaveHotelChangesAsync();

            return ServiceResult<bool>.Success(true, "Menü silindi.", StatusCodes.Status200OK);
        }

        public async Task<ServiceResult<List<MenuNodeDto>>> GetTreeAsync(string location, bool onlyActive)
        {

            var q = _repo.DataSet
                .Include(x => x.Translations)
                .Where(x => x.Location == location)
                .OrderBy(x => x.ParentId).ThenBy(x => x.SortOrder)
                .AsNoTracking();

            if (onlyActive) q = q.Where(x => x.IsActive);

            var flat = await q.ToListAsync();

            var lookup = flat.ToLookup(x => x.ParentId);
            MenuNodeDto Map(AppMenu m)
            {
                return new MenuNodeDto(
                    Id: m.Id,
                    Location: m.Location,
                    PageId: m.PageId,
                    OpenTarget: m.OpenTarget,
                    SortOrder: m.SortOrder,
                    IsActive: m.IsActive,
                    IsExternal: m.IsExternal,
                    ParentId: m.ParentId,
                    Translations: m.Translations
                        .OrderBy(t => t.AppLanguageId)
                        .Select(t => new MenuNodeTranslationDto(t.AppLanguageId, t.Title, t.Url))
                        .ToList(),
                    Children: lookup[m.Id]
                        .OrderBy(c => c.SortOrder)
                        .Select(Map)
                        .ToList()
                );
            }

            var roots = flat.Where(x => x.ParentId == null)
                            .OrderBy(x => x.SortOrder)
                            .Select(Map)
                            .ToList();

            return ServiceResult<List<MenuNodeDto>>.Success(roots, "Menü ağacı getirildi.", StatusCodes.Status200OK);

        }

        public async Task<ServiceResult<bool>> ReorderAsync(IEnumerable<MenuReorderItemDto> items, string updatedBy)
        {
            var ids = items.Select(i => i.Id).Distinct().ToList();
            var map = await _repo.DataSet.Where(x => ids.Contains(x.Id)).ToDictionaryAsync(x => x.Id);

            var now = DateTime.UtcNow;
            foreach (var it in items)
            {
                if (!map.TryGetValue(it.Id, out var e)) continue;
                e.ParentId = it.ParentId;
                e.SortOrder = it.SortOrder;
            }

            await _unitOfWork.SaveHotelChangesAsync();
            return ServiceResult<bool>.Success(true, "Sıralama güncellendi.", StatusCodes.Status200OK);

        }

        public async Task<ServiceResult<MenuItemDto>> UpsertAsync(MenuItemDto dto, string updatedBy)
        {
  
            var validation = _validatorMenuItemDto.Validate(dto);
            if (!validation.IsValid)
            {
                return ServiceResult<MenuItemDto>.Failure(
                    "Doğrulama hatası",
                    validationErrors: validation.ToValidationDictionary()
                );
            }

            // 2) Dil seti doğrulaması
            var langs = await _repoLang.DataSet.AsNoTracking().Select(l => l.Id).ToListAsync();
            var invalidLangs = dto.Translations.Select(t => t.AppLanguageId).Except(langs).ToList();
            if (invalidLangs.Any())
                return ServiceResult<MenuItemDto>.Failure("Geçersiz dil(ler) gönderildi.",
                  statusCode: StatusCodes.Status400BadRequest, errorCode: "MENU_LANG_INVALID");

            // 3) Insert/Update
            var now = DateTime.UtcNow;
                AppMenu entity;
                var isInsert = dto.Id is null;

                if (isInsert)
                {
                    entity = new AppMenu();
                    ApplyScalar(dto, entity, updatedBy, now);
                    _repo.DataSet.Add(entity);
                }
                else
                {
                    entity = await _repo.DataSet
                        .Include(x => x.Translations)
                        .FirstOrDefaultAsync(x => x.Id == dto.Id!.Value)
                        ?? throw new KeyNotFoundException("Menü bulunamadı");

                    ApplyScalar(dto, entity, updatedBy, now);
                    // Translations upsert
                    UpsertTranslations(dto, entity);
                }

                // Insert ise translations da ekle
                if (isInsert) UpsertTranslations(dto, entity);

                await _unitOfWork.SaveHotelChangesAsync();

                var resDto = MapToDto(entity);
                return ServiceResult<MenuItemDto>.Success(
                    resDto,
                    isInsert ? "Menü oluşturuldu." : "Menü güncellendi.",
                    isInsert ? StatusCodes.Status201Created : StatusCodes.Status200OK);
        
        }


        // --- helpers ---
        private static void ApplyScalar(MenuItemDto d, AppMenu e, string updatedBy, DateTime now)
        {
            e.Location = d.Location;
            e.PageId = d.PageId;
            e.OpenTarget = d.OpenTarget;
            e.SortOrder = d.SortOrder;
            e.IsActive = d.IsActive;
            e.IsExternal = d.IsExternal;
            e.ParentId = d.ParentId;
        }

        private static void UpsertTranslations(MenuItemDto d, AppMenu e)
        {
            // mevcutları sözlüğe al (langId -> entity)
            var dict = e.Translations?.ToDictionary(t => t.AppLanguageId) ?? new Dictionary<int, AppMenuTranslation>();

            foreach (var tr in d.Translations)
            {
                if (dict.TryGetValue(tr.AppLanguageId, out var existing))
                {
                    existing.Title = tr.Title?.Trim() ?? "";
                    existing.Url = tr.Url?.Trim() ?? "";
                }
                else
                {
                    e.Translations.Add(new AppMenuTranslation
                    {
                        AppLanguageId = tr.AppLanguageId,
                        Title = tr.Title?.Trim() ?? "",
                        Url = tr.Url?.Trim() ?? ""
                    });
                }
            }

            // DTO’da olmayan dilleri kaldır (temiz upsert)
            var dtoLangIds = d.Translations.Select(x => x.AppLanguageId).ToHashSet();
            var toRemove = e.Translations.Where(t => !dtoLangIds.Contains(t.AppLanguageId)).ToList();
            foreach (var rem in toRemove) e.Translations.Remove(rem);
        }

        private static MenuItemDto MapToDto(AppMenu e)
        {
            return new MenuItemDto(
                Id: e.Id,
                Location: e.Location,
                PageId: e.PageId,
                OpenTarget: e.OpenTarget,
                SortOrder: e.SortOrder,
                IsActive: e.IsActive,
                IsExternal: e.IsExternal,
                ParentId: e.ParentId,
                Translations: e.Translations?
                    .OrderBy(t => t.AppLanguageId)
                    .Select(t => new MenuTranslationDto(t.AppLanguageId, t.Title, t.Url))
                    .ToList() ?? new()
            );
        }
    }
}
