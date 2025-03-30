using Economy.Application.Dtos.AppContentDtos;
using Economy.Application.Interfaces;
using Economy.Core.Tools;
using MediatR;

namespace Economy.Application.Queries.AppContents
{
    public class GetAllAppContentByLanguageCodeByAppContentIdsQueryHandler(IAppContentService appContentService) : IRequestHandler<GetAllAppContentByLanguageCodeByAppContentIdsQuery, ResponseModel<List<AppContentDto>>>
    {
        private readonly IAppContentService _appContentService = appContentService;
        public async Task<ResponseModel<List<AppContentDto>>> Handle(GetAllAppContentByLanguageCodeByAppContentIdsQuery request, CancellationToken cancellationToken)
        {
            return _appContentService.WhereForReadByLanguageCodeByAppContentIds(request);
        }
    }

}
