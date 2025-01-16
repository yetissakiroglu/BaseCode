using Economy.Application.Dtos;
using Economy.Application.Repositories.AppMenuRepositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Economy.Application.Queries.AppMenus
{
    public class GetAppMenuQueryHandler : IRequestHandler<GetAppMenuQuery, AppMenuDto>
    {
        private readonly IAppMenuRepository _repository;

        public GetAppMenuQueryHandler(IAppMenuRepository repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public async Task<AppMenuDto> Handle(GetAppMenuQuery request, CancellationToken cancellationToken)
        {
            var appMenu = await _repository.GetByIdAsync(request.Id, cancellationToken);
            if (appMenu == null) throw new AppMenuNotFoundException(request.Id);

            return new AppMenuDto
            {
                Id = appMenu.Id,
                Title = appMenu.Title,
                Slug = appMenu.Slug,
                IsExternal = appMenu.IsExternal,
                ParentMenuId = appMenu.ParentMenuId,
                SubMenus = appMenu.SubMenus.Select(sm => new AppMenuDto
                {
                    Id = sm.Id,
                    Title = sm.Title,
                    Slug = sm.Slug
                }).ToList()
            };
        }
    }
}
