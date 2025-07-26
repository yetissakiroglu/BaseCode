using Economy.Core.Tools.Result;
using Economy.Panel.Application.Dtos.AppDtos;

namespace Economy.Application.Interfaces
{
    public interface IConnectionTesterService
    {
        Task<ServiceResult<bool>> TestConnectionAsync(int appId);
    }
}
