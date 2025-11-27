using Economy.Application.TenantUI.Dtos;
using Economy.Application.TenantUI.Interfaces;
using Economy.Panel.UI.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Economy.Panel.UI.Areas.Tenant.Controllers
{
    [Area("Tenant")]
    [Authorize]
    public class RoomAttributeGroupController : BaseController
    {
        private readonly IPanelAppPageService _panelAppPageService;
        public RoomAttributeGroupController(IPanelAppPageService panelAppPageService)
        {
            _panelAppPageService = panelAppPageService;
        }

        public async Task<IActionResult> Index(CancellationToken ct)
        {
            var pageModel = await _panelAppPageService.GetRoomAttributeGroupListAsync(ct);
            return View(pageModel.Data);
        }

        [HttpGet]
        public async Task<IActionResult> Create(CancellationToken ct)
        {
            var vm = new RoomAttributeGroupEditVm
            {
                SortOrder = 0,
            };
            await _panelAppPageService.RoomAttributeGroupFillLanguagesAsync(vm, ct);

            return View("Edit", vm);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id, CancellationToken ct)
        {
            var result = await _panelAppPageService.GetRoomAttributeGroupAsync(id, ct);

            return View(result.Data);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(RoomAttributeGroupEditVm model, CancellationToken ct)
        {
            if (!ModelState.IsValid)
                return View(model);

            var resultEdit = await _panelAppPageService.CreateEditRoomAttributeGroup(model, ct);
            AddMessage(resultEdit);
            return RedirectToAction(nameof(Index));
        }
    }
}
