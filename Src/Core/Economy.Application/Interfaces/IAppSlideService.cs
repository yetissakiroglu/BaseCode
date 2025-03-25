using Economy.Application.Dtos.AppSlideDtos;
using Economy.Application.Queries.AppSlides;
using Economy.Core.Tools;

namespace Economy.Application.Interfaces
{
    public interface IAppSlideService
    {
        Task<ResponseModel<List<AppSlideDto>>> WhereForReadByLanguageCodeAsync(GetAllAppSlideByLanguageCodeQuery query);
        Task<ResponseModel<List<AppSlideDto>>> WhereForReadByLanguageCodeBySectionIdAsync(GetAllAppSlideByLanguageCodeBySectionIdQuery query);


    }
}
