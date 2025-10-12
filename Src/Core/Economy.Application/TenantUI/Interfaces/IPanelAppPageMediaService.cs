using Economy.Application.TenantUI.Dtos.AppPageMediaDtos;
using Economy.Core.Tools;
using Economy.Core.Tools.Result;

namespace Economy.Application.TenantUI.Interfaces
{
    public interface IPanelAppPageMediaService
    {
        Task<ServiceResult<NoContent>> FillLanguagesAsync(PageMediaEditDto vm, CancellationToken ct);
        Task<ServiceResult<NoContent>> EnsureLanguageTabsAsync(PageMediaEditDto vm, CancellationToken ct);
        Task<ServiceResult<PageMediaEditDto>> GetPageMediaAsync(int id, CancellationToken ct);
        Task<ServiceResult<List<PageMediaListDto>>> GetPageMediaListAsync(int pageId, CancellationToken ct);
        Task<ServiceResult<NoContent>> Create(PageMediaEditDto vm, CancellationToken ct);
        Task<ServiceResult<NoContent>> Edit(int id, PageMediaEditDto vm, CancellationToken ct);
        Task<ServiceResult<NoContent>> Delete(int id, CancellationToken ct);
        Task<ServiceResult<NoContent>> Delete(List<int> excludeIds, int ContentItemId, CancellationToken ct);

    }
}
