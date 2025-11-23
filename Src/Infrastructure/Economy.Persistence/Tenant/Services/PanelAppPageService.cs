using AutoMapper;
using Economy.Application.TenantUI.Dtos.AppPageDtos;
using Economy.Application.TenantUI.Interfaces;
using Economy.Core.Dtos.Custom;
using Economy.Core.Enums;
using Economy.Core.Interfaces;
using Economy.Core.Tools;
using Economy.Core.Tools.Result;
using Economy.Domain.Entites.TenantEntity.EntityAppLanguages;
using Economy.Domain.Entites.TenantEntity.EntityAppPages;
using Microsoft.EntityFrameworkCore;

namespace Economy.Persistence.Tenant.Services
{
    public class PanelAppPageService : IPanelAppPageService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IEntityRepository<AppPage, int> _entityPageRepository;
        private readonly IEntityRepository<AppPageTranslation, int> _trRepo;
        private readonly IEntityRepository<AppLanguage, int> _entityLanguageRepository;
        private readonly IEntityRepository<AppPageMedia, int> _appPageMedia;

        private readonly IMapper _mapper;
        public PanelAppPageService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _entityPageRepository = unitOfWork.HotelEntityRepository<AppPage>();
            _entityLanguageRepository = unitOfWork.HotelEntityRepository<AppLanguage>();
            _trRepo = unitOfWork.HotelEntityRepository<AppPageTranslation>();
            _appPageMedia = unitOfWork.HotelEntityRepository<AppPageMedia>();
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
                CoverImageUrl = vm.Singles.FirstOrDefault(x => x.Key == "KapakImage").Url,
                OgImageUrl = vm.Singles.FirstOrDefault(x => x.Key == "OGImage").Url,
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
            ci.CoverImageUrl = vm.Singles?.FirstOrDefault(x => x.Key == "KapakImage")?.Url;
            ci.OgImageUrl = vm.Singles?.FirstOrDefault(x => x.Key == "OGImage")?.Url;

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

            var ids = vm.Galleries?.FirstOrDefault(x => x.Key == "GenelImages")?.Items.Select(x => x.Id).ToList();
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
                Type = (short)page.Type,
                CoverImageUrl = page.CoverImageUrl,
                OgImageUrl = page.OgImageUrl,
                Singles = new List<ImageFieldVm>
        {
            new ImageFieldVm { Key = "KapakImage", Label = "Kapak Görseli", Url = page.CoverImageUrl },
            new ImageFieldVm { Key = "OGImage", Label = "OG Görseli", Url = page.OgImageUrl }
        },
                Galleries = new List<GalleryGroupVm>
        {
            new GalleryGroupVm { Key = "GenelImages", Label = "Galeri Fotoğrafları" }
        }
            };

            var gal = vm.Galleries.First();
            gal.CoverUrl = page.Medias.FirstOrDefault(m => m.IsCover)?.MediaUrl;

            gal.Items = page.Medias.Select(m =>
            {
                var mediaItem = new MediaItem
                {
                    Id = m.Id,
                    MediaUrl = m.MediaUrl,
                    SortOrder = m.SortOrder
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

            var gallery = vm.Galleries?.FirstOrDefault(x => x.Key == "GenelImages");
            var items = gallery?.Items ?? new List<MediaItem>();

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
            page.PublishAtUtc = vm.PublishAtUtc;
            page.SortOrder = vm.SortOrder;
            page.Type = ContentItemType.Page;

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

            // Galeri
            if (gallery != null)
            {
                var keepIds = items.Where(i => i.Id.HasValue).Select(i => i.Id!.Value).ToHashSet();
                var toRemove = page.Medias.Where(m => !keepIds.Contains(m.Id)).ToList();
                _appPageMedia.DataSet.RemoveRange(toRemove);

                int order = 0;
                foreach (var m in items.OrderBy(x => x.SortOrder))
                {
                    var media = m.Id == null || m.Id == 0
                        ? new AppPageMedia { MediaUrl = m.MediaUrl }
                        : page.Medias.FirstOrDefault(x => x.Id == m.Id) ?? new AppPageMedia();

                    media.SortOrder = order++;
                    media.IsCover = gallery.CoverUrl == m.MediaUrl;
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
                              //join tr in _trRepo.DataSet on ci.Id equals tr.AppPageId into trx
                              //from tr in trx.Where(t => !t.IsDeleted && t.AppLanguageId == defLangId).DefaultIfEmpty()
                              join ptr in _trRepo.DataSet on ci.AppPageId equals ptr.AppPageId into ptx
                              from ptr in ptx.Where(p => !p.IsDeleted && p.AppLanguageId == defLangId).DefaultIfEmpty()
                              orderby ci.SortOrder, ci.Id
                              select new PageListDto
                              {
                                  Id = ci.Id,
                                  ParentTitle = ptr.Title,
                                  Title = ptr.Title,
                                  Slug = ptr.Slug,
                                  IsActive = ci.IsActive,
                                  Stage = ci.Stage,
                                  IsHomepage = ci.IsHomepage,
                                  PublishAtUtc = ci.PublishAtUtc,
                                  SortOrder = ci.SortOrder,
                              })
                              .ToListAsync();

            return ServiceResult<List<PageListDto>>.Success(list);
        }
    }
}
