using Economy.Application.TenantUI.Dtos.AppPageDtos;
using Economy.Core.Tools;
using Economy.Core.Tools.Result;
using Microsoft.AspNetCore.Mvc;
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
        Task<ServiceResult<List<PageListDto>>> GetPageListsync();
        Task<ServiceResult<NoContent>> FillLanguagesAsync(PageEditDto vm, CancellationToken ct);
        Task<ServiceResult<List<PageParentOptionDto>>> GetParentOptionsAsync(CancellationToken ct, int? excludeId = null);
        Task<ServiceResult<NoContent>> EnsureLanguageTabsAsync(PageEditDto vm, CancellationToken ct);
        Task<ServiceResult<NoContent>> Create(PageEditDto vm, CancellationToken ct);
        Task<ServiceResult<NoContent>> Edit(int id, PageEditDto vm, CancellationToken ct);
        Task<ServiceResult<NoContent>> Delete(int id, CancellationToken ct);
    }
}
