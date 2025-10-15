using Economy.Application.TenantUI.Dtos;
using Economy.Application.TenantUI.Interfaces;
using Economy.Panel.UI.Controllers;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Economy.Panel.UI.Areas.Tenant.Controllers
{
    [Area("Tenant")]
    [Authorize]
    public class BlockGroupsController : BaseController
    {
        private readonly IBlockGroupService _svc;
        private readonly IValidator<BlockGroupDto> _groupVal;
        private readonly IValidator<BlockItemDto> _itemVal;
        public BlockGroupsController(IBlockGroupService svc, IValidator<BlockGroupDto> groupVal, IValidator<BlockItemDto> itemVal)
        {
            _svc = svc; _groupVal = groupVal; _itemVal = itemVal;
        }
        public async Task<IActionResult> Index(CancellationToken ct)
        {
            var data = await _svc.GetGroupsListAsync(ct);
            return View(data.Data);
        }
        [HttpGet]
        public async Task<IActionResult> Create(CancellationToken ct)
        {
            var vm = new BlockGroupDto();
            await _svc.FillLanguagesAsync(vm, ct);
            return View(vm);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(BlockGroupDto model, CancellationToken ct)
        {
            ValidationResult vr = await _groupVal.ValidateAsync(model, ct);
            if (!vr.IsValid)
            {
                foreach (var e in vr.Errors) ModelState.AddModelError(e.PropertyName, e.ErrorMessage);
                return View(model);
            }
            var result = await _svc.CreateGroupAsync(model, ct);
            return RedirectToAction(nameof(Edit), new { id = result.Data.Id });
        }
        public async Task<IActionResult> Edit(int id, CancellationToken ct)
        {
            var dto = await _svc.GetGroupAsync(id, ct);
            if (dto == null) return NotFound();
            return View(dto.Data);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, BlockGroupDto model, CancellationToken ct)
        {
            ValidationResult vr = await _groupVal.ValidateAsync(model, ct);
            if (!vr.IsValid)
            {
                foreach (var e in vr.Errors) ModelState.AddModelError(e.PropertyName, e.ErrorMessage);
                return View(model);
            }
           var updateResult =  await _svc.UpdateGroupAsync(id, model, ct);
            AddMessage(updateResult);

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