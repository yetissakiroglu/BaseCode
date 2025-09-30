using Economy.Application.TenantUI.Dtos.AppTechnicalSettingDtos;
using Economy.Core.Tools;
using Economy.Core.Tools.Result;

namespace Economy.Application.TenantUI.Interfaces
{
    public interface IPanelAppTechnicalSettingService
    {
        ServiceResult<AppTechnicalSettingDto> SaveAppTechnicalSetting(AppTechnicalSettingCreateEditDto appSettingDto);
        ServiceResult<AppTechnicalSettingDto> GetAppTechnicalSetting(bool isDeleted);
        ServiceResult<AppTechnicalSettingDto> DeleteAppTechnicalSetting(int Id);
    }
}
