using MediatR;

namespace Economy.Application.Commands.AppMenus
{
    public record DeleteAppMenuCommand(int Id) : IRequest<bool>;

}
