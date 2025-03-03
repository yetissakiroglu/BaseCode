using Economy.Application.Dtos.AppMenuDtos;
using Economy.Core.Tools;
using MediatR;

namespace Economy.Application.Commands.AppMenus
{
    public record UpdateAppMenuCommand(int Id,string Title,string Slug,bool IsExternal,int? ParentMenuId) : IRequest<ResponseModel<AppMenuDto>>;
}
