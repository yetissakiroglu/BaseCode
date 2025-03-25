using Economy.Application.Dtos.AppContentDtos;
using Economy.Application.Dtos.AppPageDtos;
using Economy.Application.Interfaces;
using Economy.Core.Tools;
using MediatR;

namespace Economy.Application.Queries.AppContents
{
    public class GetAppContentByLanguageCodeByAppContentIdQueryHandler(IAppContentService appContentService) : IRequestHandler<GetAppContentByLanguageCodeByAppContentIdQuery, ResponseModel<AppContentDto>>
    {
        private readonly IAppContentService _appContentService = appContentService;
        public async Task<ResponseModel<AppContentDto>> Handle(GetAppContentByLanguageCodeByAppContentIdQuery request, CancellationToken cancellationToken)
        {
            return await _appContentService.GetForReadByLanguageCodeByAppContentIdAsync(request);
        }
    }

}
