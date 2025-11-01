using AutoMapper;
using Economy.Application.TenantUI.Dtos.AppPageDtos;
using Economy.Application.TenantUI.Interfaces;
using Economy.Core.Interfaces;
using Economy.Domain.Entites.TenantEntity.EntityAppBlocks;
using Microsoft.EntityFrameworkCore;

namespace Economy.Persistence.Tenant.Services
{
    public class PanelPageBlockGroupService : IPanelPageBlockGroupService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IEntityRepository<AppBlockGroup, int> _blockGroupRepository;
        private readonly IEntityRepository<PageBlock, int> _pageBlock;
       

        public PanelPageBlockGroupService(IMapper mapper, IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _blockGroupRepository = unitOfWork.HotelEntityRepository<AppBlockGroup>();
            _pageBlock = unitOfWork.HotelEntityRepository<PageBlock>();
            _mapper = mapper;
        }

        public async Task<IReadOnlyList<BlockGroupMiniDto>> ListForPageAsync(int pageId)
        {
            return await (from pb in _pageBlock.DataSet.AsNoTracking()
                          join g in _blockGroupRepository.DataSet.AsNoTracking() on pb.BlockGroupId equals g.Id
                          where pb.PageId == pageId
                          orderby pb.SortOrder
                          select new BlockGroupMiniDto
                          {
                              Id = g.Id,
                              Title = g.Translations.FirstOrDefault().Title,
                              Columns = (int)g.Columns,
                              IsActive = g.IsActive,
                              SortOrder = pb.SortOrder
                          })
                 .ToListAsync();
        }

        public async Task<IReadOnlyList<BlockGroupMiniDto>> ListCandidatesAsync(int pageId, string? q = null)
        {
            // Bu sayfada zaten kullanılan blok gruplarını bul
            var usedIds = await _pageBlock.DataSet
                .Where(x => x.PageId == pageId)
                .Select(x => x.BlockGroupId)
                .ToListAsync();

            // Kullanılmayan aktif blok gruplarını getir
            var query = _blockGroupRepository.DataSet
                .Include(x => x.Translations)
                .Where(x => x.IsActive && !x.IsDeleted && !usedIds.Contains(x.Id));

            // Arama yapılacaksa
            if (!string.IsNullOrWhiteSpace(q))
                query = query.Where(x => x.Translations.Any(t => t.Title.Contains(q)));

            // Listeyi oluştur
            var list = await query
                .Select(x => new BlockGroupMiniDto
                {
                    Id = x.Id,
                    Title = x.Translations.FirstOrDefault()!.Title,
                    Columns = (int)x.Columns,
                    IsActive = x.IsActive,
                    SortOrder = 0
                })
                .OrderBy(x => x.Title)
                .ToListAsync();

            return list;
        }

        public async Task AttachAsync(int pageId, int blockGroupId)
        {
            // zaten varsa ekleme
            var exists = await _pageBlock.DataSet.AnyAsync(x => x.PageId == pageId && x.BlockGroupId == blockGroupId);
            if (exists) return;

            var maxSort = await _pageBlock.DataSet.Where(x => x.PageId == pageId)
                             .Select(x => (int?)x.SortOrder).MaxAsync() ?? -1;

            _pageBlock.DataSet.Add(new PageBlock
            {
                PageId = pageId,
                BlockGroupId = blockGroupId,
                SortOrder = maxSort + 1
            });
            await _unitOfWork.SaveHotelChangesAsync();
        }

        public async Task DetachAsync(int pageId, int blockGroupId)
        {
            var row = await _pageBlock.DataSet.FirstOrDefaultAsync(x => x.PageId == pageId && x.BlockGroupId == blockGroupId);
            if (row == null) return;
            _pageBlock.DataSet.Remove(row);
            await _unitOfWork.SaveHotelChangesAsync();
        }

        public async Task SortAsync(int pageId, List<(int blockGroupId, int sortOrder)> pairs)
        {
            var map = pairs.ToDictionary(x => x.blockGroupId, x => x.sortOrder);
            var list = await _pageBlock.DataSet.Where(x => x.PageId == pageId).ToListAsync();
            foreach (var pb in list)
                if (map.TryGetValue(pb.BlockGroupId, out var sort)) pb.SortOrder = sort;

            await _unitOfWork.SaveHotelChangesAsync();
        }
    }
}
