using Economy.Application.Dtos.AppContentDtos;
using Economy.Core.Tools;
using MediatR;

namespace Economy.Application.Queries.AppContents
{
    public record GetAllAppContentByLanguageCodeByAppContentIdsQuery(List<int> AppContentIds, string LanguageCode) : IRequest<ResponseModel<List<AppContentDto>>>;

   
}
