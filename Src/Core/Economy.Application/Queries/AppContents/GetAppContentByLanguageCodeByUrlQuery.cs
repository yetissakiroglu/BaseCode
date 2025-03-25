using Economy.Application.Dtos.AppContentDtos;
using Economy.Core.Tools;
using MediatR;

namespace Economy.Application.Queries.AppContents
{
    public record GetAppContentByLanguageCodeByUrlQuery(string Url,string LanguageCode) : IRequest<ResponseModel<AppContentDto>>;

   
}
