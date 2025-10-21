using Economy.Application.TenantUI.Dtos;
using Economy.Application.TenantUI.Interfaces;
using Economy.Core.Enums;
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
        private readonly IPanelAppLanguageService _panelAppLanguageService;
        public BlockGroupsController(IBlockGroupService svc, IValidator<BlockGroupDto> groupVal, IPanelAppLanguageService panelAppLanguageService)
        {
            _svc = svc; _groupVal = groupVal;
            _panelAppLanguageService = panelAppLanguageService;
        }
        public async Task<IActionResult> Index(CancellationToken ct)
        {
            var data = await _svc.GetGroupsListAsync(ct);
            return View(data.Data);
        }
        [HttpGet]
        public async Task<IActionResult> Create(CancellationToken ct)
        {
            var langs = _panelAppLanguageService.GetAllLanguage(false);
            ViewBag.Languages = langs.Data;

            var vm = new BlockGroupDto();
            await _svc.FillLanguagesAsync(vm, ct);
            return View(vm);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(BlockGroupDto model, CancellationToken ct)
        {
            var updateResult2 = await _svc.CreateUpdateGroupAndItemsAsync(model, ct);
            AddMessage(updateResult2);

            //ValidationResult vr = await _groupVal.ValidateAsync(model, ct);
            //if (!vr.IsValid)
            //{
            //    foreach (var e in vr.Errors) ModelState.AddModelError(e.PropertyName, e.ErrorMessage);
            //    return View(model);
            //}
            //var result = await _svc.CreateGroupAsync(model, ct);
            return RedirectToAction(nameof(Edit), new { id = updateResult2.Data.Id });
        }
        public async Task<IActionResult> Edit(int id, CancellationToken ct)
        {
            var langs = _panelAppLanguageService.GetAllLanguage(false,true);
            ViewBag.Languages = langs.Data;

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

            var updateResult2 = await _svc.CreateUpdateGroupAndItemsAsync(model, ct);
            AddMessage(updateResult2);

            //var updateResult = await _svc.UpdateGroupAsync(id, model, ct);
            //AddMessage(updateResult);

            return RedirectToAction(nameof(Edit), new { id });
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id, CancellationToken ct)
        {
            var result = await _svc.DeleteGroupAsync(id, ct);
            AddMessage(result);
            return RedirectToAction(nameof(Index));
        }


        // Content-grid: yeni kart üretir
        [HttpGet("block-card-partial")]
        public IActionResult BlockCardPartial(BlockType type, int index, List<int> languageIds)
        {
            var langs = _panelAppLanguageService.GetAllLanguage(false,true);
            ViewBag.Languages = langs.Data;

            var vm = new BlockGroupBlockVm
            {
                Type = type,
                SortOrder = index,
                IsActive = true,
                Stage = ContentStage.Draft,
                SharedJson = type switch
                {
                    BlockType.Hero => """{"backgroundUrl":"/media/hero.jpg","verticalAlign":"center"}""",
                    BlockType.ImageGallery => """{"mode":"grid"}""",
                    BlockType.FeatureGrid => """{"columns":4}""",
                    _ => "{}"
                },
                Translations = languageIds.Select(lid => new BlockGroupBlockTranslationVm
                {
                    LanguageId = lid,
                    LocalizedJson = type switch
                    {
                        BlockType.Hero => """{"heading":"Başlık","subHeading":"Alt başlık","buttonText":"Devam","buttonUrl":"/"}""",
                        BlockType.Text => """{"heading":"Bölüm","bodyHtml":"<p>Metin…</p>"}""",
                        BlockType.FeatureGrid => """{"items":[{"icon":"wifi","title":"Ücretsiz Wi-Fi","description":"Tesis genelinde"}]}""",
                        _ => "{}"
                    }
                }).ToList()
            };

            // ÖNEMLİ: Koleksiyon prefix’i
            ViewData.TemplateInfo.HtmlFieldPrefix = $"Blocks[{index}]";

            return PartialView("_BlockCard", vm);
        }






        // --- Items ---
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> AddItem(int groupId, BlockItemDto item)
        //{
        //    var vr = await _itemVal.ValidateAsync(item);
        //    if (!vr.IsValid)
        //    {
        //        foreach (var e in vr.Errors) ModelState.AddModelError(e.PropertyName, e.ErrorMessage);
        //        return RedirectToAction(nameof(Edit), new { id = groupId });
        //    }
        //    await _svc.AddItemAsync(groupId, item);
        //    return RedirectToAction(nameof(Edit), new { id = groupId });
        //}


        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> UpdateItem(int groupId, int itemId, BlockItemDto item)
        //{
        //    var vr = await _itemVal.ValidateAsync(item);
        //    if (!vr.IsValid)
        //    {
        //        foreach (var e in vr.Errors) ModelState.AddModelError(e.PropertyName, e.ErrorMessage);
        //        return RedirectToAction(nameof(Edit), new { id = groupId });
        //    }
        //    await _svc.UpdateItemAsync(itemId, item);
        //    return RedirectToAction(nameof(Edit), new { id = groupId });
        //}


        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> DeleteItem(int groupId, int itemId)
        //{
        //    await _svc.DeleteItemAsync(itemId);
        //    return RedirectToAction(nameof(Edit), new { id = groupId });
        //}
    }
}