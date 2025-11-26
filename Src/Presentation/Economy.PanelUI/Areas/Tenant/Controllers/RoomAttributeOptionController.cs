using Economy.Application.TenantUI.Dtos;
using Economy.Application.TenantUI.Interfaces;
using Economy.Panel.UI.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Economy.Panel.UI.Areas.Tenant.Controllers
{
    [Area("Tenant")]
    [Authorize]
    public class RoomAttributeOptionController : BaseController
    {
        private readonly IPanelAppPageService _panelAppPageService;
        public RoomAttributeOptionController(IPanelAppPageService panelAppPageService)
        {
            _panelAppPageService = panelAppPageService;
        }
        // Belirli bir attribute’a ait seçenek listesi
        public async Task<IActionResult> Index(int attributeId, CancellationToken ct)
        {
            var attr = _panelAppPageService.GetRoomAttributeAsync(attributeId, ct).Result.Data;
            if (attr == null) return NotFound();

            var list = await _panelAppPageService.GetRoomAttributeOptionListAsync(attributeId, ct);

            foreach (var item in list.Data)
            {
                item.AttributeCode = attr.Code;
            }


            ViewBag.AttributeName = attr.Code; // veya çeviri ile isim
            ViewBag.AttributeId = attributeId;

            return View(list.Data);
        }

        [HttpGet]
        public async Task<IActionResult> CreateAsync(int attributeId, CancellationToken ct)
        {
            var attr = _panelAppPageService.GetRoomAttributeAsync(attributeId, ct).Result.Data;
            if (attr == null) return NotFound();

            var vm = new RoomAttributeOptionEditVm
            {
                DefRoomAttributeId = attributeId,
                AttributeCode = attr.Code,
                SortOrder = 0
            };

            await _panelAppPageService.RoomAttributeOptionFillLanguagesAsync(vm, ct);

            return View("Edit", vm);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id, CancellationToken ct)
        {
            var result = await _panelAppPageService.GetRoomAttributeOptionAsync(id, ct);

            return View(result.Data);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(RoomAttributeOptionEditVm model, CancellationToken ct)
        {
            if (!ModelState.IsValid)
                return View(model);

            var resultEdit = await _panelAppPageService.CreateEditRoomAttributeOptionAsync(model, ct);
            AddMessage(resultEdit);
            return RedirectToAction(nameof(Index), new { attributeId = resultEdit.Data.Id });
        }

    }
}
