using Economy.Application.Dtos.AppPageDtos;
using Economy.Application.Interfaces;
using Economy.Core.Tools;
using MediatR;

namespace Economy.Application.Queries.AppPages
{
    public class GetAppPageDefaultQueryHandler(IAppPageService appPageService) : IRequestHandler<GetAppPageDefaultQuery, ResponseModel<AppPageDto>>
    {
        private readonly IAppPageService _appPageService = appPageService;
        public async Task<ResponseModel<AppPageDto>> Handle(GetAppPageDefaultQuery request, CancellationToken cancellationToken)
        {
            return _appPageService.GetForReadDefaultPage(request);
        }
    }

}
