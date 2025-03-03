using Economy.Application.Interfaces;
using Economy.Core.Tools;
using MediatR;

namespace Economy.Application.Commands.AppMenus
{
    public class DeleteAppMenuCommandHandler(IAppMenuService appMenuService) : IRequestHandler<DeleteAppMenuCommand, ResponseModel<bool>>
    {
        private readonly IAppMenuService _appMenuService = appMenuService;
        public async Task<ResponseModel<bool>> Handle(DeleteAppMenuCommand request, CancellationToken cancellationToken)
        {
            var appMenuDelete = await _appMenuService.DeleteAsync(request);
            return appMenuDelete;
        }
    }
}
