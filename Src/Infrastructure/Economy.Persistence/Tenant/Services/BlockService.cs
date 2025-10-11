using AutoMapper;
using Economy.Application.TenantUI.Dtos;
using Economy.Application.TenantUI.Interfaces;
using Economy.Core.Interfaces;
using Economy.Domain.Entites.TenantEntity.EntityAppBlocks;
using Microsoft.EntityFrameworkCore;

namespace Economy.Persistence.Tenant.Services
{
    public class BlockService : IBlockService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IEntityRepository<BlockGroup, int> _blockGroupRepository;
        private readonly IEntityRepository<BlockItem, int> _blockItemRepository;
        private readonly IEntityRepository<BlockItemImage, int> _blockItemImageRepository;

        

        public BlockService(IMapper mapper, IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _blockGroupRepository = unitOfWork.HotelEntityRepository<BlockGroup>();
            _blockItemRepository = unitOfWork.HotelEntityRepository<BlockItem>();
            _blockItemImageRepository = unitOfWork.HotelEntityRepository<BlockItemImage>();
            _mapper = mapper;
        }
        public async Task<List<BlockGroupDto>> GetGroupsAsync(bool includeItems = true)
        {
            var q = _blockGroupRepository.DataSet.AsQueryable();
            if (includeItems)
                q = q.Include(x => x.Items).ThenInclude(i => i.Gallery);


            var list = await q.OrderBy(x => x.SortOrder).ToListAsync();
            return _mapper.Map<List<BlockGroupDto>>(list);
        }


        public async Task<BlockGroupDto?> GetGroupAsync(int id, bool includeItems = true)
        {
            var q = _blockGroupRepository.DataSet.AsQueryable();
            if (includeItems)
                q = q.Include(x => x.Items).ThenInclude(i => i.Gallery);


            var ent = await q.FirstOrDefaultAsync(x => x.Id == id);
            return ent == null ? null : _mapper.Map<BlockGroupDto>(ent);
        }


        public async Task<int> CreateGroupAsync(BlockGroupDto dto)
        {
            var ent = _mapper.Map<BlockGroup>(dto);
            // Items servis katmanında teker teker eklenecek
            _blockGroupRepository.DataSet.Add(ent);
            await _unitOfWork.SaveHotelChangesAsync();
            return ent.Id;
        }

        public async Task UpdateGroupAsync(int id, BlockGroupDto dto)
        {
            var ent = await _blockGroupRepository.DataSet.FirstOrDefaultAsync(x => x.Id == id);
            if (ent == null) return;


            ent.Code = dto.Code;
            ent.Title = dto.Title;
            ent.Description = dto.Description;
            ent.Columns = dto.Columns;
            ent.DefaultImageMode = dto.DefaultImageMode;
            ent.ShowTitle = dto.ShowTitle;
            ent.ShowDescription = dto.ShowDescription;
            ent.PageId = dto.PageId;

            await _unitOfWork.SaveHotelChangesAsync();
        }

        public async Task DeleteGroupAsync(int id)
        {
            var ent = await _blockGroupRepository.DataSet.Include(x => x.Items).ThenInclude(i => i.Gallery)
            .FirstOrDefaultAsync(x => x.Id == id);
            if (ent == null) return;
            _blockGroupRepository.DataSet.Remove(ent);
            await _unitOfWork.SaveHotelChangesAsync();
        }


        public async Task<int> AddItemAsync(int groupId, BlockItemDto dto)
        {
            var group = await _blockGroupRepository.DataSet.FirstOrDefaultAsync(x => x.Id == groupId);
            if (group == null) return 0;


            var item = _mapper.Map<BlockItem>(dto);
            item.BlockGroupId = groupId;
            _blockItemRepository.DataSet.Add(item);
            await _unitOfWork.SaveHotelChangesAsync();

            // Gallery url’lerini kaydet
            if (dto.Gallery?.Any() == true)
            {
                var imgs = dto.Gallery.Select((url, idx) => new BlockItemImage
                {
                    BlockItemId = item.Id,
                    ImageUrl = url,
                    SortOrder = idx
                });
                _blockItemImageRepository.DataSet.AddRange(imgs);
                await _unitOfWork.SaveHotelChangesAsync();
            }


            return item.Id;
        }
        public async Task UpdateItemAsync(int itemId, BlockItemDto dto)
        {
            var item = await _blockItemRepository.DataSet.Include(x => x.Gallery).FirstOrDefaultAsync(x => x.Id == itemId);
            if (item == null) return;


            item.Title = dto.Title;
            item.Summary = dto.Summary;
            item.CoverImage = dto.CoverImage;
            item.ImageModeOverride = dto.ImageModeOverride;
            item.LinkType = dto.LinkType;
            item.LinkedPageId = dto.LinkedPageId;
            item.ExternalUrl = dto.ExternalUrl;
            item.Target = dto.Target;
            item.ColumnsOverride = dto.ColumnsOverride;
            item.SortOrder = dto.SortOrder;
            item.IsActive = dto.IsActive;


            // Gallery’yi sıfırla ve yeniden yaz
            _blockItemImageRepository.DataSet.RemoveRange(item.Gallery);
            if (dto.Gallery?.Any() == true)
            {
                var imgs = dto.Gallery.Select((url, idx) => new BlockItemImage
                {
                    BlockItemId = item.Id,
                    ImageUrl = url,
                    SortOrder = idx
                });
                await _blockItemImageRepository.DataSet.AddRangeAsync(imgs);
            }


            await _unitOfWork.SaveHotelChangesAsync();
        }


        public async Task DeleteItemAsync(int itemId)
        {
            var item = await _blockItemRepository.DataSet.Include(x => x.Gallery).FirstOrDefaultAsync(x => x.Id == itemId);
            if (item == null) return;
            _blockItemRepository.DataSet.Remove(item);
            await _unitOfWork.SaveHotelChangesAsync();
        }


        public async Task SortItemsAsync(int groupId, List<(int itemId, int sortOrder)> sortPairs)
        {
            var items = await _blockItemRepository.DataSet.Where(x => x.BlockGroupId == groupId).ToListAsync();
            foreach (var (itemId, sort) in sortPairs)
            {
                var it = items.FirstOrDefault(x => x.Id == itemId);
                if (it != null) it.SortOrder = sort;
            }
            await _unitOfWork.SaveHotelChangesAsync();
        }
    }
}