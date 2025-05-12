using Economy.Core.Tools;
using Economy.Panel.Application.Dtos.AppSettingDtos;

namespace Economy.Panel.Application.Interfaces
{
    public interface IPanelAppSettingService
    {
        Task<ResponseModel<AppSettingDto>> CreateAppSetting(AppSettingCreateDto appSettingCreateDto);
        Task<ResponseModel<AppSettingDto>> EditAppSetting(AppSettingEditDto appSettingEditDto);
        ResponseModel<AppSettingDto> GetAppSetting(int id, bool isDeleted);
        ResponseModel<AppSettingDto> DeleteAppSetting(int Id);
    }
}
