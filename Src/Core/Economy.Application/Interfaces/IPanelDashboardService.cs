using Economy.Application.Dtos.DashboardSummaryDtos;
using Economy.Core.Tools.Result;

namespace Economy.Application.Interfaces
{
    public interface IPanelDashboardService
    {
        Task<ServiceResult<DashboardSummaryDto>> GetSummaryAsync();
    }
}
