using Economy.Application.TenantUI.Dtos;
using Economy.Application.TenantUI.Interfaces;
using Economy.Panel.UI.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using static Economy.Panel.UI.Areas.Tenant.Controllers.SelectController;

namespace Economy.Panel.UI.Areas.Tenant.Controllers
{
    [Area("Tenant")]
    [Authorize]
    public class RoomAttributeController : BaseController
    {
        private readonly IPanelAppPageService _panelAppPageService;
        public RoomAttributeController(IPanelAppPageService panelAppPageService)
        {
            _panelAppPageService = panelAppPageService;
        }

        public async Task<IActionResult> Index(CancellationToken ct)
        {
            var pageModel = await _panelAppPageService.GetRoomAttributeListAsync(ct);
            return View(pageModel.Data);
        }

        [HttpGet]
        public async Task<IActionResult> Create(CancellationToken ct)
        {
            var vm = new RoomAttributeEditVm
            {
                SortOrder = 0,
                InputType = "Option"
            };
            await _panelAppPageService.RoomAttributeFillLanguagesAsync(vm, ct);
            return View("Edit", vm);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id, CancellationToken ct)
        {
            var result = await _panelAppPageService.GetRoomAttributeAsync(id, ct);
            
            return View(result.Data);
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(RoomAttributeEditVm model, CancellationToken ct)
        {
            if (!ModelState.IsValid)
                return View(model);

            var resultEdit = await _panelAppPageService.CreateEditRoomAttribute(model, ct);
            AddMessage(resultEdit);
            return RedirectToAction(nameof(Index));
        }

    }
}
