using Economy.Core.Tools;
using MediatR;

namespace Economy.Application.Commands.AppMenus
{
    public record CreateAppMenuCommand(string Title,string Url,bool IsExternal,int? ParentMenuId,int AppLanguageId) : IRequest<ResponseModel<int>>;
}
