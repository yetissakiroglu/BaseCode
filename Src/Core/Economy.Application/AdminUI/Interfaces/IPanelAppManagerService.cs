using Economy.Core.Tools;
using Economy.Core.Tools.Result;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Economy.Application.AdminUI.Interfaces
{
    public interface IPanelAppManagerService
    {
        Task<List<int>> GetManagerIdsByAppIdAsync(int appId);
        Task<ServiceResult<NoContent>> UpdateManagersForAppAsync(int appId, List<int> userIds);
    }
}
