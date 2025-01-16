using Economy.Application.Exceptions;
using Economy.Application.Repositories.AppMenuRepositories;
using MediatR;

namespace Economy.Application.Commands.AppMenus
{
    public class UpdateAppMenuCommandHandler : IRequestHandler<UpdateAppMenuCommand, bool>
    {
        private readonly IAppMenuRepository _appMenuRepository;

        public UpdateAppMenuCommandHandler(IAppMenuRepository appMenuRepository)
        {
            _appMenuRepository = appMenuRepository;
        }

        public async Task<bool> Handle(UpdateAppMenuCommand request, CancellationToken cancellationToken)
        {
            // Existing AppMenu fetched from database
            var existingMenu = await _appMenuRepository.GetByIdAsync(request.Id);

            if (existingMenu == null)
            {
                throw new AppMenuNotFoundException(request.Id); // Özel istisna sınıfı kullanıldı
            }

            // Update properties
            existingMenu.Title = request.Title;
            existingMenu.Slug = request.Slug;
            existingMenu.IsExternal = request.IsExternal;
            existingMenu.ParentMenuId = request.ParentMenuId;

            // Save changes
            await _appMenuRepository.UpdateAsync(existingMenu);

            return true;
        }
    }
}
