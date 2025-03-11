using Economy.Application.Dtos.AppPageDtos;
using Economy.Application.Interfaces;
using Economy.Core.Tools;
using MediatR;

namespace Economy.Application.Queries.AppPages
{
    public class GetAppPageQueryHandler(IAppPageService appPageService) : IRequestHandler<GetAppPageQuery, ResponseModel<AppPageDto>>
    {
        private readonly IAppPageService _appPageService = appPageService;
        public async Task<ResponseModel<AppPageDto>> Handle(GetAppPageQuery request, CancellationToken cancellationToken)
        {
            return await _appPageService.GetForReadDefaultPageAsync(request);
        }
    }

}
