using Economy.Application.Dtos.AppTechnicalSettingDtos;
using Economy.Core.Tools;
using Economy.Core.Tools.Result;

namespace Economy.Application.Interfaces
{
    public interface IPanelAppTechnicalSettingService
    {

        ServiceResult<AppTechnicalSettingDto> SaveAppTechnicalSetting(AppTechnicalSettingCreateEditDto appSettingDto);
        ServiceResult<AppTechnicalSettingDto> GetAppTechnicalSetting(bool isDeleted);
        ServiceResult<AppTechnicalSettingDto> DeleteAppTechnicalSetting(int Id);


    }
}
