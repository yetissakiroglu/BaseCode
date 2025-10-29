using Economy.Application.TenantUI.Dtos;
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

        public async Task<IActionResult> Index(BlockType? type, string? q, int page = 1, int size = 20)
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
            var langs = _panelAppLanguageService.GetAllLanguage(false, true);
            ViewBag.Languages = langs.Data;
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






            [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            return View();

        }

        [HttpPost("edit/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, BlockGroupBlockVm vm)
        {

            return View();
            //var b = await _apppageblock.DataSet.Include(x => x.Translations).FirstOrDefaultAsync(x => x.Id == id);
            //if (b == null) return NotFound();

            //b.Type = vm.Type; b.SortOrder = vm.SortOrder; b.IsActive = vm.IsActive; b.Stage = vm.Stage;
            //b.SharedJson = vm.SharedJson;

            //var langs = await _langRepo.DataSet.Where(x => x.IsActive).ToListAsync();
            //foreach (var l in langs)
            //{
            //    var incoming = vm.Translations.First(t => t.LanguageId == l.Id);
            //    var cur = b.Translations.FirstOrDefault(t => t.AppLanguageId == l.Id);
            //    if (cur == null) b.Translations.Add(new AppBlockTranslation { AppLanguageId = l.Id, LocalizedJson = incoming.LocalizedJson });
            //    else cur.LocalizedJson = incoming.LocalizedJson;
            //}
            //await _uow.SaveHotelChangesAsync();
            //TempData["ok"] = "Blok kaydedildi";
            //return RedirectToAction(nameof(Edit), new { id });
        }

    }


}
