using Economy.Application.TenantUI.Dtos.AppBlockGroupDtos;
using Economy.Application.TenantUI.Interfaces;
using Economy.Panel.UI.Areas.Tenant.Models;
using Economy.Panel.UI.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Economy.Panel.UI.Areas.Tenant.Controllers
{
    [Area("Tenant")]
    [Authorize]
    public class BlockGroupsController : BaseController
    {
        private readonly IPanelAppBlockGroupService _panelAppBlockGroupService;
        private readonly IPanelAppBlockService _panelAppBlockService;

        public BlockGroupsController(IPanelAppBlockGroupService panelAppBlockGroupService, IPanelAppBlockService panelAppBlockService)
        {
            _panelAppBlockGroupService = panelAppBlockGroupService;
            _panelAppBlockService = panelAppBlockService;
        }

        [HttpGet]
        public async Task<IActionResult> Index(CancellationToken ct)
        {
            var data = await _panelAppBlockGroupService.GetAllBlockGroupsListAsync(ct);
            return View(data.Data);
        }

        [HttpGet]
        public async Task<IActionResult> Create(CancellationToken ct)
        {
            var vm = new AppBlockGroupDto();
            await _panelAppBlockGroupService.FillLanguagesAsync(vm, ct);
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AppBlockGroupDto vm, CancellationToken ct)
        {
            var result = await _panelAppBlockGroupService.CreateBlockGroupAsync(vm, ct);
            AddMessage(result);
            return RedirectToAction(nameof(Edit), new { id = result.Data.Id });
        }

        public async Task<IActionResult> Edit(int id, CancellationToken ct)
        {
            var d = await _panelAppBlockGroupService.GetBlockGroupAsync(id, ct);
            if (d == null) return NotFound();
            return View(d.Data);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, AppBlockGroupDto vm, CancellationToken ct)
        {
            var result = await _panelAppBlockGroupService.UpdateBlockGroupAsync(id,vm, ct);
            AddMessage(result);
            return RedirectToAction(nameof(Edit), new { id });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id, CancellationToken ct)
        {
            var result = await _panelAppBlockGroupService.DeleteBlockGroupAsync(id, ct);
            AddMessage(result);
            return RedirectToAction(nameof(Index));
        }

        [HttpGet("Tenant/BlockGroups/BlockEdit/{id:int}")]
        public async Task<IActionResult> BlockEdit(int id, CancellationToken ct)
        {
            var group = await _panelAppBlockGroupService.GetBlockGroupAsync(id, ct);
            if (group is null) return NotFound();

            // Dil ID’n varsa geçir; yoksa null
            var allBlocks = await _panelAppBlockService.GetAllMiniBlocksAsync(ct);
            var layout = await _panelAppBlockGroupService.GetGroupLayoutAsync(id, ct);

            var vm = new BlockGroupEditVm
            {
                GroupId = id,
                GroupTitle = group.Data.Translations.Select(t => t.Title).FirstOrDefault() ?? $"Group #{id}",
                AllBlocks = allBlocks.Data,
                Selected = layout.Data.OrderBy(x => x.SortOrder).ToList()
            };
            return View(vm);
        }

        [HttpPost("Tenant/BlockGroups/SaveLayout")]
        public async Task<IActionResult> SaveLayout([FromBody] SaveGroupLayoutRequest model, CancellationToken ct)
        {
            if (model is null || model.GroupId <= 0)
                return BadRequest("Geçersiz veri");

            // SortOrder normalize
            for (int i = 0; i < model.Items.Count; i++)
                model.Items[i] = model.Items[i] with { SortOrder = i + 1 };

            var res = await _panelAppBlockGroupService.SaveGroupLayoutAsync(model, ct);
            return Ok(res);
        }


    }
}