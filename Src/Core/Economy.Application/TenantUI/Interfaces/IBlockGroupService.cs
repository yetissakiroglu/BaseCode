using Economy.Application.TenantUI.Dtos;
using Economy.Core.Tools;
using Economy.Core.Tools.Result;

namespace Economy.Application.TenantUI.Interfaces
{
    public interface IBlockGroupService
    {
        Task<ServiceResult<List<BlockGroupListDto>>> GetGroupsListAsync(CancellationToken ct);
        Task<ServiceResult<NoContent>> FillLanguagesAsync(BlockGroupDto vm, CancellationToken ct);
        Task<ServiceResult<NoContent>> EnsureLanguageTabsAsync(BlockGroupDto vm, CancellationToken ct);
        Task<ServiceResult<NoContent>> CreateGroupAsync(BlockGroupDto vm, CancellationToken ct);
        Task<ServiceResult<NoContent>> UpdateGroupAsync(int id, BlockGroupDto dto, CancellationToken ct);
        Task<ServiceResult<BlockGroupDto>> GetGroupAsync(int id, CancellationToken ct);
        Task<ServiceResult<NoContent>> DeleteGroupAsync(int id, CancellationToken ct);

        Task<ServiceResult<NoContent>> CreateUpdateGroupAndItemsAsync(BlockGroupDto dto, CancellationToken ct);







        Task<List<BlockGroupDto>> GetGroupsAsync(bool includeItems = true);
        //Task<BlockGroupDto?> GetGroupAsync(int id, bool includeItems = true);
        Task UpdateGroupAsync(int id, BlockGroupDto dto);
        Task DeleteGroupAsync(int id);


        Task<int> AddItemAsync(int groupId, BlockItemDto dto);
        Task UpdateItemAsync(int itemId, BlockItemDto dto);
        Task DeleteItemAsync(int itemId);
        Task SortItemsAsync(int groupId, List<(int itemId, int sortOrder)> sortPairs);
    }
}
