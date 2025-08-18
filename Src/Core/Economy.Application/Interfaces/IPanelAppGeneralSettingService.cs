using Economy.Application.Dtos.AppGeneralSettingDtos;
using Economy.Core.Tools.Result;

namespace Economy.Application.Interfaces
{
    public interface IPanelAppGeneralSettingService
    {
        Task<ServiceResult<List<AppGeneralSettingDto>>> GetGeneralSettingListAsync();
        Task<ServiceResult<AppGeneralSettingDto>> GetGeneralSettingAsync(int Id);
        Task<ServiceResult<AppGeneralSettingDto>> CreateGeneralSettingAsync(AppGeneralSettingCreateDto modelDto);
        Task<ServiceResult<AppGeneralSettingDto>> UpdateGeneralSettingAsync(AppGeneralSettingEditDto modelDto);
        Task<ServiceResult<AppGeneralSettingDto>> DeleteGeneralSettingAsync(int Id);
    }
}
