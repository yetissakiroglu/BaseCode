using Economy.Application.Interfaces;
using Economy.Core.Tools;
using MediatR;

namespace Economy.Application.Commands.AppMenus
{
    public class CreateAppMenuCommandHandler(IAppMenuService appMenuService) : IRequestHandler<CreateAppMenuCommand, ResponseModel<int>>
    {
        private readonly IAppMenuService _appMenuService = appMenuService;
        public async Task<ResponseModel<int>> Handle(CreateAppMenuCommand request, CancellationToken cancellationToken)
        {
            // Menü veritabanına ekleniyor
           return await _appMenuService.InsertAsync(request);
        }
    }
}
