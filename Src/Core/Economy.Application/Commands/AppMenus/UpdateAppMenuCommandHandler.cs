using Economy.Application.Dtos.AppMenuDtos;
using Economy.Application.Interfaces;
using Economy.Core.Tools;
using MediatR;

namespace Economy.Application.Commands.AppMenus
{
    public class UpdateAppMenuCommandHandler(IAppMenuService appMenuService) : IRequestHandler<UpdateAppMenuCommand, ResponseModel<AppMenuDto>>
    {
        private readonly IAppMenuService _appMenuService = appMenuService;
        public async Task<ResponseModel<AppMenuDto>> Handle(UpdateAppMenuCommand request, CancellationToken cancellationToken)
        {
            var response = await _appMenuService.UpdateAsync(request);
            return response;
        }
    }
}
