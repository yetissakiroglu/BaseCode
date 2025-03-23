using Economy.Application.Dtos.AppSlideDtos;
using Economy.Core.Tools;
using MediatR;

namespace Economy.Application.Queries.AppSlides
{
    public record GetAllAppSlideByLanguageCodeQuery(string LanguageCode) : IRequest<ResponseModel<List<AppSlideDto>>>;

   
}
