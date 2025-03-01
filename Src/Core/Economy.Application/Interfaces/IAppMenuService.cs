using Economy.Application.Commands.AppMenus;
using Economy.Application.Dtos.AppMenuDtos;
using Economy.Application.Queries.AppMenus;
using Economy.Core.Tools;

namespace Economy.Application.Interfaces
{
    public interface IAppMenuService
    {
        Task<ResponseModel<AppMenuDto>> GetForReadAsync(GetAppMenuQuery query);
        Task<ResponseModel<List<AppMenuDto>>> WhereForReadAsync(GetAllAppMenuQuery query);
        Task<ResponseModel<bool>> DeleteAsync(int id);
        Task<ResponseModel<bool>> DeleteAsync(AppMenuDto model);
        Task<ResponseModel<int>> InsertAsync(CreateAppMenuCommand command);
        Task<ResponseModel<AppMenuDto>> UpdateAsync(UpdateAppMenuCommand command);
    }

}
