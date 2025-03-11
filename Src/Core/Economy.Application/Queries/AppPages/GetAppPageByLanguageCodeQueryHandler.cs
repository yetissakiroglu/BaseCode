using Economy.Application.Dtos.AppPageDtos;
using Economy.Application.Interfaces;
using Economy.Core.Tools;
using MediatR;

namespace Economy.Application.Queries.AppPages
{
    public class GetAppPageByLanguageCodeQueryHandler(IAppPageService appPageService) : IRequestHandler<GetAppPageByLanguageCodeQuery, ResponseModel<AppPageDto>>
    {
        private readonly IAppPageService _appPageService = appPageService;
        public async Task<ResponseModel<AppPageDto>> Handle(GetAppPageByLanguageCodeQuery request, CancellationToken cancellationToken)
        {
            return await _appPageService.GetForReadDefaultPageByLanguageCodeAsync(request);
        }
    }

}
