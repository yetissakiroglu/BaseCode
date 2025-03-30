using Economy.Application.Dtos.AppPageDtos;
using Economy.Application.Interfaces;
using Economy.Core.Tools;
using MediatR;

namespace Economy.Application.Queries.AppPages
{
    public class GetAppPageByLanguageCodeByUrlQueryHandler(IAppPageService appPageService) : IRequestHandler<GetAppPageByLanguageCodeByUrlQuery, ResponseModel<AppPageDto>>
    {
        private readonly IAppPageService _appPageService = appPageService;
        public async Task<ResponseModel<AppPageDto>> Handle(GetAppPageByLanguageCodeByUrlQuery request, CancellationToken cancellationToken)
        {
            return _appPageService.GetForReadPageByLanguageCodeByUrl(request);
        }
    }

}
