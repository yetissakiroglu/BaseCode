using Economy.Application.Commands.AppMenus;
using Economy.Application.Dtos;
using Economy.Application.Services.BaseServices;
using Economy.Domain.Entites.EntityMenuItems;
using Economy.Persistence.Services;

namespace Economy.Application.Interfaces
{
    public interface IAppMenuService : IService<AppMenu, int>
    {
        Task<AppMenuDto> CreateAppMenuAsync(CreateAppMenuCommand command);
        Task<AppMenuDto> GetAppMenuByIdAsync(int id);
        Task<IEnumerable<AppMenuDto>> GetAllAppMenusAsync();
        Task<AppMenuDto> UpdateAppMenuAsync(UpdateAppMenuCommand command);
        Task DeleteAppMenuAsync(int id);

    }
   
}
