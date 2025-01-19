using Economy.Application.Repositories.AppMenuRepositories;
using Economy.Domain.Entites.EntityMenuItems;
using MediatR;

namespace Economy.Application.Commands.AppMenus
{
    public class CreateAppMenuCommandHandler : IRequestHandler<CreateAppMenuCommand, int>
    {
        private readonly IAppMenuRepository _appMenuRepository;

        public CreateAppMenuCommandHandler(IAppMenuRepository appMenuRepository)
        {
            _appMenuRepository = appMenuRepository;
        }

        public async Task<int> Handle(CreateAppMenuCommand request, CancellationToken cancellationToken)
        {
            // Yeni bir AppMenu oluşturuyoruz
            var appMenu = new AppMenu
            {
                Title = request.Title,
                Slug = request.Slug,
                IsExternal = request.IsExternal,
                ParentMenuId = request.ParentMenuId
            };

            // Menü veritabanına ekleniyor
            await _appMenuRepository.AddAsync(appMenu);

            // Yeni eklenen menünün ID'si döndürülüyor
            return appMenu.Id;
        }
    }
}
