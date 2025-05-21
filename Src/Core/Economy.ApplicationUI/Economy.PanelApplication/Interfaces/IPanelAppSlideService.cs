using Economy.Core.Tools;
using Economy.Panel.Application.Dtos.AppSlideDtos;

namespace Economy.Panel.Application.Interfaces
{
    public interface IPanelAppSlideService
    {
        ResponseModel<IEnumerable<AppSlideDto>> GetAllSlide(bool isDeleted);
        ResponseModel<AppSlideDto> GetSlide(int id, bool isDeleted);
        ResponseModel<AppSlideDto> EditSlide(AppSlideEditDto model);
        ResponseModel<AppSlideDto> CreateSlide(AppSlideCreateEditDto model);
        ResponseModel<AppSlideDto> DeleteSlide(int id);
    }
}
