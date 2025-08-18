using Economy.Application.Dtos;
using Economy.Application.Dtos.AppAuditLogDtos;
using Economy.Application.Dtos.AppErrorLogDtos;
using Economy.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Economy.Panel.UI.Controllers
{
    [Authorize]
    [Route("Logs")]
    public class LogsController : Controller
    {
        private readonly IPanelAuditLogService _audit;
        private readonly IPanelErrorLogService _errorService;

        public LogsController(IPanelAuditLogService audit, IPanelErrorLogService errorService)
        {
            _audit = audit;
            _errorService = errorService;
        }

        // GET /Logs/Audit
        [HttpGet("Audit")]
        public async Task<IActionResult> Audit([FromQuery] AuditLogQueryDto q)
        {
            // varsayılan tarih aralığı: son 7 gün
            if (q.DateFrom == null && q.DateTo == null)
            {
                q.DateFrom = DateTime.UtcNow.Date.AddDays(-7);
                q.DateTo = DateTime.UtcNow.Date;
            }

            var res = await _audit.ListAsync(q);
            if (!res.IsSuccess)
            {
                TempData["Error"] = res.Message;
                return View(new AuditPageViewModel { Query = q, Result = new PagedResult<AuditLogDto>() });
            }

            return View(new AuditPageViewModel { Query = q, Result = res.Data! });
        }

        // GET /Logs/Audit/Detail/123
        [HttpGet("Audit/Detail/{id:long}")]
        public async Task<IActionResult> AuditDetail(long id)
        {
            var res = await _audit.GetAsync(id);
            if (!res.IsSuccess) return NotFound(res.Message);

            return PartialView("_AuditDetailPartial", res.Data); // veya return Json(res.Data);
        }

        #region Error log
        // GET: /Logs/Error
        [HttpGet("Error")]
        public async Task<IActionResult> Error([FromQuery] ErrorLogQueryDto q)
        {
            if (q.DateFrom == null && q.DateTo == null)
            {
                q.DateFrom = DateTime.UtcNow.Date.AddDays(-7);
                q.DateTo = DateTime.UtcNow.Date;
            }

            var res = await _errorService.ListAsync(q);
            if (!res.IsSuccess)
            {
                TempData["Error"] = res.Message;
                return View(new ErrorPageViewModel { Query = q, Result = new PagedResult<ErrorLogDto>() });
            }

            return View(new ErrorPageViewModel { Query = q, Result = res.Data! });
        }

        // GET: /Logs/Error/Detail/123
        [HttpGet("Error/Detail/{id:long}")]
        public async Task<IActionResult> ErrorDetail(long id)
        {
            var res = await _errorService.GetAsync(id);
            if (!res.IsSuccess) return NotFound(res.Message);
            return PartialView("_ErrorDetailPartial", res.Data);
        }
        #endregion

    }

    public class AuditPageViewModel
    {
        public AuditLogQueryDto Query { get; set; } = new();
        public PagedResult<AuditLogDto> Result { get; set; } = new();
    }
    public class ErrorPageViewModel
    {
        public ErrorLogQueryDto Query { get; set; } = new();
        public PagedResult<ErrorLogDto> Result { get; set; } = new();
    }
}
