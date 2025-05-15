using Economy.Core.Tools;
using Economy.Panel.Application.Dtos.AppSettingLogoDtos;

namespace Economy.Panel.Application.Interfaces
{
    public interface IPanelAppSettingLogoService
    {
        ResponseModel<AppSettingLogoDto> GetAppSettingLogo(bool isDeleted);
        ResponseModel<AppSettingLogoDto> EditAppSettingLogo(AppSettingLogoEditDto model);
        ResponseModel<AppSettingLogoDto> CreateAppSettingLogo(AppSettingLogoCreateDto model);
        ResponseModel<AppSettingLogoDto> DeleteAppSettingLogo(int id);

    }
}
