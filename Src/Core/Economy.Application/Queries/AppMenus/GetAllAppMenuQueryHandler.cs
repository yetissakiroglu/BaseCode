using Economy.Application.Dtos.AppMenuDtos;
using Economy.Application.Interfaces;
using Economy.Core.Tools;
using MediatR;

namespace Economy.Application.Queries.AppMenus
{
    public class GetAllAppMenuQueryHandler(IAppMenuService appMenuService) : IRequestHandler<GetAllAppMenuQuery, ResponseModel<List<AppMenuDto>>>
    {
        private readonly IAppMenuService _appMenuService = appMenuService;

        public async Task<ResponseModel<List<AppMenuDto>>> Handle(GetAllAppMenuQuery request, CancellationToken cancellationToken)
        {
            return await _appMenuService.WhereForReadAsync(request);
        }
    }
}
