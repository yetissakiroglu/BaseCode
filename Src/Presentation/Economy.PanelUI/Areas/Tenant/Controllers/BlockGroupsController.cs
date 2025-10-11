using Economy.Application.TenantUI.Dtos;
using Economy.Application.TenantUI.Interfaces;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Economy.Panel.UI.Areas.Tenant.Controllers
{
    [Area("Tenant")]
    [Authorize]
    public class BlockGroupsController : Controller
    {
        private readonly IBlockService _svc;
        private readonly IValidator<BlockGroupDto> _groupVal;
        private readonly IValidator<BlockItemDto> _itemVal;


        public BlockGroupsController(IBlockService svc, IValidator<BlockGroupDto> groupVal, IValidator<BlockItemDto> itemVal)
        {
            _svc = svc; _groupVal = groupVal; _itemVal = itemVal;
        }


        public async Task<IActionResult> Index()
        {
            var data = await _svc.GetGroupsAsync();
            return View(data);
        }

        [HttpGet]
        public IActionResult Create()
        {
         return View(new BlockGroupDto());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(BlockGroupDto model)
        {
            ValidationResult vr = await _groupVal.ValidateAsync(model);
            if (!vr.IsValid)
            {
                foreach (var e in vr.Errors) ModelState.AddModelError(e.PropertyName, e.ErrorMessage);
                return View(model);
            }
            var id = await _svc.CreateGroupAsync(model);
            return RedirectToAction(nameof(Edit), new { id });
        }


        public async Task<IActionResult> Edit(int id)
        {
            var dto = await _svc.GetGroupAsync(id);
            if (dto == null) return NotFound();
            return View(dto);
        }





        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, BlockGroupDto model)
        {
            ValidationResult vr = await _groupVal.ValidateAsync(model);
            if (!vr.IsValid)
            {
                foreach (var e in vr.Errors) ModelState.AddModelError(e.PropertyName, e.ErrorMessage);
                return View(model);
            }
            await _svc.UpdateGroupAsync(id, model);
            TempData["ok"] = "Güncellendi";
            return RedirectToAction(nameof(Edit), new { id });
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            await _svc.DeleteGroupAsync(id);
            return RedirectToAction(nameof(Index));
        }


        // --- Items ---
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddItem(int groupId, BlockItemDto item)
        {
            var vr = await _itemVal.ValidateAsync(item);
            if (!vr.IsValid)
            {
                foreach (var e in vr.Errors) ModelState.AddModelError(e.PropertyName, e.ErrorMessage);
                return RedirectToAction(nameof(Edit), new { id = groupId });
            }
            await _svc.AddItemAsync(groupId, item);
            return RedirectToAction(nameof(Edit), new { id = groupId });
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateItem(int groupId, int itemId, BlockItemDto item)
        {
            var vr = await _itemVal.ValidateAsync(item);
            if (!vr.IsValid)
            {
                foreach (var e in vr.Errors) ModelState.AddModelError(e.PropertyName, e.ErrorMessage);
                return RedirectToAction(nameof(Edit), new { id = groupId });
            }
            await _svc.UpdateItemAsync(itemId, item);
            return RedirectToAction(nameof(Edit), new { id = groupId });
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteItem(int groupId, int itemId)
        {
            await _svc.DeleteItemAsync(itemId);
            return RedirectToAction(nameof(Edit), new { id = groupId });
        }
    }
}