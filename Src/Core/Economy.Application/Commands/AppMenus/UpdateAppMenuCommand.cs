using Economy.Application.Dtos.AppMenuDtos;
using Economy.Core.Tools;
using MediatR;

namespace Economy.Application.Commands.AppMenus
{
    public record UpdateAppMenuCommand(int Id,string Title,string Url,bool IsExternal,int? ParentMenuId, int AppLanguageId) : IRequest<ResponseModel<AppMenuDto>>;
}
