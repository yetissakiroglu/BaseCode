using AutoMapper;
using Economy.Application.TenantUI.Dtos;
using Economy.Application.TenantUI.Interfaces;
using Economy.Core.Interfaces;
using Economy.Domain.Entites.TenantEntity.EntityAppBlocks;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Economy.Persistence.Tenant.Services
{
    public class PageBlockService : IPageBlockService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IEntityRepository<BlockGroup, int> _blockGroupRepository;
        private readonly IEntityRepository<BlockItem, int> _blockItemRepository;
        private readonly IEntityRepository<BlockItemImage, int> _blockItemImageRepository;
        private readonly IEntityRepository<PageBlock, int> _pageBlock;

        

        public PageBlockService(IMapper mapper, IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _blockGroupRepository = unitOfWork.HotelEntityRepository<BlockGroup>();
            _blockItemRepository = unitOfWork.HotelEntityRepository<BlockItem>();
            _blockItemImageRepository = unitOfWork.HotelEntityRepository<BlockItemImage>();
            _pageBlock = unitOfWork.HotelEntityRepository<PageBlock>();
            _mapper = mapper;
        }

        public async Task<IReadOnlyList<BlockGroupMiniDto>> ListForPageAsync(int pageId)
        {
            return await (from pb in _pageBlock.DataSet
                          join g in _blockGroupRepository.DataSet on pb.BlockGroupId equals g.Id
                          where pb.PageId == pageId
                          orderby pb.SortOrder
                          select new BlockGroupMiniDto
                          {
                              Id = g.Id,
                              //Title = g.Title,
                              Columns = (int)g.Columns,
                              IsActive = g.IsActive,
                              SortOrder = pb.SortOrder
                          }).ToListAsync();
        }

        public async Task<IReadOnlyList<BlockGroupMiniDto>> ListCandidatesAsync(int pageId, string? q = null)
        {
            var usedIds = _pageBlock.DataSet
                             .Where(x => x.PageId == pageId)
                             .Select(x => x.BlockGroupId);

            var baseQ = _blockGroupRepository.DataSet
                .Where(g => !g.IsDeleted && g.IsActive && !usedIds.Contains(g.Id));

            //if (!string.IsNullOrWhiteSpace(q))
            //    baseQ = baseQ.Where(g => g.Title.Contains(q));

            var based = baseQ.ToList();

            return await baseQ
                //.OrderBy(g => g.Title)
                .Select(g => new BlockGroupMiniDto
                {
                    Id = g.Id,
                    //Title = g.Title,
                    Columns = (int)g.Columns,
                    IsActive = g.IsActive,
                    SortOrder = 0
                }).ToListAsync();
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
