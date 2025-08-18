using Economy.Application.Dtos.AppSecuritySettingDtos;
using Economy.Core.Tools.Result;

namespace Economy.Application.Interfaces
{
    public interface IPanelAppSecuritySettingService
    {
        Task<ServiceResult<AppSecuritySettingDto>> CreateAsync(AppSecuritySettingCreateDto model);
        Task<ServiceResult<AppSecuritySettingDto>> UpdateAsync(AppSecuritySettingEditDto model);
        Task<ServiceResult<AppSecuritySettingDto>> GetAsync(int id);
        Task<ServiceResult<List<AppSecuritySettingDto>>> ListAsync();
        Task<ServiceResult<AppSecuritySettingDto>> DeleteAsync(int id);
    }
}
