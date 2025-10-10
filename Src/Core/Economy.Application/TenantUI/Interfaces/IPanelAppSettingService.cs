using Economy.Application.TenantUI.Dtos.AppPageDtos;
using Economy.Application.TenantUI.Dtos.AppSettingDtos;
using Economy.Core.Tools;
using Economy.Core.Tools.Result;

namespace Economy.Application.TenantUI.Interfaces
{
    public interface IPanelAppSettingService
    {
        Task<ServiceResult<NoContent>> FillLanguagesAsync(AppSettingCreateEditDto vm, CancellationToken ct);
        Task<ServiceResult<NoContent>> EnsureLanguageTabsAsync(AppSettingCreateEditDto vm, CancellationToken ct);
        Task<ServiceResult<AppSettingDto>> GetAppSettingAsync(bool isDeleted, CancellationToken ct);
        Task<ServiceResult<NoContent>> Create(AppSettingCreateEditDto vm, CancellationToken ct);
        Task<ServiceResult<NoContent>> Edit(int id, AppSettingCreateEditDto vm, CancellationToken ct);

    }
}
