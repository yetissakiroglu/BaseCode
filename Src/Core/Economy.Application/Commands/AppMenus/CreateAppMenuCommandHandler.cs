using Economy.Application.Repositories.AppMenuRepositories;
using Economy.Domain.Entites.EntityMenuItems;
using MediatR;

namespace Economy.Application.Commands.AppMenus
{
    public class CreateAppMenuCommandHandler : IRequestHandler<CreateAppMenuCommand, int>
    {
        private readonly IAppMenuRepository _repository;

        public CreateAppMenuCommandHandler(IAppMenuRepository repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public async Task<int> Handle(CreateAppMenuCommand request)
        {
            var appMenu = new AppMenu
            {
                Title = request.Title,
                Slug = request.Slug,
                IsExternal = request.IsExternal,
                ParentMenuId = request.ParentMenuId
            };

            await _repository.AddAsync(appMenu);
            return appMenu.Id;
        }
    }
}
