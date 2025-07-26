using Economy.Application.Dtos.AppSuperAdminUserDtos;
using Economy.Application.Dtos.AppUserDtos;
using Economy.Core.Tools.Result;

namespace Economy.Application.Interfaces
{
    public interface IPanelSuperAdminService
    {
        Task<ServiceResult<List<AppRoleDto>>> GetRolesAsync();



        Task<ServiceResult<List<AppSuperAdminUserDto>>> GetUserListAsync();
        Task<ServiceResult<AppSuperAdminUserDto>> GetUserAsync(int userId);
        Task<ServiceResult<AppSuperAdminUserDto>> CreateUserAsync(AppSuperAdminUserCreateEditDto userDto);
        Task<ServiceResult<AppSuperAdminUserDto>> UpdateUserAsync(AppSuperAdminUserCreateEditDto userDto);
    }
}
