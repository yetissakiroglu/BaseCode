using Economy.Application.Dtos.SlideDtos;
using Economy.Application.Services.BaseServices;
using Economy.Domain.Entites.EntitySlides;
using Economy.Persistence.Services;

namespace Economy.Application.Services
{
    public interface IAppSlideServices : IService<AppSlide,int>
    {
        Task<ServiceResult<List<AppSlideDto>>> AppSlidesAsync();
        Task<ServiceResult<List<AppSlideDto>>> AppSlidesAsync(int appSectionId);
       
    }
}
