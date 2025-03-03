using Economy.Core.Tools;
using MediatR;

namespace Economy.Application.Commands.AppMenus
{
    public record DeleteAppMenuCommand(int MenuId) : IRequest<ResponseModel<bool>>;
}
