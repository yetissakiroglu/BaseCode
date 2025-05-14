using Economy.Core.Tools;
using Economy.Panel.Application.Dtos.AppSettingDtos;

namespace Economy.Panel.Application.Interfaces
{
    public interface IPanelAppSettingService
    {
        ResponseModel<AppSettingDto> SaveAppSetting(AppSettingCreateEditDto appSettingDto);
        ResponseModel<AppSettingDto> GetAppSetting(bool isDeleted);
        ResponseModel<AppSettingDto> DeleteAppSetting(int Id);
    }
}
