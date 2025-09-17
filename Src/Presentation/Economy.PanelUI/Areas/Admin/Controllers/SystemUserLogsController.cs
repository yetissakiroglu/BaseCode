using Economy.Application.Dtos.LoginLogPageQueryDto;
using Economy.Application.Interfaces;
using Economy.Panel.UI.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Economy.Panel.UI.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class SystemUserLogsController : BaseController
    {
        private readonly IPanelLoginLogService _svc;

        public SystemUserLogsController(IPanelLoginLogService svc)
        {
            _svc = svc;
        }

        [HttpGet]
        public async Task<IActionResult> LoginLogs([FromQuery] LoginLogPageQuery q)
        {
            var res = await _svc.GetPageAsync(q);
            if (!res.HasData)
            {
                AddMessage(res);
                return View(new Economy.Application.Dtos.LoginLogPageQueryDto.LoginLogPageViewModel { Q = q ?? new LoginLogPageQuery() });
            }
            return View(res.Data);
        }
        
    }
}
