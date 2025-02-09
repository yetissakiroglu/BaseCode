using Economy.Application.Dtos.AppMenuDtos;
using Economy.Application.Interfaces;
using Economy.Application.Repositories.AppMenuRepositories;
using MediatR;

namespace Economy.Application.Queries.AppMenus
{
    public class GetAllAppMenuQueryHandler : IRequestHandler<GetAllAppMenuQuery, List<AppMenuDto>>
    {
        private readonly IAppMenuService _appMenuService;

        public GetAllAppMenuQueryHandler(IAppMenuService appMenuService)
        {
            _appMenuService = appMenuService;
        }

        public async Task<List<AppMenuDto>> Handle(GetAllAppMenuQuery request, CancellationToken cancellationToken)
        {
            var menus = await _appMenuService.WhereForReadAsync(request);
            return menus.Data.Select(menu => new AppMenuDto
            {
                Id = menu.Id,
                Title = menu.Title,
                Slug = menu.Slug,
                IsExternal = menu.IsExternal,
                ParentMenuId = menu.ParentMenuId,
                SubMenus = menu.SubMenus
                
            }).ToList();
        }
    }
}
