using Economy.Application.TenantUI.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Economy.Panel.UI.Areas.Tenant.Controllers
{
    // /Areas/Tenant/Controllers/PageBlocksController.cs
    [Area("Tenant")]
    [Route("tenant/pageblocks")]
    public class PageBlocksController : Controller
    {
        private readonly IPanelPageBlockGroupService _svc;
        public PageBlocksController(IPanelPageBlockGroupService svc) => _svc = svc;

        [HttpGet("list")]
        public async Task<IActionResult> List(int pageId)
            => Json(await _svc.ListForPageAsync(pageId));

        [HttpGet("candidates")]
        public async Task<IActionResult> Candidates(int pageId, string? q)
            => Json(await _svc.ListCandidatesAsync(pageId, q));

        [HttpPost("attach")]
        public async Task<IActionResult> Attach(int pageId, int blockGroupId)
        {
            await _svc.AttachAsync(pageId, blockGroupId);
            return Ok();
        }

        [HttpPost("detach")]
        public async Task<IActionResult> Detach(int pageId, int blockGroupId)
        {
            await _svc.DetachAsync(pageId, blockGroupId);
            return Ok();
        }

        public class SortVm { public int BlockGroupId { get; set; } public int SortOrder { get; set; } }

        [HttpPost("sort")]
        public async Task<IActionResult> Sort(int pageId, [FromBody] List<SortVm> rows)
        {
            await _svc.SortAsync(pageId, rows.Select(x => (x.BlockGroupId, x.SortOrder)).ToList());
            return Ok();
        }
    }

}
