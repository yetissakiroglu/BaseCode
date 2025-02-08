using Economy.Application.Dtos;
using Economy.Application.Interfaces;
using MediatR;

namespace Economy.Application.Queries.AppMenus
{
    public class GetAppMenuQueryHandler : IRequestHandler<GetAppMenuQuery, AppMenuDto>
    {
        private readonly IAppMenuService _appMenuService;

        public GetAppMenuQueryHandler(IAppMenuService appMenuService)
        {
            _appMenuService = appMenuService ?? throw new ArgumentNullException(nameof(appMenuService));
        }

        public async Task<AppMenuDto> Handle(GetAppMenuQuery request, CancellationToken cancellationToken)
        {
            var filters = new GetAppMenuQuery(request.Id);
            var appMenu = await _appMenuService.GetForReadAsync(filters);

            return new AppMenuDto
            {
                Id = appMenu.Data.Id,
                Title = appMenu.Data.Title,
                Slug = appMenu.Data.Slug,
                IsExternal = appMenu.Data.IsExternal,
                ParentMenuId = appMenu.Data.ParentMenuId,
                SubMenus = appMenu.Data.SubMenus.Select(sm => new AppMenuDto
                {
                    Id = sm.Id,
                    Title = sm.Title,
                    Slug = sm.Slug
                }).ToList()
            };
        }
    }
}
