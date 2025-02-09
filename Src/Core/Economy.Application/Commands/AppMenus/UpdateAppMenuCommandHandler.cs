using Economy.Application.Dtos.AppMenuDtos;
using Economy.Application.Interfaces;
using Economy.Persistence.Services;
using MediatR;

namespace Economy.Application.Commands.AppMenus
{
    public class UpdateAppMenuCommandHandler(IAppMenuService appMenuService) : IRequestHandler<UpdateAppMenuCommand, ServiceResult<AppMenuDto>>
    {
        private readonly IAppMenuService _appMenuService = appMenuService;

        public async Task<ServiceResult<AppMenuDto>> Handle(UpdateAppMenuCommand request, CancellationToken cancellationToken)
        {
            var response = await _appMenuService.UpdateAsync(request);

            return response;
        }
    }
}
