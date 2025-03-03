using Economy.Application.Dtos.AppMenuDtos;
using Economy.Application.Interfaces;
using Economy.Core.Tools;
using MediatR;

namespace Economy.Application.Queries.AppMenus
{
    public class GetAppMenuByMenuIdQueryHandler(IAppMenuService appMenuService) : IRequestHandler<GetAppMenuByMenuIdQuery, ResponseModel<AppMenuDto>>
    {
        private readonly IAppMenuService _appMenuService = appMenuService;
        public async Task<ResponseModel<AppMenuDto>> Handle(GetAppMenuByMenuIdQuery request, CancellationToken cancellationToken)
        {
            return await _appMenuService.GetForReadAsync(request);
        }
    }
}
