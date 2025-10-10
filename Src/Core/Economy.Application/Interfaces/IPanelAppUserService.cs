using Economy.Base.Application.Dtos.BaseModels;
using Economy.Core.Dtos;
using Economy.Core.Tools;
using Economy.Core.Tools.Result;

namespace Economy.Panel.Application.Interfaces
{
    public interface IPanelAppUserService
    {
        Task<ServiceResult<AppUserDto>> GetUser(int id, bool isDeleted);
        Task<ServiceResult<List<AppUserDto>>> GetAllManagers();
    }
}
