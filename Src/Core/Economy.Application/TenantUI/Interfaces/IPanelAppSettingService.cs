using Economy.Application.TenantUI.Dtos.AppSettingDtos;
using Economy.Core.Tools;
using Economy.Core.Tools.Result;

namespace Economy.Application.TenantUI.Interfaces
{
    public interface IPanelAppSettingService
    {
        ServiceResult<AppSettingDto> SaveAppSetting(AppSettingCreateEditDto appSettingDto);
        ServiceResult<AppSettingDto> GetAppSetting(bool isDeleted);
        ServiceResult<AppSettingDto> DeleteAppSetting(int Id);
    }
}
