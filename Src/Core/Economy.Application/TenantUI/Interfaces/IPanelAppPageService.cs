using Economy.Application.TenantUI.Dtos.AppPageDtos;
using Economy.Core.Tools.Result;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Economy.Application.TenantUI.Interfaces
{
    public interface IPanelAppPageService
    {
        Task<ServiceResult<List<PageListItemDto>>> GetMiniPageItemAsync(bool onlyActive);
    //    Task<ServiceResult<PageItemDto>> UpsertAsync(PageItemDto dto, string updatedBy);
    //    Task<ServiceResult<bool>> DeleteAsync(int id);
    //    Task<ServiceResult<bool>> ReorderAsync(IEnumerable<PageReorderItemDto> items, string updatedBy);
    }
}
