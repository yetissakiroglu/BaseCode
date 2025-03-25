using Economy.Application.Dtos.AppContentDtos;
using Economy.Application.Dtos.AppPageDtos;
using Economy.Core.Tools;
using MediatR;

namespace Economy.Application.Queries.AppContents
{
    public record GetAppContentByLanguageCodeByAppContentIdQuery(int AppContentId,string LanguageCode) : IRequest<ResponseModel<AppContentDto>>;

   
}
