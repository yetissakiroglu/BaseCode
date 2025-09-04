using Economy.Application.Dtos;
using Economy.Application.Dtos.AppErrorLogDtos;
using Economy.Core.Tools.Result;
using Economy.Domain.Entites.AppEntities;

namespace Economy.Application.Interfaces
{
    public interface IPanelErrorLogService
    {
        Task<ServiceResult<PagedResult<ErrorLogDto>>> ListAsync(ErrorLogQueryDto q);
        Task<ServiceResult<ErrorLogDto>> GetAsync(long id);

        Task<ServiceResult<AppErrorLog>> Create(AppErrorLog appErrorLog);


        // İsteğe bağlı:
        Task<ServiceResult<int>> ClearAsync(DateTime? olderThanUtc = null);
        Task<ServiceResult<ErrorLogDto>> DeleteAsync(long id);
    }
}
