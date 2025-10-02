using Economy.Application.TenantUI.Dtos.AppSlideDtos;
using Economy.Core.Tools.Result;

namespace Economy.Application.TenantUI.Interfaces
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
