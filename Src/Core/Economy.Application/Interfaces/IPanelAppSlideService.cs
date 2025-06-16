using Economy.Core.Tools.Result;
using Economy.Panel.Application.Dtos.AppSlideDtos;

namespace Economy.Panel.Application.Interfaces
{
    public interface IPanelAppSlideService
    {
        ServiceResult<List<AppSlideDto>> GetAllSlide(bool isDeleted);
        ServiceResult<AppSlideDto> GetSlide(int id, bool isDeleted);
        ServiceResult<AppSlideDto> EditSlide(AppSlideCreateEditDto model);
        ServiceResult<AppSlideDto> CreateSlide(AppSlideCreateEditDto model);
        ServiceResult<AppSlideDto> DeleteSlide(int id);
    }
}
