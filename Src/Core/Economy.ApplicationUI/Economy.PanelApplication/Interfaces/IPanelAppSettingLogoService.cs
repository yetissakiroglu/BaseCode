using Economy.Core.Tools;
using Economy.Panel.Application.Dtos.AppSettingLogoDtos;

namespace Economy.Panel.Application.Interfaces
{
    public interface IPanelAppSettingLogoService
    {
        ResponseModel<AppSettingLogoDto> GetAppSettingLogo(bool isDeleted);
        ResponseModel<AppSettingLogoDto> CreateEditAppSettingLogo(AppSettingLogoCreateEditDto model);
        ResponseModel<AppSettingLogoDto> DeleteAppSettingLogo(int id);

    }
}
