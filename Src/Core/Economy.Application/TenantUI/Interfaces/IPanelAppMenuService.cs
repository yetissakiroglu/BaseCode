using Economy.Application.TenantUI.Dtos.AppMenuDtos;
using Economy.Core.Tools.Result;

namespace Economy.Application.Interfaces
{
    public interface IPanelAppMenuService
    {
        Task<ServiceResult<List<MenuNodeDto>>> GetTreeAsync(string location, bool onlyActive);
        Task<ServiceResult<MenuItemDto>> UpsertAsync(MenuItemDto dto, string updatedBy);
        Task<ServiceResult<bool>> DeleteAsync(int id);
        Task<ServiceResult<bool>> ReorderAsync(IEnumerable<MenuReorderItemDto> items, string updatedBy);
    }
}
