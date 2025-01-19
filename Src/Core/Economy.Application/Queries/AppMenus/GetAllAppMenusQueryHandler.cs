using Economy.Application.Dtos;
using Economy.Application.Interfaces;
using Economy.Application.Repositories.AppMenuRepositories;
using MediatR;

namespace Economy.Application.Queries.AppMenus
{
    public class GetAllAppMenusQueryHandler : IRequestHandler<GetAllAppMenusQuery, List<AppMenuDto>>
    {
        private readonly IAppMenuService _appMenuService;

        public GetAllAppMenusQueryHandler(IAppMenuService appMenuService)
        {
            _appMenuService = appMenuService;
        }

        public async Task<List<AppMenuDto>> Handle(GetAllAppMenusQuery request, CancellationToken cancellationToken)
        {
            var menus = await _appMenuService.WhereForReadAsync(request);
            return menus.Select(menu => new AppMenuDto
            {
                Id = menu.Id,
                Title = menu.Title,
                Slug = menu.Slug,
                IsExternal = menu.IsExternal,
                ParentMenuId = menu.ParentMenuId
            }).ToList();
        }
    }
}
