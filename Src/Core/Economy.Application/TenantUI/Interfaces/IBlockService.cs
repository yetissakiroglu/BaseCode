using Economy.Application.TenantUI.Dtos;

namespace Economy.Application.TenantUI.Interfaces
{
    public interface IBlockService
    {
        Task<List<BlockGroupDto>> GetGroupsAsync(bool includeItems = true);
        Task<BlockGroupDto?> GetGroupAsync(int id, bool includeItems = true);
        Task<int> CreateGroupAsync(BlockGroupDto dto);
        Task UpdateGroupAsync(int id, BlockGroupDto dto);
        Task DeleteGroupAsync(int id);


        Task<int> AddItemAsync(int groupId, BlockItemDto dto);
        Task UpdateItemAsync(int itemId, BlockItemDto dto);
        Task DeleteItemAsync(int itemId);
        Task SortItemsAsync(int groupId, List<(int itemId, int sortOrder)> sortPairs);
    }
}
