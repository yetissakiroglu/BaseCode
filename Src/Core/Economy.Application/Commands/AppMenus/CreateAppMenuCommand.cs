using Economy.Core.Tools;
using MediatR;

namespace Economy.Application.Commands.AppMenus
{
    public record CreateAppMenuCommand(string Title,string Slug,bool IsExternal,int? ParentMenuId) : IRequest<ResponseModel<int>>;
}
