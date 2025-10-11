using Economy.Application.AdminUI.Dtos.DashboardSummaryDtos;
using Economy.Core.Tools.Result;

namespace Economy.Application.AdminUI.Interfaces
{
    public interface IPanelDashboardService
    {
        Task<ServiceResult<DashboardSummaryDto>> GetSummaryAsync();
    }
}
