using Economy.Application.Commands.AppMenus;
using Economy.Application.Dtos.AppMenuDtos;
using Economy.Application.Queries.AppMenus;
using Economy.Core.Tools;

namespace Economy.Application.Interfaces
{
    public interface IAppMenuService
    {
        ResponseModel<AppMenuDto> GetForRead(GetAppMenuByMenuIdQuery query);
        ResponseModel<List<AppMenuDto>> WhereForRead(GetAllAppMenuQuery query);
        ResponseModel<List<AppMenuDto>> WhereForRead(GetAllAppMenuByParentMenuIdQuery query);
        ResponseModel<bool> Delete(DeleteAppMenuCommand command);
        ResponseModel<int> Insert(CreateAppMenuCommand command);
        ResponseModel<AppMenuDto> Update(UpdateAppMenuCommand command);
    }
}
