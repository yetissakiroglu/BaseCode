using Economy.Application.Interfaces;
using Economy.Application.Queries.AppMenus;
using MediatR;

namespace Economy.Application.Commands.AppMenus
{
    public class DeleteAppMenuCommandHandler(IAppMenuService appMenuService) : IRequestHandler<DeleteAppMenuCommand, bool>
    {
        private readonly IAppMenuService _appMenuService = appMenuService;

        public async Task<bool> Handle(DeleteAppMenuCommand request, CancellationToken cancellationToken)
        {
            var filters = new GetAppMenuQuery(request.Id);


            var appMenu = await _appMenuService.GetForReadAsync(filters);

            if(!appMenu.IsSuccess)
            {
               return false; // Menü bulunamadı
            }

            if (appMenu.Data == null)
            {
                return false; // Menü bulunamadı
            }

            await _appMenuService.DeleteAsync(appMenu.Data);

            return true;
        }
    }
}
