using Economy.Application.Commands.AppMenus;
using Economy.Application.Dtos.AppMenuDtos;
using Economy.Application.Queries.AppMenus;
using Economy.Core.Tools;
using LoggingLibrary.Attributes;

namespace Economy.Application.Interfaces
{
    public interface IAppMenuService
    {
        Task<ResponseModel<AppMenuDto>> GetForReadAsync(GetAppMenuByMenuIdQuery query);
        Task<ResponseModel<List<AppMenuDto>>> WhereForReadAsync(GetAllAppMenuQuery query);
        [Log("Menü WhereForReadAsync interface alındı.")]
        Task<ResponseModel<List<AppMenuDto>>> WhereForReadAsync(GetAllAppMenuByParentMenuIdQuery query);
        Task<ResponseModel<bool>> DeleteAsync(DeleteAppMenuCommand command);
        Task<ResponseModel<int>> InsertAsync(CreateAppMenuCommand command);
        Task<ResponseModel<AppMenuDto>> UpdateAsync(UpdateAppMenuCommand command);
    }

}
