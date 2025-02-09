using Economy.Application.Commands.AppMenus;
using Economy.Application.Dtos.AppMenuDtos;
using Economy.Application.Queries.AppMenus;
using Economy.Persistence.Services;

namespace Economy.Application.Interfaces
{
    public interface IAppMenuService
    {
        Task<ServiceResult<AppMenuDto>> GetForReadAsync(GetAppMenuQuery query);
        Task<ServiceResult<List<AppMenuDto>>> WhereForReadAsync(GetAllAppMenuQuery query);
        Task<ServiceResult<bool>> DeleteAsync(int id);
        Task<ServiceResult<bool>> DeleteAsync(AppMenuDto model);
        Task<ServiceResult<int>> InsertAsync(CreateAppMenuCommand command);
        Task<ServiceResult<AppMenuDto>> UpdateAsync(UpdateAppMenuCommand command);
    }

}
