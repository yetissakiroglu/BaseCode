using Economy.Application.Dtos.AppSlideDtos;
using Economy.Application.Queries.AppSlides;
using Economy.Core.Tools;
namespace Economy.Application.Interfaces
{
    public interface IAppSlideService
    {
        ResponseModel<List<AppSlideDto>> WhereForReadByLanguageCode(GetAllAppSlideByLanguageCodeQuery query);
        ResponseModel<List<AppSlideDto>> WhereForReadByLanguageCodeBySectionId(GetAllAppSlideByLanguageCodeBySectionIdQuery query);
    }
}
