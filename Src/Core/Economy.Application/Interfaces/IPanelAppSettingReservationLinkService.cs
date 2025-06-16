using Economy.Core.Tools;
using Economy.Panel.Application.Dtos.AppSettingReservationLinkDtos;

namespace Economy.Panel.Application.Interfaces
{
    public interface IPanelAppSettingReservationLinkService
    {
        ResponseModel<IEnumerable<AppSettingReservationLinkDto>> GetAllReservationLink(bool isDeleted);
        ResponseModel<AppSettingReservationLinkDto> GetReservationLink(int id, bool isDeleted);
        ResponseModel<AppSettingReservationLinkDto> EditReservationLink(AppSettingReservationLinkEditDto model);
        ResponseModel<AppSettingReservationLinkDto> CreateReservationLink(AppSettingReservationLinkCreateDto model);
        ResponseModel<AppSettingReservationLinkDto> DeleteReservationLink(int id);
    }
}
