using Economy.Core.Tools.Result;

namespace Economy.Application.AdminUI.Interfaces
{
    public interface IConnectionTesterService
    {
        Task<ServiceResult<bool>> TestConnectionAsync(int appId);
    }
}
