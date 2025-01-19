using Economy.Application.Dtos.SlideDtos;
using Economy.Persistence.Services;

namespace Economy.Application.Services
{
    public interface IAppSlideServices 
    {
        Task<ServiceResult<List<AppSlideDto>>> AppSlidesAsync();
        Task<ServiceResult<List<AppSlideDto>>> AppSlidesAsync(int appSectionId);
       
    }
}
