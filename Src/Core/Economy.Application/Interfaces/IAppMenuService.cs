using Economy.Application.Commands.AppMenus;
using Economy.Application.Dtos;
using Economy.Application.Queries.AppMenus;
using Economy.Persistence.Services;
using System.Linq.Expressions;

namespace Economy.Application.Interfaces
{
    public interface IAppMenuService
    {
        Task<ServiceResult<AppMenuDto>> GetForReadAsync(GetAppMenuQuery query);
        Task<List<AppMenuDto>> WhereForReadAsync(GetAllAppMenusQuery query);
        Task<ServiceResult<bool>> DeleteAsync(int id);
        Task<ServiceResult<bool>> DeleteAsync(AppMenuDto model);
        Task<ServiceResult<AppMenuDto>> InsertAsync(CreateAppMenuCommand command);
        Task<ServiceResult<AppMenuDto>> UpdateAsync(UpdateAppMenuCommand command);
    }

}
