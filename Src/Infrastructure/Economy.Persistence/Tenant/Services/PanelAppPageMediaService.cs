using AutoMapper;
using Economy.Application.TenantUI.Dtos.AppPageDtos;
using Economy.Application.TenantUI.Interfaces;
using Economy.Core.Interfaces;
using Economy.Core.Tools;
using Economy.Core.Tools.Result;
using Economy.Domain.Entites.TenantEntity.EntityAppLanguages;
using Economy.Domain.Entites.TenantEntity.EntityAppPages;
using Microsoft.EntityFrameworkCore;

namespace Economy.Persistence.Tenant.Services
{

    public class PanelAppPageMediaService : IPanelAppPageMediaService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IEntityRepository<AppPageMedia, int> _entityPageMediaRepository;
        private readonly IEntityRepository<AppPageMediaTranslation, int> _trRepo;
        private readonly IEntityRepository<AppLanguage, int> _entityLanguageRepository;
        private readonly IMapper _mapper;
        public PanelAppPageMediaService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _entityPageMediaRepository = unitOfWork.HotelEntityRepository<AppPageMedia>();
            _entityLanguageRepository = unitOfWork.HotelEntityRepository<AppLanguage>();
            _trRepo = unitOfWork.HotelEntityRepository<AppPageMediaTranslation>();
            _mapper = mapper;
        }
        public async Task<ServiceResult<NoContent>> Create(PageMediaEditDto vm, CancellationToken ct)
        {

            var ci = new AppPageMedia
            {
                IsDeleted = false,
                IsActive = vm.IsActive,
                SortOrder = vm.SortOrder,
                AppPageId = vm.AppPageId,
                MediaUrl = vm.MediaUrl,
                IsCover = vm.IsCover
            };

            await _entityPageMediaRepository.DataSet.AddAsync(ci, ct);
            await _unitOfWork.SaveHotelChangesAsync();

            await FillLanguagesAsync(vm, ct);

            foreach (var t in vm.Translations)
            {

                var tr = new AppPageMediaTranslation
                {
                    AppPageMediaId = ci.Id,
                    AppLanguageId = t.AppLanguageId,
                    IsDeleted = false,
                    Alt = t.Alt,
                    Caption = t.Caption
                };
                await _trRepo.DataSet.AddAsync(tr, ct);
            }

            await _unitOfWork.SaveHotelChangesAsync();
            return ServiceResult<NoContent>.Success(new NoContent() { Id = ci.Id });
        }
        public async Task<ServiceResult<NoContent>> Delete(int id, CancellationToken ct)
        {
            var ci = await _entityPageMediaRepository.DataSet.FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted, ct);
            if (ci is null)
            {
                return ServiceResult<NoContent>.Empty();
            }
            ci.IsDeleted = true;
            await _unitOfWork.SaveHotelChangesAsync();
            return ServiceResult<NoContent>.Success(null);
        }
        public async Task<ServiceResult<NoContent>> Edit(int id, PageMediaEditDto vm, CancellationToken ct)
        {
            var ci = await _entityPageMediaRepository.DataSet.FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted, ct);
            if (ci is null)
            {
                return ServiceResult<NoContent>.Empty();
            }

            ci.AppPageId = vm.AppPageId;
            ci.IsActive = vm.IsActive;
            ci.SortOrder = vm.SortOrder;
            ci.MediaUrl = vm.MediaUrl;

            var existing = await _trRepo.DataSet
                .Where(t => !t.IsDeleted && t.AppPageMediaId == id)
                .ToListAsync(ct);

            foreach (var t in vm.Translations)
            {
                var ex = existing.FirstOrDefault(x => x.AppLanguageId == t.AppLanguageId);
                if (ex is null)
                {
                    var tr = new AppPageMediaTranslation
                    {
                        AppPageMediaId = id,
                        AppLanguageId = t.AppLanguageId,
                        IsDeleted = false,
                        Caption = t.Caption,
                        Alt = t.Alt,
                    };
                    await _trRepo.DataSet.AddAsync(tr, ct);
                }
                else
                {
                    ex.Alt = t.Alt;
                    ex.Caption = t.Caption;
                }
            }

            await _unitOfWork.SaveHotelChangesAsync();
            return ServiceResult<NoContent>.Success(new NoContent() { Id = ci.Id });
        }
        public async Task<ServiceResult<NoContent>> EnsureLanguageTabsAsync(PageMediaEditDto vm, CancellationToken ct)
        {
            var exist = vm.Translations.Select(t => t.AppLanguageId).ToHashSet();
            var langs = await _entityLanguageRepository.DataSet.Where(x => !x.IsDeleted && x.IsActive)
                .Select(x => new { x.Id, x.Code, x.Icon }).ToListAsync(ct);

            foreach (var l in langs)
                if (!exist.Contains(l.Id))
                    vm.Translations.Add(new PageMediaTranslationDto { AppLanguageId = l.Id, AppLanguageCode = l.Code, AppLanguageIcon = l.Icon });

            vm.Translations = vm.Translations
                .OrderByDescending(t => t.AppLanguageCode == "tr")
                .ThenBy(t => t.AppLanguageId)
                .ToList();

            return ServiceResult<NoContent>.Success(null);
        }
        public async Task<ServiceResult<NoContent>> FillLanguagesAsync(PageMediaEditDto vm, CancellationToken ct)
        {
            var langs = await _entityLanguageRepository.DataSet
              .Where(x => !x.IsDeleted && x.IsActive)
              .OrderByDescending(x => x.IsDefault)
              .ThenBy(x => x.Id)
              .Select(x => new { x.Id, x.Code, x.Icon })
              .ToListAsync(ct);

            vm.Translations = langs.Select(l => new PageMediaTranslationDto
            {
                AppLanguageId = l.Id,
                AppLanguageCode = l.Code,
                AppLanguageIcon = l.Icon
            }).ToList();

            return ServiceResult<NoContent>.Success(null);
        }
        public async Task<ServiceResult<List<PageMediaListDto>>> GetPageMediaListAsync(int pageId, CancellationToken ct)
        {
            var defLangId = await _entityLanguageRepository.DataSet.Where(l => !l.IsDeleted && l.IsActive && l.IsDefault)
               .Select(l => l.Id).FirstOrDefaultAsync(ct);

            if (defLangId == 0)
                defLangId = await _entityLanguageRepository.DataSet.Where(l => !l.IsDeleted && l.IsActive)
                    .Select(l => l.Id).FirstOrDefaultAsync(ct);

            var list = await (from ci in _entityPageMediaRepository.DataSet
                              where !ci.IsDeleted && ci.AppPageId == pageId
                              join tr in _trRepo.DataSet on ci.Id equals tr.AppPageMediaId into trx
                              from tr in trx.Where(t => !t.IsDeleted && t.AppLanguageId == defLangId).DefaultIfEmpty()
                              orderby ci.SortOrder, ci.Id
                              select new PageMediaListDto
                              {
                                  Id = ci.Id,
                                  Caption = tr.Caption,
                                  Alt = tr.Alt,
                                  IsActive = ci.IsActive,
                                  MediaUrl = ci.MediaUrl,
                                  SortOrder = ci.SortOrder,
                                  IsCover = ci.IsCover

                              })
                              .ToListAsync();

            return ServiceResult<List<PageMediaListDto>>.Success(list);
        }
        public async Task<ServiceResult<PageMediaEditDto>> GetPageMediaAsync(int id, CancellationToken ct)
        {
            var ci = await _entityPageMediaRepository.DataSet.FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted, ct);
            if (ci is null)
            {
                return ServiceResult<PageMediaEditDto>.Empty();
            }

            var vm = new PageMediaEditDto
            {
                Id = ci.Id,
                AppPageId = ci.AppPageId,
                IsActive = ci.IsActive,
                MediaUrl = ci.MediaUrl,
                SortOrder = ci.SortOrder,
                IsCover = ci.IsCover
            };


            await FillLanguagesAsync(vm, ct);

            var trs = await _trRepo.DataSet
                .Where(t => !t.IsDeleted && t.AppPageMediaId == ci.Id)
                .ToListAsync(ct);

            foreach (var t in vm.Translations)
            {
                var hit = trs.FirstOrDefault(x => x.AppLanguageId == t.AppLanguageId);
                if (hit is null) continue;

                t.Id = hit.Id;
                t.Caption = hit.Caption;
                t.Alt = hit.Alt;
            }

            return ServiceResult<PageMediaEditDto>.Success(vm);
        }

        public async Task<ServiceResult<NoContent>> Delete(List<int> excludeIds, int ContentItemId, CancellationToken ct)
        {
            try
            {
                var ci = await _entityPageMediaRepository.DataSet
                .Where(x => !x.IsDeleted
                         && !excludeIds.Contains(x.Id)
                         && x.AppPageId == ContentItemId)
                .ToListAsync(ct);

                if (ci is null)
                {
                    return ServiceResult<NoContent>.Empty();
                }
                foreach (var item in ci)
                {
                    item.IsDeleted = true;

                }
            }
            catch (Exception ex)
            {


            }



            await _unitOfWork.SaveHotelChangesAsync();
            return ServiceResult<NoContent>.Success(null);
        }
    }
}
