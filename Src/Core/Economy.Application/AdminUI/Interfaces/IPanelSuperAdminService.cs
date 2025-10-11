using Economy.Application.AdminUI.Dtos.AppSuperAdminUserDtos;
using Economy.Core.Tools;
using Economy.Core.Tools.Result;

namespace Economy.Application.AdminUI.Interfaces
{
    public interface IPanelSuperAdminService
    {
        Task<ServiceResult<List<AppRoleDto>>> GetRolesAsync();
        Task<ServiceResult<List<AppSuperAdminUserDto>>> GetUserListAsync();
        Task<ServiceResult<NoContent>> ChangePasswordAsync(AppSuperAdminChangePasswordDto model);
        Task<ServiceResult<AppSuperAdminUserDto>> GetUserAsync(int userId);
        Task<ServiceResult<AppSuperAdminUserDto>> CreateUserAsync(AppSuperAdminUserCreateDto userDto);
        Task<ServiceResult<AppSuperAdminUserDto>> UpdateUserAsync(AppSuperAdminUserEditDto userDto);
        Task<ServiceResult<AppSuperAdminUserDto>> DeleteUserAsync(int Id);
    }
}
