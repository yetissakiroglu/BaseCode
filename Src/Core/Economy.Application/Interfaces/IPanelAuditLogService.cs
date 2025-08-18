using Economy.Application.Dtos;
using Economy.Application.Dtos.AppAuditLogDtos;
using Economy.Core.Tools.Result;

namespace Economy.Application.Interfaces
{
    public interface IPanelAuditLogService
    {
        Task<ServiceResult<PagedResult<AuditLogDto>>> ListAsync(AuditLogQueryDto q);
        Task<ServiceResult<AuditLogDto>> GetAsync(long id);
    }
}
