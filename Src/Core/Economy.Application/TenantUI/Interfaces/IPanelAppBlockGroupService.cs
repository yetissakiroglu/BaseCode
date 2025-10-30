using Economy.Application.TenantUI.Dtos.AppBlockGroupDtos;
using Economy.Core.Tools.Result;

namespace Economy.Application.TenantUI.Interfaces
{
    public interface IPanelAppBlockGroupService
    {
        Task<ServiceResult<List<AppBlockGroupListDto>>> GetAllBlockGroupsListAsync(CancellationToken ct);



        //Task<ServiceResult<NoContent>> FillLanguagesAsync(BlockGroupDto vm, CancellationToken ct);
        //Task<ServiceResult<NoContent>> EnsureLanguageTabsAsync(BlockGroupDto vm, CancellationToken ct);
        //Task<ServiceResult<NoContent>> CreateGroupAsync(BlockGroupDto vm, CancellationToken ct);
        //Task<ServiceResult<NoContent>> UpdateGroupAsync(int id, BlockGroupDto dto, CancellationToken ct);
        //Task<ServiceResult<BlockGroupDto>> GetGroupAsync(int id, CancellationToken ct);
        //Task<ServiceResult<NoContent>> DeleteGroupAsync(int id, CancellationToken ct);
        //Task<ServiceResult<NoContent>> CreateUpdateGroupAndItemsAsync(BlockGroupDto dto, CancellationToken ct);

        //Task<ServiceResult<List<BlockItemDto>>> GetAllBlocksAsync(int? languageId, CancellationToken ct);
        //Task<ServiceResult<List<GroupLayoutItemDto>>> GetGroupLayoutAsync(int groupId, CancellationToken ct);
        //Task<ServiceResult<NoContent>> SaveGroupLayoutAsync(SaveGroupLayoutRequest model, CancellationToken ct);




        //Task<List<BlockGroupDto>> GetGroupsAsync(bool includeItems = true);
        //Task UpdateGroupAsync(int id, BlockGroupDto dto);
        //Task DeleteGroupAsync(int id);

    }
}
