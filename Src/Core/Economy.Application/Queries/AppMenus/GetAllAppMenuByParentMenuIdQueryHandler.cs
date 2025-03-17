using Economy.Application.Dtos.AppMenuDtos;
using Economy.Application.Interfaces;
using Economy.Core.Tools;
using LoggingLibrary.Attributes;
using MediatR;

namespace Economy.Application.Queries.AppMenus
{
    public class GetAllAppMenuByParentMenuIdQueryHandler(IAppMenuService appMenuService) : IRequestHandler<GetAllAppMenuByParentMenuIdQuery, ResponseModel<List<AppMenuDto>>>
    {
        private readonly IAppMenuService _appMenuService = appMenuService;
        public async Task<ResponseModel<List<AppMenuDto>>> Handle(GetAllAppMenuByParentMenuIdQuery request, CancellationToken cancellationToken)
        {
            var filters = new GetAllAppMenuByParentMenuIdQuery(request.ParentMenuId, request.LanguageCode);
            var appMenu = await _appMenuService.WhereForReadAsync(filters);
            return appMenu;
        }
    }
}
