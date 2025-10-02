using AutoMapper;
using Economy.Application.TenantUI.Dtos.AppPageDtos;
using Economy.Application.TenantUI.Interfaces;
using Economy.Core.Enums;
using Economy.Core.Interfaces;
using Economy.Core.Tools.Result;
using Economy.Domain.Entites.EntityAppLanguage;
using Economy.Domain.Entites.EntityAppNewPages;
using Economy.Domain.Entites.TenantEntity.EntityAppPages;
using Economy.Domain.Entites.TenantEntity.EntityAppSettings;
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
        public PanelAppPageService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _entityPageRepository = unitOfWork.HotelEntityRepository<ContentItem>();
            _entityLanguageRepository = unitOfWork.DefaultEntityRepository<AppLanguage>();
            _trRepo = unitOfWork.HotelEntityRepository<ContentItemTranslation>();
            _mapper = mapper;
        }

        public async Task<ServiceResult<List<PageListItemDto>>> GetMiniPageItemAsync(bool onlyActive)
        {
            var defLangId = await _entityLanguageRepository.DataSet.Where(l => !l.IsDeleted && l.IsActive && l.IsDefault)
              .Select(l => l.Id).FirstOrDefaultAsync();

            if (defLangId == 0)
                defLangId = await _entityLanguageRepository.DataSet.Where(l => !l.IsDeleted && l.IsActive)
                    .Select(l => l.Id).FirstOrDefaultAsync();

            var list = await(from ci in _entityPageRepository.DataSet
                             where !ci.IsDeleted && ci.Type == ContentItemType.Page && ci.IsActive == onlyActive
                             join tr in _trRepo.DataSet on ci.Id equals tr.ContentItemId into trx
                             from tr in trx.Where(t => !t.IsDeleted && t.LanguageId == defLangId).DefaultIfEmpty()
                             join ptr in _trRepo.DataSet on ci.OwnerId equals ptr.ContentItemId into ptx
                             from ptr in ptx.Where(p => !p.IsDeleted && p.LanguageId == defLangId).DefaultIfEmpty()
                             orderby ci.SortOrder, ci.Id
                             select new PageListItemDto
                             {
                                 Id = ci.Id,
                                 ParentTitle = ptr.Title,
                                 Title = tr.Title,
                                 Slug = tr.Slug,
                                 IsActive = ci.IsActive,
                                 PublishAtUtc = ci.PublishAtUtc,
                                 SortOrder = ci.SortOrder
                             })
                              .ToListAsync();

            return ServiceResult<List<PageListItemDto>>.Success(list);

        }
    }
}
