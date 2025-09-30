using Economy.Application.TenantUI.Dtos.AppSettingLogoDtos;
using Economy.Core.Tools.Result;

namespace Economy.Application.TenantUI.Interfaces
{
    public interface IPanelAppSettingLogoService
    {
        ServiceResult<AppSettingLogoDto> GetAppSettingLogo(bool isDeleted);
        ServiceResult<AppSettingLogoDto> CreateEditAppSettingLogo(AppSettingLogoCreateEditDto model);
    }
}
