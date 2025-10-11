using Economy.Application.AdminUI.Dtos.AppGeneralSettingDtos;
using Economy.Core.Tools.Result;

namespace Economy.Application.AdminUI.Interfaces
{
    public interface IPanelAppGeneralSettingService
    {
        Task<ServiceResult<AppGeneralSettingDto>> GetGeneralSettingAsync();


        Task<ServiceResult<List<AppGeneralSettingDto>>> GetGeneralSettingListAsync();
  
        Task<ServiceResult<AppGeneralSettingDto>> GetGeneralSettingAsync(int Id);
        Task<ServiceResult<AppGeneralSettingDto>> CreateGeneralSettingAsync(AppGeneralSettingCreateDto modelDto);
        Task<ServiceResult<AppGeneralSettingDto>> UpdateGeneralSettingAsync(AppGeneralSettingEditDto modelDto);
        Task<ServiceResult<AppGeneralSettingDto>> DeleteGeneralSettingAsync(int Id);
    }
}
