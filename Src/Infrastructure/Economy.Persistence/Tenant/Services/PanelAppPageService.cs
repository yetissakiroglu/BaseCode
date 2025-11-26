using AutoMapper;
using Economy.Application.TenantUI.Dtos;
using Economy.Application.TenantUI.Dtos.AppPageDtos;
using Economy.Application.TenantUI.Interfaces;
using Economy.Core.Dtos.Custom;
using Economy.Core.Enums;
using Economy.Core.Interfaces;
using Economy.Core.Tools;
using Economy.Core.Tools.Result;
using Economy.Domain.Entites.TenantEntity.EntityAppLanguages;
using Economy.Domain.Entites.TenantEntity.EntityAppPages;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Text.RegularExpressions;

namespace Economy.Persistence.Tenant.Services
{
    public class PanelAppPageService : IPanelAppPageService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IEntityRepository<AppPage, int> _entityPageRepository;
        private readonly IEntityRepository<AppPageTranslation, int> _trRepo;
        private readonly IEntityRepository<AppLanguage, int> _entityLanguageRepository;
        private readonly IEntityRepository<AppPageMedia, int> _appPageMedia;

        private readonly IEntityRepository<DefRoomAttribute, int> _defRoomAttributeRepository;
        private readonly IEntityRepository<DefRoomAttributeTranslation, int> _defRoomAttributeTranslationRepository;
        private readonly IEntityRepository<DefRoomAttributeOption, int> _defRoomAttributeOptionRepository;
        private readonly IEntityRepository<DefRoomAttributeOptionTranslation, int> _defRoomAttributeOptionTranslationRepository;
        private readonly IEntityRepository<RoomAttributeValue, int> _roomAttributeValueRepository;

        private readonly IMapper _mapper;

        public PanelAppPageService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _entityPageRepository = unitOfWork.HotelEntityRepository<AppPage>();
            _entityLanguageRepository = unitOfWork.HotelEntityRepository<AppLanguage>();
            _trRepo = unitOfWork.HotelEntityRepository<AppPageTranslation>();
            _appPageMedia = unitOfWork.HotelEntityRepository<AppPageMedia>();
            _defRoomAttributeRepository = unitOfWork.HotelEntityRepository<DefRoomAttribute>();
            _defRoomAttributeTranslationRepository = unitOfWork.HotelEntityRepository<DefRoomAttributeTranslation>();
            _defRoomAttributeOptionRepository = unitOfWork.HotelEntityRepository<DefRoomAttributeOption>();
            _defRoomAttributeOptionTranslationRepository = unitOfWork.HotelEntityRepository<DefRoomAttributeOptionTranslation>();
            _roomAttributeValueRepository = unitOfWork.HotelEntityRepository<RoomAttributeValue>();
            _mapper = mapper;
        }
        public async Task<ServiceResult<NoContent>> Create(PageEditDto vm, CancellationToken ct)
        {

            var ci = new AppPage
            {
                IsDeleted = false,
                IsActive = vm.IsActive,
                Stage = vm.Stage,
                IsHomepage = vm.IsHomepage,
                PublishAtUtc = vm.PublishAtUtc,
                SortOrder = vm.SortOrder,
                Type = ContentItemType.Page,
                AppPageId = vm.AppPageId,
                CoverImageUrl = vm.CoverImageUrl,
                OgImageUrl = vm.OgImageUrl,
            };

            await _entityPageRepository.DataSet.AddAsync(ci, ct);
            await _unitOfWork.SaveHotelChangesAsync();

            foreach (var t in vm.Translations)
            {
                if (string.IsNullOrWhiteSpace(t.Slug) && string.IsNullOrWhiteSpace(t.Title))
                    continue;

                var tr = new AppPageTranslation
                {
                    AppPageId = ci.Id,
                    AppLanguageId = t.AppLanguageId,
                    IsDeleted = false,
                    Slug = t.Slug,
                    Title = t.Title,
                    Summary = t.Summary,
                    Body = t.Body,
                    MetaTitle = t.MetaTitle,
                    MetaDescription = t.MetaDescription,
                };
                await _trRepo.DataSet.AddAsync(tr, ct);
            }

            await _unitOfWork.SaveHotelChangesAsync();
            return ServiceResult<NoContent>.Success(new NoContent() { Id = ci.Id });
        }
        public async Task<ServiceResult<NoContent>> Delete(int id, CancellationToken ct)
        {
            var ci = await _entityPageRepository.DataSet.FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted, ct);
            if (ci is null)
            {
                return ServiceResult<NoContent>.Empty();
            }
            ci.IsDeleted = true;
            await _unitOfWork.SaveHotelChangesAsync();
            return ServiceResult<NoContent>.Success(null);
        }
        public async Task<ServiceResult<NoContent>> Edit(int id, PageEditDto vm, CancellationToken ct)
        {
            var ci = await _entityPageRepository.DataSet.FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted, ct);
            if (ci is null)
            {
                return ServiceResult<NoContent>.Empty();
            }

            ci.AppPageId = vm.AppPageId;
            ci.Stage = vm.Stage;
            ci.IsActive = vm.IsActive;
            ci.IsHomepage = vm.IsHomepage;
            ci.PublishAtUtc = vm.PublishAtUtc;
            ci.SortOrder = vm.SortOrder;
            //ci.CoverImageUrl = vm.Singles?.FirstOrDefault(x => x.Key == "KapakImage")?.Url;
            //ci.OgImageUrl = vm.Singles?.FirstOrDefault(x => x.Key == "OGImage")?.Url;

            var existing = await _trRepo.DataSet.Where(t => !t.IsDeleted && t.AppPageId == id).ToListAsync(ct);
            foreach (var t in vm.Translations)
            {
                var ex = existing.FirstOrDefault(x => x.AppLanguageId == t.AppLanguageId);
                if (ex is null)
                {
                    if (string.IsNullOrWhiteSpace(t.Slug) && string.IsNullOrWhiteSpace(t.Title))
                        continue;

                    var tr = new AppPageTranslation
                    {
                        AppPageId = id,
                        AppLanguageId = t.AppLanguageId,
                        IsDeleted = false,
                        Slug = t.Slug,
                        Title = t.Title,
                        Summary = t.Summary,
                        Body = t.Body,
                        MetaTitle = t.MetaTitle,
                        MetaDescription = t.MetaDescription,
                    };
                    await _trRepo.DataSet.AddAsync(tr, ct);
                }
                else
                {
                    ex.Slug = t.Slug;
                    ex.Title = t.Title;
                    ex.Summary = t.Summary;
                    ex.Body = t.Body;
                    ex.MetaTitle = t.MetaTitle;
                    ex.MetaDescription = t.MetaDescription;
                }
            }

            await _unitOfWork.SaveHotelChangesAsync();

            //var ids = vm.Galleries?.FirstOrDefault(x => x.Key == "GenelImages")?.Items.Select(x => x.Id).ToList();
            //await _panelAppPageMediaService.Delete(ids, (int)vm.Id, ct);

            return ServiceResult<NoContent>.Success(new NoContent() { Id = ci.Id });
        }
        public async Task<ServiceResult<NoContent>> EnsureLanguageTabsAsync(PageEditDto vm, CancellationToken ct)
        {
            var exist = vm.Translations.Select(t => t.AppLanguageId).ToHashSet();
            var langs = await _entityLanguageRepository.DataSet.Where(x => !x.IsDeleted && x.IsActive)
                .Select(x => new { x.Id, x.Code, x.Icon }).ToListAsync(ct);

            foreach (var l in langs)
                if (!exist.Contains(l.Id))
                    vm.Translations.Add(new PageTranslationDto { AppLanguageId = l.Id, AppLanguageCode = l.Code, AppLanguageIcon = l.Icon });

            vm.Translations = vm.Translations
                .OrderByDescending(t => t.AppLanguageCode == "tr")
                .ThenBy(t => t.AppLanguageId)
                .ToList();

            return ServiceResult<NoContent>.Success(null);
        }
        public async Task<ServiceResult<NoContent>> FillLanguagesAsync(PageEditDto vm, CancellationToken ct)
        {
            var langs = await _entityLanguageRepository.DataSet
              .Where(x => !x.IsDeleted && x.IsActive)
              .OrderByDescending(x => x.IsDefault)
              .ThenBy(x => x.Id)
              .Select(x => new { x.Id, x.Code, x.Icon })
              .ToListAsync(ct);

            vm.Translations = langs.Select(l => new PageTranslationDto
            {
                AppLanguageId = l.Id,
                AppLanguageCode = l.Code,
                AppLanguageIcon = l.Icon
            }).ToList();

            return ServiceResult<NoContent>.Success(null);
        }
        public async Task<ServiceResult<List<PageMiniListDto>>> GetPageMiniListAsync(bool onlyActive, CancellationToken ct)
        {
            var defLangId = await _entityLanguageRepository.DataSet.Where(l => !l.IsDeleted && l.IsActive && l.IsDefault)
              .Select(l => l.Id).FirstOrDefaultAsync(ct);

            if (defLangId == 0)
                defLangId = await _entityLanguageRepository.DataSet.Where(l => !l.IsDeleted && l.IsActive)
                    .Select(l => l.Id).FirstOrDefaultAsync(ct);

            var list = await (from ci in _entityPageRepository.DataSet
                              where !ci.IsDeleted && ci.Type == ContentItemType.Page && ci.IsActive == onlyActive
                              join tr in _trRepo.DataSet on ci.Id equals tr.AppPageId into trx
                              from tr in trx.Where(t => !t.IsDeleted && t.AppLanguageId == defLangId).DefaultIfEmpty()
                              join ptr in _trRepo.DataSet on ci.AppPageId equals ptr.AppPageId into ptx
                              from ptr in ptx.Where(p => !p.IsDeleted && p.AppLanguageId == defLangId).DefaultIfEmpty()
                              orderby ci.SortOrder, ci.Id
                              select new PageMiniListDto
                              {
                                  Id = ci.Id,
                                  Title = tr.Title,
                                  SortOrder = ci.SortOrder
                              })
                              .ToListAsync();

            return ServiceResult<List<PageMiniListDto>>.Success(list);

        }
        public async Task<ServiceResult<List<PageListDto>>> GetPageListAsync(CancellationToken ct)
        {
            var defLangId = await _entityLanguageRepository.DataSet.Where(l => !l.IsDeleted && l.IsActive && l.IsDefault)
               .Select(l => l.Id).FirstOrDefaultAsync(ct);

            if (defLangId == 0)
                defLangId = await _entityLanguageRepository.DataSet.Where(l => !l.IsDeleted && l.IsActive)
                    .Select(l => l.Id).FirstOrDefaultAsync(ct);

            var list = await (from ci in _entityPageRepository.DataSet
                              where !ci.IsDeleted && ci.Type == ContentItemType.Page
                              join tr in _trRepo.DataSet on ci.Id equals tr.AppPageId into trx
                              from tr in trx.Where(t => !t.IsDeleted && t.AppLanguageId == defLangId).DefaultIfEmpty()
                              join ptr in _trRepo.DataSet on ci.AppPageId equals ptr.AppPageId into ptx
                              from ptr in ptx.Where(p => !p.IsDeleted && p.AppLanguageId == defLangId).DefaultIfEmpty()
                              orderby ci.SortOrder, ci.Id
                              select new PageListDto
                              {
                                  Id = ci.Id,
                                  ParentTitle = ptr.Title,
                                  Title = tr.Title,
                                  Slug = tr.Slug,
                                  IsActive = ci.IsActive,
                                  Stage = ci.Stage,
                                  IsHomepage = ci.IsHomepage,
                                  PublishAtUtc = ci.PublishAtUtc,
                                  SortOrder = ci.SortOrder,

                              })
                              .ToListAsync();

            return ServiceResult<List<PageListDto>>.Success(list);
        }
        public async Task<ServiceResult<List<PageParentOptionDto>>> GetParentOptionsAsync(CancellationToken ct, int? excludeId = null)
        {
            var q = _entityPageRepository.DataSet.Where(x => !x.IsDeleted && x.IsActive && x.Type == ContentItemType.Page);
            if (excludeId.HasValue) q = q.Where(x => x.Id != excludeId.Value);

            var defLangId = await _entityLanguageRepository.DataSet.Where(l => !l.IsDeleted && l.IsActive && l.IsDefault)
                .Select(l => l.Id).FirstOrDefaultAsync(ct);

            var result = await (from ci in q
                                join tr in _trRepo.DataSet on ci.Id equals tr.AppPageId
                                where !tr.IsDeleted && tr.AppLanguageId == defLangId
                                orderby ci.SortOrder, ci.Id
                                select new PageParentOptionDto { Id = ci.Id, Title = tr.Title ?? ("#" + ci.Id) })
                         .ToListAsync(ct);

            return ServiceResult<List<PageParentOptionDto>>.Success(result);
        }
        public async Task<ServiceResult<PageEditDto>> GetPageAsync(int id, CancellationToken ct)
        {
            var langs = await _entityLanguageRepository.DataSet
                .Where(x => x.IsActive && !x.IsDeleted)
                .ToListAsync(ct);

            if (langs.Count == 0)
                return ServiceResult<PageEditDto>.Empty("Aktif dil bulunamadı.");

            var page = await _entityPageRepository.DataSet
                .Include(x => x.Translations)
                .Include(x => x.Medias)
                    .ThenInclude(x => x.Translations)
                .FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted, ct);

            if (page == null)
                return ServiceResult<PageEditDto>.Empty();

            var vm = new PageEditDto
            {
                Id = page.Id,
                AppPageId = page.AppPageId,
                IsActive = page.IsActive,
                Stage = page.Stage,
                IsHomepage = page.IsHomepage,
                PublishAtUtc = page.PublishAtUtc,
                SortOrder = page.SortOrder,
                Type = page.Type,
                CoverImageUrl = page.CoverImageUrl,
                OgImageUrl = page.OgImageUrl,
                CoverImageMobilUrl = page.CoverImageMobilUrl,
            };

            vm.MediaItems = new List<MediaItem>();

            vm.MediaItems = page.Medias.Select(m =>
            {
                var mediaItem = new MediaItem
                {
                    Id = m.Id,
                    MediaUrl = m.MediaUrl,
                    SortOrder = m.SortOrder,
                    IsCover = m.IsCover,
                };

                foreach (var lang in langs)
                {
                    var tr = m.Translations?.FirstOrDefault(t => t.AppLanguageId == lang.Id);
                    mediaItem.Translations.Add(new MediaItemTranslation
                    {
                        Id = tr?.Id,
                        AppLanguageId = lang.Id,
                        AppLanguageCode = lang.Code,
                        AppLanguageIcon = lang.Icon,
                        Alt = tr?.Alt ?? "",
                        Caption = tr?.Caption ?? ""
                    });
                }

                return mediaItem;
            }).ToList();

            await FillLanguagesAsync(vm, ct);

            var trs = await _trRepo.DataSet
                .Where(t => !t.IsDeleted && t.AppPageId == page.Id)
                .ToListAsync(ct);

            foreach (var t in vm.Translations)
            {
                var hit = trs.FirstOrDefault(x => x.AppLanguageId == t.AppLanguageId);
                if (hit == null) continue;

                t.Id = hit.Id;
                t.Slug = hit.Slug;
                t.Title = hit.Title;
                t.Summary = hit.Summary;
                t.Body = hit.Body;
                t.MetaTitle = hit.MetaTitle;
                t.MetaDescription = hit.MetaDescription;
            }

            return ServiceResult<PageEditDto>.Success(vm);
        }
        public async Task<ServiceResult<NoContent>> CreateEdit(PageEditDto vm, CancellationToken ct)
        {
            var langs = await _entityLanguageRepository.DataSet
                .Where(x => x.IsActive && !x.IsDeleted)
                .ToListAsync(ct);

            if (langs.Count == 0)
                return ServiceResult<NoContent>.Failure("Aktif dil bulunamadı.");

            AppPage page = vm.Id == null
                ? new AppPage()
                : await _entityPageRepository.DataSet
                    .Include(x => x.Translations)
                    .Include(x => x.Medias).ThenInclude(x => x.Translations)
                    .FirstOrDefaultAsync(x => x.Id == vm.Id, ct) ?? new AppPage();

            // Ana alanlar
            page.IsActive = vm.IsActive;
            page.Stage = vm.Stage;
            page.IsHomepage = vm.IsHomepage;
            page.CoverImageUrl = vm.CoverImageUrl;
            page.OgImageUrl = vm.OgImageUrl;
            page.CoverImageMobilUrl = vm.CoverImageMobilUrl;
            page.PublishAtUtc = vm.PublishAtUtc;
            page.SortOrder = vm.SortOrder;
            page.Type = vm.Type;

            // Çeviriler
            foreach (var l in langs)
            {
                var incoming = vm.Translations.FirstOrDefault(x => x.AppLanguageId == l.Id);
                var cur = page.Translations.FirstOrDefault(x => x.AppLanguageId == l.Id);
                if (cur == null)
                {
                    page.Translations.Add(new AppPageTranslation
                    {
                        AppLanguageId = l.Id,
                        Slug = incoming?.Slug,
                        Title = incoming?.Title,
                        Summary = incoming?.Summary,
                        Body = incoming?.Body,
                        MetaTitle = incoming?.MetaTitle,
                        MetaDescription = incoming?.MetaDescription
                    });
                }
                else
                {
                    cur.Slug = incoming?.Slug;
                    cur.Title = incoming?.Title;
                    cur.Summary = incoming?.Summary;
                    cur.Body = incoming?.Body;
                    cur.MetaTitle = incoming?.MetaTitle;
                    cur.MetaDescription = incoming?.MetaDescription;
                }
            }


            if (vm.MediaItems != null)
            {
                var keepIds = vm.MediaItems.Where(i => i.Id.HasValue).Select(i => i.Id!.Value).ToHashSet();
                var toRemove = page.Medias.Where(m => !keepIds.Contains(m.Id)).ToList();
                _appPageMedia.DataSet.RemoveRange(toRemove);

                int order = 0;
                foreach (var m in vm.MediaItems.OrderBy(x => x.SortOrder))
                {
                    var media = m.Id == null || m.Id == 0
                        ? new AppPageMedia { MediaUrl = m.MediaUrl }
                        : page.Medias.FirstOrDefault(x => x.Id == m.Id) ?? new AppPageMedia();

                    media.SortOrder = order++;
                    media.IsCover = m.IsCover;
                    media.MediaUrl = m.MediaUrl;

                    foreach (var l in langs)
                    {
                        var mt = m.Translations.FirstOrDefault(x => x.AppLanguageId == l.Id);
                        var cur = media.Translations.FirstOrDefault(x => x.AppLanguageId == l.Id);
                        if (cur == null)
                            media.Translations.Add(new AppPageMediaTranslation { AppLanguageId = l.Id, Alt = mt?.Alt, Caption = mt?.Caption });
                        else
                        {
                            cur.Alt = mt?.Alt;
                            cur.Caption = mt?.Caption;
                        }
                    }

                    if (m.Id == null || m.Id == 0)
                        page.Medias.Add(media);
                }
            }

            if (vm.Id == null)
                _entityPageRepository.DataSet.Add(page);

            await _unitOfWork.SaveHotelChangesAsync();
            return ServiceResult<NoContent>.Success(new NoContent { Id = page.Id });
        }
        public async Task<ServiceResult<List<PageListDto>>> GetPageListAsync(ContentItemType type, CancellationToken ct)
        {
            var defLangId = await _entityLanguageRepository.DataSet.Where(l => !l.IsDeleted && l.IsActive && l.IsDefault)
               .Select(l => l.Id).FirstOrDefaultAsync(ct);

            if (defLangId == 0)
                defLangId = await _entityLanguageRepository.DataSet.Where(l => !l.IsDeleted && l.IsActive)
                    .Select(l => l.Id).FirstOrDefaultAsync(ct);

            var list = await (from ci in _entityPageRepository.DataSet
                              where !ci.IsDeleted && ci.Type == type
                              join tr in _trRepo.DataSet on ci.Id equals tr.AppPageId into trx
                              from tr in trx.Where(t => !t.IsDeleted && t.AppLanguageId == defLangId).DefaultIfEmpty()
                              join ptr in _trRepo.DataSet on ci.AppPageId equals ptr.AppPageId into ptx
                              from ptr in ptx.Where(p => !p.IsDeleted && p.AppLanguageId == defLangId).DefaultIfEmpty()
                              orderby ci.SortOrder, ci.Id
                              select new PageListDto
                              {
                                  Id = ci.Id,
                                  ParentTitle = ptr.Title,
                                  Title = tr.Title,
                                  Slug = tr.Slug,
                                  IsActive = ci.IsActive,
                                  Stage = ci.Stage,
                                  IsHomepage = ci.IsHomepage,
                                  PublishAtUtc = ci.PublishAtUtc,
                                  SortOrder = ci.SortOrder,
                              })
                              .ToListAsync();

            return ServiceResult<List<PageListDto>>.Success(list);
        }

        /*------ ------*/
        public async Task<ServiceResult<List<RoomAttributeListVm>>> GetRoomAttributeListAsync(CancellationToken ct)
        {
            var defLangId = await _entityLanguageRepository.DataSet.Where(l => !l.IsDeleted && l.IsActive && l.IsDefault)
                .Select(l => l.Id).FirstOrDefaultAsync(ct);

            if (defLangId == 0)
                defLangId = await _entityLanguageRepository.DataSet.Where(l => !l.IsDeleted && l.IsActive)
                    .Select(l => l.Id).FirstOrDefaultAsync(ct);

            var list = await (from ci in _defRoomAttributeRepository.DataSet
                              where !ci.IsDeleted
                              join tr in _defRoomAttributeTranslationRepository.DataSet on ci.Id equals tr.DefRoomAttributeId into trx
                              from tr in trx.Where(t => !t.IsDeleted && t.AppLanguageId == defLangId).DefaultIfEmpty()
                              orderby ci.SortOrder, ci.Id
                              select new RoomAttributeListVm
                              {
                                  Id = ci.Id,
                                  Description = tr.Description,
                                  Code = ci.Code,
                                  Group = ci.Group,
                                  InputType = ci.InputType,
                                  IsActive = ci.IsActive,
                                  IsFilterable = ci.IsFilterable,
                                  IsRequired = ci.IsRequired,
                                  Name = tr.Name,
                                  SortOrder = ci.SortOrder,

                              })
                              .ToListAsync();

            return ServiceResult<List<RoomAttributeListVm>>.Success(list);
        }
        public async Task<ServiceResult<List<RoomAttributeOptionListVm>>> GetRoomAttributeOptionListAsync(int attributeId, CancellationToken ct)
        {
            var defLangId = await _entityLanguageRepository.DataSet.Where(l => !l.IsDeleted && l.IsActive && l.IsDefault)
                .Select(l => l.Id).FirstOrDefaultAsync(ct);

            if (defLangId == 0)
                defLangId = await _entityLanguageRepository.DataSet.Where(l => !l.IsDeleted && l.IsActive)
                    .Select(l => l.Id).FirstOrDefaultAsync(ct);

            var list = await (from ci in _defRoomAttributeOptionRepository.DataSet
                              where !ci.IsDeleted && ci.DefRoomAttributeId == attributeId
                              join tr in _defRoomAttributeOptionTranslationRepository.DataSet on ci.Id equals tr.DefRoomAttributeOptionId into trx
                              from tr in trx.Where(t => !t.IsDeleted && t.AppLanguageId == defLangId).DefaultIfEmpty()
                              orderby ci.SortOrder, ci.Id
                              select new RoomAttributeOptionListVm
                              {
                                  Id = ci.Id,
                                  DefRoomAttributeId = ci.DefRoomAttributeId,
                                  DisplayName = tr.DisplayName,
                                  //AttributeCode = tr.,
                                  IsActive = ci.IsActive,
                                  Value = ci.Value,
                                  SortOrder = ci.SortOrder,

                              })
                              .ToListAsync();

            return ServiceResult<List<RoomAttributeOptionListVm>>.Success(list);
        }
        public async Task<ServiceResult<RoomAttributeEditVm>> GetRoomAttributeAsync(int attributeId, CancellationToken ct)
        {
            var langs = await _entityLanguageRepository.DataSet
                .Where(x => x.IsActive && !x.IsDeleted)
                .ToListAsync(ct);

            if (langs.Count == 0)
                return ServiceResult<RoomAttributeEditVm>.Empty("Aktif dil bulunamadı.");

            var page = await _defRoomAttributeRepository.DataSet
                .Include(x => x.Translations)
                    .FirstOrDefaultAsync(x => x.Id == attributeId && !x.IsDeleted, ct);

            if (page == null)
                return ServiceResult<RoomAttributeEditVm>.Empty();

            var vm = new RoomAttributeEditVm
            {
                Id = page.Id,
                Code = page.Code,
                Group = page.Group,
                InputType = page.InputType,
                IsActive = page.IsActive,
                IsFilterable = page.IsFilterable,
                IsRequired = page.IsRequired,
                SortOrder = page.SortOrder
            };

            await RoomAttributeFillLanguagesAsync(vm, ct);

            var trs = await _defRoomAttributeTranslationRepository.DataSet
                .Where(t => !t.IsDeleted && t.DefRoomAttributeId == page.Id)
                .ToListAsync(ct);

            foreach (var t in vm.Translations)
            {
                var hit = trs.FirstOrDefault(x => x.AppLanguageId == t.AppLanguageId);
                if (hit == null) continue;

                t.Id = hit.Id;
                t.Name = hit.Name;
                t.Description = hit.Description;
            }

            return ServiceResult<RoomAttributeEditVm>.Success(vm);
        }
        public async Task<ServiceResult<NoContent>> RoomAttributeFillLanguagesAsync(RoomAttributeEditVm vm, CancellationToken ct)
        {
            var langs = await _entityLanguageRepository.DataSet
             .Where(x => !x.IsDeleted && x.IsActive)
             .OrderByDescending(x => x.IsDefault)
             .ThenBy(x => x.Id)
             .Select(x => new { x.Id, x.Code, x.Icon })
             .ToListAsync(ct);

            vm.Translations = langs.Select(l => new RoomAttributeTranslationDto
            {
                AppLanguageId = l.Id,
                AppLanguageCode = l.Code,
                AppLanguageIcon = l.Icon
            }).ToList();

            return ServiceResult<NoContent>.Success(null);
        }
        public async Task<ServiceResult<NoContent>> RoomAttributeEnsureLanguageTabsAsync(RoomAttributeEditVm vm, CancellationToken ct)
        {
            var exist = vm.Translations.Select(t => t.AppLanguageId).ToHashSet();
            var langs = await _entityLanguageRepository.DataSet.Where(x => !x.IsDeleted && x.IsActive)
                .Select(x => new { x.Id, x.Code, x.Icon }).ToListAsync(ct);

            foreach (var l in langs)
                if (!exist.Contains(l.Id))
                    vm.Translations.Add(new RoomAttributeTranslationDto { AppLanguageId = l.Id, AppLanguageCode = l.Code, AppLanguageIcon = l.Icon });

            vm.Translations = vm.Translations
                .OrderByDescending(t => t.AppLanguageCode == "tr")
                .ThenBy(t => t.AppLanguageId)
                .ToList();

            return ServiceResult<NoContent>.Success(null);
        }
        public async Task<ServiceResult<RoomAttributeValueEditVm>> GetPageEditAttributesAsync(int pageId, CancellationToken ct)
        {
            // Tüm aktif attribute’ları ve seçeneklerini TR çevirileriyle birlikte çek
            var attrs = await _defRoomAttributeRepository.DataSet
                .Where(a => a.IsActive)
                .Include(a => a.Translations).ThenInclude(t => t.AppLanguage)
                .Include(a => a.Options).ThenInclude(o => o.Translations).ThenInclude(t => t.AppLanguage)
                .OrderBy(a => a.Group)
                .ThenBy(a => a.SortOrder)
                .ToListAsync(ct);

            // Bu sayfaya/odaya ait mevcut değerler + translations
            var values = await _roomAttributeValueRepository.DataSet
                .Where(v => v.AppPageId == pageId)
                .Include(v => v.Translations).ThenInclude(t => t.AppLanguage)
                .ToListAsync(ct);

            var vm = new RoomAttributeValueEditVm
            {
                RoomId = pageId
            };

            foreach (var attr in attrs)
            {
                var groupKey = string.IsNullOrWhiteSpace(attr.Group)
                    ? "Diğer"
                    : attr.Group;

                if (!vm.GroupedAttributes.ContainsKey(groupKey))
                    vm.GroupedAttributes[groupKey] = new List<RoomAttributeItemVm>();

                // Türkçe isim (yoksa code’a düş)
                var trName = attr.Translations
                    .FirstOrDefault(t => t.AppLanguage.Code == "tr")
                    ?.Name ?? attr.Code;

                var currentValue = values.FirstOrDefault(v => v.DefRoomAttributeId == attr.Id);

                var item = new RoomAttributeItemVm
                {
                    AttributeId = attr.Id,
                    Name = trName,
                    Type = attr.InputType,
                    Group = attr.Group ?? ""
                };

                // Option tipiyse seçenekleri hazırla
                if (attr.InputType == "Option")
                {
                    foreach (var opt in attr.Options.Where(o => o.IsActive).OrderBy(o => o.SortOrder))
                    {
                        var optTrName = opt.Translations
                            .FirstOrDefault(t => t.AppLanguage.Code == "tr")
                            ?.DisplayName ?? opt.Value;

                        item.Options.Add(new RoomAttributeOptionVm
                        {
                            Id = opt.Id,
                            DisplayName = optTrName
                        });
                    }

                    if (currentValue != null)
                        item.SelectedOptionId = currentValue.DefRoomAttributeOptionId;
                }
                else if (attr.InputType == "Bool")
                {
                    if (currentValue != null)
                        item.ValueBool = currentValue.ValueBool;
                }
                else if (attr.InputType == "Number")
                {
                    if (currentValue != null)
                        item.ValueInt = currentValue.ValueInt;
                }
                else if (attr.InputType == "Text")
                {
                    if (currentValue != null)
                    {
                        // Eski mantığı koru
                        item.ValueText = currentValue.ValueText;

                        // ➕ Yeni: Text değerlerin dil bazlı çevirilerini doldur
                        item.Translations = currentValue.Translations
                            .Select(t => new RoomAttributeItemTranslationDto
                            {
                                Id = t.Id,
                                AppLanguageId = t.AppLanguageId,
                                AppLanguageCode = t.AppLanguage.Code,
                                AppLanguageIcon = t.AppLanguage.Icon, // AppLanguage'de Icon varsa
                                Text = t.Text
                            })
                            .ToList();
                    }
                    else
                    {
                        var langs = await _entityLanguageRepository.DataSet.Where(x => !x.IsDeleted && x.IsActive)
             .Select(x => new { x.Id, x.Code, x.Icon }).ToListAsync(ct);
                        // ➕ Yeni: Text değerlerin dil bazlı çevirilerini doldur
                        item.Translations = langs
                            .Select(t => new RoomAttributeItemTranslationDto
                            {
                                Id = t.Id,
                                AppLanguageId = t.Id,
                                AppLanguageCode = t.Code,
                                AppLanguageIcon = t.Icon, // AppLanguage'de Icon varsa
                                Text = null
                            })
                            .ToList();
                    }
                }

                vm.GroupedAttributes[groupKey].Add(item);

                // POST sırasında Values sözlüğü dolsun diye AttributeId kaydı da açabiliriz (şart değil ama temiz olur)
                if (!vm.Values.ContainsKey(attr.Id))
                {
                    vm.Values[attr.Id] = new RoomAttributeValueInputVm
                    {
                        AttributeId = attr.Id,
                        OptionId = item.SelectedOptionId,
                        BoolValue = item.ValueBool,
                        IntValue = item.ValueInt,
                        TextValue = item.ValueText
                    };
                }
            }

            return ServiceResult<RoomAttributeValueEditVm>.Success(vm);
        }
        public async Task<ServiceResult<NoContent>> CreateEditRoomAttribute(RoomAttributeEditVm vm, CancellationToken ct)
        {
            var langs = await _entityLanguageRepository.DataSet
                 .Where(x => x.IsActive && !x.IsDeleted)
                 .ToListAsync(ct);

            if (langs.Count == 0)
                return ServiceResult<NoContent>.Failure("Aktif dil bulunamadı.");

            DefRoomAttribute page = vm.Id == null
                ? new DefRoomAttribute()
                : await _defRoomAttributeRepository.DataSet
                    .Include(x => x.Translations)
                    .FirstOrDefaultAsync(x => x.Id == vm.Id, ct) ?? new DefRoomAttribute();

            // Ana alanlar

            page.Code = vm.Code.Trim();
            page.Group = vm.Group.Trim();
            page.InputType = vm.InputType.Trim();
            page.SortOrder = vm.SortOrder;
            page.IsFilterable = vm.IsFilterable;
            page.IsRequired = vm.IsRequired;
            page.IsActive = vm.IsActive;

            // Çeviriler
            foreach (var l in langs)
            {
                var incoming = vm.Translations.FirstOrDefault(x => x.AppLanguageId == l.Id);
                var cur = page.Translations.FirstOrDefault(x => x.AppLanguageId == l.Id);
                if (cur == null)
                {
                    page.Translations.Add(new DefRoomAttributeTranslation
                    {
                        AppLanguageId = l.Id,
                        Name = incoming?.Name,
                        Description = incoming?.Description
                    });
                }
                else
                {
                    cur.Name = incoming?.Name;
                    cur.Description = incoming?.Description;
                }
            }

            if (vm.Id == null)
                _defRoomAttributeRepository.DataSet.Add(page);

            await _unitOfWork.SaveHotelChangesAsync();
            return ServiceResult<NoContent>.Success(new NoContent { Id = page.Id });
        }

        public async Task<ServiceResult<RoomAttributeOptionEditVm>> GetRoomAttributeOptionAsync(int attributeOptionId, CancellationToken ct)
        {
            var langs = await _entityLanguageRepository.DataSet
                .Where(x => x.IsActive && !x.IsDeleted)
                .ToListAsync(ct);

            if (langs.Count == 0)
                return ServiceResult<RoomAttributeOptionEditVm>.Empty("Aktif dil bulunamadı.");

            var page = await _defRoomAttributeOptionRepository.DataSet
                .Include(x => x.Translations)
                .Include(y => y.DefRoomAttribute)
                    .FirstOrDefaultAsync(x => x.Id == attributeOptionId && !x.IsDeleted, ct);

            if (page == null)
                return ServiceResult<RoomAttributeOptionEditVm>.Empty();

            var vm = new RoomAttributeOptionEditVm
            {
                Id = page.Id,
                DefRoomAttributeId = page.DefRoomAttributeId,
                AttributeCode = page.DefRoomAttribute.Code,
                SortOrder = page.SortOrder,
                IsActive = page.IsActive,
                Value = page.Value
            };

            await RoomAttributeOptionFillLanguagesAsync(vm, ct);

            var trs = await _defRoomAttributeOptionTranslationRepository.DataSet
                .Where(t => !t.IsDeleted && t.DefRoomAttributeOptionId == page.Id)
                .ToListAsync(ct);

            foreach (var t in vm.Translations)
            {
                var hit = trs.FirstOrDefault(x => x.AppLanguageId == t.AppLanguageId);
                if (hit == null) continue;

                t.Id = hit.Id;
                t.DisplayName = hit.DisplayName;
            }

            return ServiceResult<RoomAttributeOptionEditVm>.Success(vm);
        }

        public async Task<ServiceResult<NoContent>> RoomAttributeOptionFillLanguagesAsync(RoomAttributeOptionEditVm vm, CancellationToken ct)
        {
            var langs = await _entityLanguageRepository.DataSet
            .Where(x => !x.IsDeleted && x.IsActive)
            .OrderByDescending(x => x.IsDefault)
            .ThenBy(x => x.Id)
            .Select(x => new { x.Id, x.Code, x.Icon })
            .ToListAsync(ct);

            vm.Translations = langs.Select(l => new RoomAttributeOptionTranslationDto
            {
                AppLanguageId = l.Id,
                AppLanguageCode = l.Code,
                AppLanguageIcon = l.Icon
            }).ToList();

            return ServiceResult<NoContent>.Success(null);
        }

        public async Task<ServiceResult<NoContent>> RoomAttributeOptionEnsureLanguageTabsAsync(RoomAttributeOptionEditVm vm, CancellationToken ct)
        {
            var exist = vm.Translations.Select(t => t.AppLanguageId).ToHashSet();
            var langs = await _entityLanguageRepository.DataSet.Where(x => !x.IsDeleted && x.IsActive)
                .Select(x => new { x.Id, x.Code, x.Icon }).ToListAsync(ct);

            foreach (var l in langs)
                if (!exist.Contains(l.Id))
                    vm.Translations.Add(new RoomAttributeOptionTranslationDto { AppLanguageId = l.Id, AppLanguageCode = l.Code, AppLanguageIcon = l.Icon });

            vm.Translations = vm.Translations
                .OrderByDescending(t => t.AppLanguageCode == "tr")
                .ThenBy(t => t.AppLanguageId)
                .ToList();

            return ServiceResult<NoContent>.Success(null);
        }

        public async Task<ServiceResult<NoContent>> CreateEditRoomAttributeOptionAsync(RoomAttributeOptionEditVm vm, CancellationToken ct)
        {
            var langs = await _entityLanguageRepository.DataSet
                 .Where(x => x.IsActive && !x.IsDeleted)
                 .ToListAsync(ct);

            if (langs.Count == 0)
                return ServiceResult<NoContent>.Failure("Aktif dil bulunamadı.");

            DefRoomAttributeOption page = vm.Id == null
                ? new DefRoomAttributeOption()
                : await _defRoomAttributeOptionRepository.DataSet
                    .Include(x => x.Translations)
                    .FirstOrDefaultAsync(x => x.Id == vm.Id, ct) ?? new DefRoomAttributeOption();

            // Ana alanlar

            page.DefRoomAttributeId = vm.DefRoomAttributeId;
            page.Value = vm.Value.Trim();
            page.SortOrder = vm.SortOrder;
            page.IsActive = vm.IsActive;

            // Çeviriler
            foreach (var l in langs)
            {
                var incoming = vm.Translations.FirstOrDefault(x => x.AppLanguageId == l.Id);
                var cur = page.Translations.FirstOrDefault(x => x.AppLanguageId == l.Id);
                if (cur == null)
                {
                    page.Translations.Add(new DefRoomAttributeOptionTranslation
                    {
                        AppLanguageId = l.Id,
                        DisplayName = incoming?.DisplayName
                    });
                }
                else
                {
                    cur.DisplayName = incoming?.DisplayName;
                }
            }

            if (vm.Id == null)
                _defRoomAttributeOptionRepository.DataSet.Add(page);

            await _unitOfWork.SaveHotelChangesAsync();
            return ServiceResult<NoContent>.Success(new NoContent { Id = page.DefRoomAttributeId });
        }

        public async Task<ServiceResult<List<PageListDto>>> GetPageListPagingAsync(
            ContentItemType type,
            int pageNumber,
            int pageSize,
            CancellationToken ct)
        {
            // 🧩 1) Varsayılan dili bul
            var defLangId = await _entityLanguageRepository.DataSet
                .Where(l => !l.IsDeleted && l.IsActive && l.IsDefault)
                .Select(l => l.Id)
                .FirstOrDefaultAsync(ct);

            if (defLangId == 0)
            {
                defLangId = await _entityLanguageRepository.DataSet
                    .Where(l => !l.IsDeleted && l.IsActive)
                    .Select(l => l.Id)
                    .FirstOrDefaultAsync(ct);
            }

            // Güvenlik: pageNumber / pageSize minimum değerleri
            if (pageNumber <= 0) pageNumber = 1;
            if (pageSize <= 0) pageSize = 10;

            // 🧩 2) Temel query (henüz ToList yok!)
            var query =
                from ci in _entityPageRepository.DataSet
                where !ci.IsDeleted && ci.Type == type
                join tr in _trRepo.DataSet on ci.Id equals tr.AppPageId into trx
                from tr in trx.Where(t => !t.IsDeleted && t.AppLanguageId == defLangId).DefaultIfEmpty()
                join ptr in _trRepo.DataSet on ci.AppPageId equals ptr.AppPageId into ptx
                from ptr in ptx.Where(p => !p.IsDeleted && p.AppLanguageId == defLangId).DefaultIfEmpty()
                orderby ci.SortOrder, ci.Id
                select new PageListDto
                {
                    Id = ci.Id,
                    ParentTitle = ptr.Title,
                    Title = tr.Title,
                    Slug = tr.Slug,
                    IsActive = ci.IsActive,
                    Stage = ci.Stage,
                    IsHomepage = ci.IsHomepage,
                    PublishAtUtc = ci.PublishAtUtc,
                    SortOrder = ci.SortOrder,
                };

            // 🧩 3) Toplam kayıt sayısı (sayfalama için)
            var totalItems = await query.CountAsync(ct);

            // 🧩 4) Sayfalı data (Skip + Take)
            var list = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(ct);

            // 🧩 5) PaginationInfo oluştur
            var pagination = new PaginationInfo
            {
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalItems = totalItems
                // TotalPages kendisi hesaplanıyor
            };

            // 🧩 6) ServiceResult ile dön
            return ServiceResult<List<PageListDto>>.Success(
                data: list,
                message: "Sayfalı sayfa listesi getirildi.",
                statusCode: 200,
                pagination: pagination
            );
        }
    }
}
