using Economy.Application.TenantUI.Dtos.AppBlockDtos;
using Economy.Application.TenantUI.Interfaces;
using Economy.Core.Enums;
using Economy.Panel.UI.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Economy.Panel.UI.Areas.Tenant.Controllers
{
    [Area("Tenant")]
    [Authorize]
    public class BlocksController : BaseController
    {
        private readonly IPanelAppBlockService _panelAppBlockService;
        private readonly IPanelAppLanguageService _panelAppLanguageService;
        public BlocksController(IPanelAppBlockService panelAppBlockService, IPanelAppLanguageService panelAppLanguageService)
        {
            _panelAppBlockService = panelAppBlockService;
            _panelAppLanguageService = panelAppLanguageService;
        }

        public async Task<IActionResult> Index(BlockType? type, string? q, int page = 1, int size = 50)
        {
            var model = await _panelAppBlockService.GetAllBlocksAsync(type, page, size, q);
            return View(model.Data);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id, CancellationToken ct)
        {
            var result = await _panelAppBlockService.DeleteBlockAsync(id, ct);
            AddMessage(result);
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> EditBlock(int blockId, CancellationToken ct)
        {
            var b = await _panelAppBlockService.GetBlocksAsync(blockId, ct);
            return View(b.Data);
        }

        [HttpGet]
        public async Task<IActionResult> CreateBlock(BlockType blockType, CancellationToken ct)
        {
            var vm = new AppBlockDto();
            await _panelAppBlockService.FillLanguagesAsync(vm, blockType, ct);
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateBlock(AppBlockDto vm, CancellationToken ct)
        {
            var result = await _panelAppBlockService.CreateBlockAsync(vm, ct);
            AddMessage(result);
            return RedirectToAction(nameof(EditBlock), new { blockId = result.Data.Id });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditBlock(int blockId, AppBlockDto vm, CancellationToken ct)
        {
            var result = await _panelAppBlockService.EditBlockAsync(blockId,vm, ct);
            AddMessage(result);
            return RedirectToAction(nameof(EditBlock), new { blockId = result.Data.Id });
        }

    }


}
