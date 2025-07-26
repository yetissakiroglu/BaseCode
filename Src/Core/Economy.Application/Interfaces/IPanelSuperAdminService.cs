using Economy.Application.Dtos.AppSuperAdminUserDtos;
using Economy.Application.Dtos.AppUserDtos;
using Economy.Base.Application.Dtos.BaseModels;
using Economy.Core.Tools;
using Economy.Core.Tools.Result;

namespace Economy.Application.Interfaces
{
    public interface IPanelSuperAdminService
    {
        Task<ServiceResult<List<AppRoleDto>>> GetRolesAsync();
        Task<ServiceResult<List<AppSuperAdminUserDto>>> GetUserListAsync();
        Task<ServiceResult<AppSuperAdminUserDto>> GetUserAsync(int userId);
        Task<ServiceResult<AppSuperAdminUserDto>> CreateUserAsync(AppSuperAdminUserCreateDto userDto);
        Task<ServiceResult<AppSuperAdminUserDto>> UpdateUserAsync(AppSuperAdminUserEditDto userDto);
        Task<ServiceResult<AppSuperAdminUserDto>> DeleteUserAsync(int Id);
    }
}
