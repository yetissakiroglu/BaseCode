using Economy.Core.Tools;
using Economy.Panel.Application.Dtos.AppSettingWhatsappLineDtos;

namespace Economy.Panel.Application.Interfaces
{
    public interface IPanelAppSettingWhatsappLineService
    {
        ResponseModel<IEnumerable<AppSettingWhatsappLineDto>> GetAllAppSettingWhatsappLine(bool isDeleted);
        ResponseModel<AppSettingWhatsappLineDto> GetAppSettingWhatsappLine(int id, bool isDeleted);
        ResponseModel<AppSettingWhatsappLineDto> EditAppSettingWhatsappLine(AppSettingWhatsappLineEditDto model);
        ResponseModel<AppSettingWhatsappLineDto> CreateAppSettingWhatsappLine(AppSettingWhatsappLineCreateDto model);
        ResponseModel<AppSettingWhatsappLineDto> DeleteAppSettingWhatsappLine(int id);
    }
}
