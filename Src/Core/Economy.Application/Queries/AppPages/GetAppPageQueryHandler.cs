using Economy.Application.Dtos.AppMenuDtos;
using Economy.Application.Interfaces;
using Economy.Application.Queries.AppMenus;
using MediatR;

namespace Economy.Application.Queries.AppPages
{
    public class GetAppPageQueryHandler : IRequestHandler<GetAppMenuQuery, AppMenuDto>
    {
        private readonly IAppMenuService _appMenuService;

        public GetAppPageQueryHandler(IAppMenuService appMenuService)
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
                Url = appMenu.Data.Url,
                IsExternal = appMenu.Data.IsExternal,
                ParentMenuId = appMenu.Data.ParentMenuId,
                SubMenus = appMenu.Data.SubMenus.Select(sm => new AppMenuDto
                {
                    Id = sm.Id,
                    Title = sm.Title,
                    Url = sm.Url
                }).ToList()
            };
        }
    }
}
