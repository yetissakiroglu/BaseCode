using Economy.Application.Dtos.LoginLogPageQueryDto;
using Economy.Core.Tools.Result;

namespace Economy.Application.Interfaces
{
    public interface IPanelLoginLogService
    {
        Task<ServiceResult<LoginLogPageViewModel>> GetPageAsync(LoginLogPageQuery q);
        Task<ServiceResult<byte[]>> ExportCsvAsync(LoginLogPageQuery q, int maxRows = 5000);
    }
}
