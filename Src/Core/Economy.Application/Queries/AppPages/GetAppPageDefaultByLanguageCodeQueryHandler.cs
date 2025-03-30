using Economy.Application.Dtos.AppPageDtos;
using Economy.Application.Interfaces;
using Economy.Core.Tools;
using MediatR;

namespace Economy.Application.Queries.AppPages
{
    public class GetAppPageDefaultByLanguageCodeQueryHandler(IAppPageService appPageService) : IRequestHandler<GetAppPageDefaultByLanguageCodeQuery, ResponseModel<AppPageDto>>
    {
        private readonly IAppPageService _appPageService = appPageService;
        public async Task<ResponseModel<AppPageDto>> Handle(GetAppPageDefaultByLanguageCodeQuery request, CancellationToken cancellationToken)
        {
            return _appPageService.GetForReadPageDefaultByLanguageCode(request);
        }
    }

}
