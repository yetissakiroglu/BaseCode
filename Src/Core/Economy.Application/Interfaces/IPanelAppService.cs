using Economy.Application.Dtos.AppDtos;
using Economy.Core.Tools.Result;
using Economy.Panel.Application.Dtos.AppDtos;

namespace Economy.Panel.Application.Interfaces
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
