using Economy.Application.Dtos;
using Economy.Application.Dtos.AppErrorLogDtos;
using Economy.Core.Tools.Result;

namespace Economy.Application.Interfaces
{
    public interface IPanelErrorLogService
    {
        Task<ServiceResult<PagedResult<ErrorLogDto>>> ListAsync(ErrorLogQueryDto q);
        Task<ServiceResult<ErrorLogDto>> GetAsync(long id);

        // İsteğe bağlı:
        Task<ServiceResult<int>> ClearAsync(DateTime? olderThanUtc = null);
        Task<ServiceResult<ErrorLogDto>> DeleteAsync(long id);
    }
}
