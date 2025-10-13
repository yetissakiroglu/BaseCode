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
        private readonly IEntityRepository<ContentItem, int> _entityPageRepository;
        private readonly IEntityRepository<ContentItemTranslation, int> _trRepo;
        private readonly IEntityRepository<AppLanguage, int> _entityLanguageRepository;
        private readonly IMapper _mapper;
        private readonly IPanelAppPageMediaService _panelAppPageMediaService;
        public PanelAppPageService(IUnitOfWork unitOfWork, IMapper mapper, IPanelAppPageMediaService panelAppPageMediaService)
        {
            _unitOfWork = unitOfWork;
            _entityPageRepository = unitOfWork.HotelEntityRepository<ContentItem>();
            _entityLanguageRepository = unitOfWork.HotelEntityRepository<AppLanguage>();
            _trRepo = unitOfWork.HotelEntityRepository<ContentItemTranslation>();
            _mapper = mapper;
            _panelAppPageMediaService = panelAppPageMediaService;
        }
        public async Task<ServiceResult<NoContent>> Create(PageEditDto vm, CancellationToken ct)
        {

            var ci = new ContentItem
            {
                IsDeleted = false,
                IsActive = vm.IsActive,
                IsHomepage = vm.IsHomepage,
                PublishAtUtc = vm.PublishAtUtc,
                SortOrder = vm.SortOrder,
                Type = ContentItemType.Page,
                OwnerId = vm.OwnerId,
                Image = vm.Singles.FirstOrDefault(x => x.Key == "KapakImage").Url,
                OgImage = vm.Singles.FirstOrDefault(x => x.Key == "OGImage").Url,
            };

            await _entityPageRepository.DataSet.AddAsync(ci, ct);
            await _unitOfWork.SaveHotelChangesAsync();

            foreach (var t in vm.Translations)
            {
                if (string.IsNullOrWhiteSpace(t.Slug) && string.IsNullOrWhiteSpace(t.Title))
                    continue;

                var tr = new ContentItemTranslation
                {
                    ContentItemId = ci.Id,
                    LanguageId = t.LanguageId,
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

            ci.OwnerId = vm.OwnerId;
            ci.IsActive = vm.IsActive;
            ci.IsHomepage = vm.IsHomepage;
            ci.PublishAtUtc = vm.PublishAtUtc;
            ci.SortOrder = vm.SortOrder;
            ci.Image = vm.Singles.FirstOrDefault(x => x.Key == "KapakImage").Url;
            ci.OgImage = vm.Singles.FirstOrDefault(x => x.Key == "OGImage").Url;

            var existing = await _trRepo.DataSet
                .Where(t => !t.IsDeleted && t.ContentItemId == id)
                .ToListAsync(ct);

            foreach (var t in vm.Translations)
            {
                var ex = existing.FirstOrDefault(x => x.LanguageId == t.LanguageId);
                if (ex is null)
                {
                    if (string.IsNullOrWhiteSpace(t.Slug) && string.IsNullOrWhiteSpace(t.Title))
                        continue;

                    var tr = new ContentItemTranslation
                    {
                        ContentItemId = id,
                        LanguageId = t.LanguageId,
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

            var ids = vm.Galleries.FirstOrDefault(x => x.Key == "GenelImages").Items.Select(x => x.Id).ToList();
           await _panelAppPageMediaService.Delete(ids, (int)vm.Id, ct);

            return ServiceResult<NoContent>.Success(new NoContent() { Id = ci.Id });
        }
        public async Task<ServiceResult<NoContent>> EnsureLanguageTabsAsync(PageEditDto vm, CancellationToken ct)
        {
            var exist = vm.Translations.Select(t => t.LanguageId).ToHashSet();
            var langs = await _entityLanguageRepository.DataSet.Where(x => !x.IsDeleted && x.IsActive)
                .Select(x => new { x.Id, x.Code ,x.Icon}).ToListAsync(ct);

            foreach (var l in langs)
                if (!exist.Contains(l.Id))
                    vm.Translations.Add(new PageTranslationDto { LanguageId = l.Id, LanguageCode = l.Code, LanguageIcon = l.Icon });

            vm.Translations = vm.Translations
                .OrderByDescending(t => t.LanguageCode == "tr")
                .ThenBy(t => t.LanguageId)
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
                LanguageId = l.Id,
                LanguageCode = l.Code,
                LanguageIcon = l.Icon
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
                              join tr in _trRepo.DataSet on ci.Id equals tr.ContentItemId into trx
                              from tr in trx.Where(t => !t.IsDeleted && t.LanguageId == defLangId).DefaultIfEmpty()
                              join ptr in _trRepo.DataSet on ci.OwnerId equals ptr.ContentItemId into ptx
                              from ptr in ptx.Where(p => !p.IsDeleted && p.LanguageId == defLangId).DefaultIfEmpty()
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
                              join tr in _trRepo.DataSet on ci.Id equals tr.ContentItemId into trx
                              from tr in trx.Where(t => !t.IsDeleted && t.LanguageId == defLangId).DefaultIfEmpty()
                              join ptr in _trRepo.DataSet on ci.OwnerId equals ptr.ContentItemId into ptx
                              from ptr in ptx.Where(p => !p.IsDeleted && p.LanguageId == defLangId).DefaultIfEmpty()
                              orderby ci.SortOrder, ci.Id
                              select new PageListDto
                              {
                                  Id = ci.Id,
                                  ParentTitle = ptr.Title,
                                  Title = tr.Title,
                                  Slug = tr.Slug,
                                  IsActive = ci.IsActive,
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
                                join tr in _trRepo.DataSet on ci.Id equals tr.ContentItemId
                                where !tr.IsDeleted && tr.LanguageId == defLangId
                                orderby ci.SortOrder, ci.Id
                                select new PageParentOptionDto { Id = ci.Id, Title = tr.Title ?? ("#" + ci.Id) })
                         .ToListAsync(ct);

            return ServiceResult<List<PageParentOptionDto>>.Success(result);
        }
        public async Task<ServiceResult<PageEditDto>> GetPageAsync(int id, CancellationToken ct)
        {
            var ci = await _entityPageRepository.DataSet.FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted, ct);
            if (ci is null)
            {
                return ServiceResult<PageEditDto>.Empty();
            }

            var vm = new PageEditDto
            {
                Id = ci.Id,
                OwnerId = ci.OwnerId,
                IsActive = ci.IsActive,
                IsHomepage = ci.IsHomepage,
                PublishAtUtc = ci.PublishAtUtc,
                SortOrder = ci.SortOrder,
                Type = (short)ci.Type,
            };

            vm.Singles = new List<ImageFieldVm>()
            {
                new ImageFieldVm { Key = "KapakImage", Label = "Kapak Görseli",Url = ci.Image },
                new ImageFieldVm { Key ="OGImage", Label="OG Görseli", Url = ci.OgImage}
            };


            vm.Galleries = new List<GalleryGroupVm>()
            {
                new GalleryGroupVm { Key = "GenelImages", Label = "Galeri Fotoğrafları" },
            };

            var galeri = await _panelAppPageMediaService.GetPageMediaListAsync(ci.Id, ct);
            if (galeri.IsSuccess)
            {
                var gal = vm.Galleries.FirstOrDefault(x => x.Key == "GenelImages");
                if (gal != null)
                {
                    foreach (var item in galeri.Data)
                    {
                        if (item.IsCover)
                        {
                            gal.CoverUrl = item.MediaUrl;
                        }
                    }
                    gal.Items = galeri.Data.Select(x => new MediaItem
                    {
                        Id = x.Id,
                        MediaUrl = x.MediaUrl,
                    }).ToList();
                }
            }



            await FillLanguagesAsync(vm, ct);

            var trs = await _trRepo.DataSet
                .Where(t => !t.IsDeleted && t.ContentItemId == ci.Id)
                .ToListAsync(ct);

            foreach (var t in vm.Translations)
            {
                var hit = trs.FirstOrDefault(x => x.LanguageId == t.LanguageId);
                if (hit is null) continue;

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
    }
}
