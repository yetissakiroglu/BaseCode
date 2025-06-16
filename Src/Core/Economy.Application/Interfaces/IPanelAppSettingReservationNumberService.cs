using Economy.Core.Tools;
using Economy.Panel.Application.Dtos.AppSettingReservationNumberDtos;

namespace Economy.Panel.Application.Interfaces
{
    public interface IPanelAppSettingReservationNumberService
    {
        ResponseModel<IEnumerable<AppSettingReservationNumberDto>> GetAllReservationNumber(bool isDeleted);
        ResponseModel<AppSettingReservationNumberDto> GetReservationNumber(int id, bool isDeleted);
        ResponseModel<AppSettingReservationNumberDto> EditReservationNumber(AppSettingReservationNumberEditDto model);
        ResponseModel<AppSettingReservationNumberDto> CreateReservationNumber(AppSettingReservationNumberCreateDto model);
        ResponseModel<AppSettingReservationNumberDto> DeleteReservationNumber(int id);
    }
}
