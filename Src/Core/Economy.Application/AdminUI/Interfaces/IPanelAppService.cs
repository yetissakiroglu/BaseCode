using Economy.Application.AdminUI.Dtos.AppDtos;
using Economy.Core.Tools.Result;

namespace Economy.Application.AdminUI.Interfaces
{
    public interface IPanelAppService
    {
        ServiceResult<AppDto> GetApp(int Id, bool isDeleted);
        ServiceResult<IEnumerable<AppDto>> Apps(bool isDeleted);
        Task<ServiceResult<AppDto>> GetAppById(int id);
        Task<ServiceResult<AppDto>> CreateApp(AppCreateEditDto model);
        Task<ServiceResult<AppDto>> EditApp(AppCreateEditDto model);
        ServiceResult<AppDto> DeleteApp(int Id);
    }
}
