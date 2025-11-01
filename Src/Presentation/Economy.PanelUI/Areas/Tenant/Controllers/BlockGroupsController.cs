using Economy.Application.TenantUI.Dtos.AppBlockGroupDtos;
using Economy.Application.TenantUI.Interfaces;
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
        public BlockGroupsController(IPanelAppBlockGroupService panelAppBlockGroupService)
        {
            _panelAppBlockGroupService = panelAppBlockGroupService;
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


        //[HttpGet]
        //public async Task<IActionResult> Create(CancellationToken ct)
        //{
        //    var langs = _panelAppLanguageService.GetAllLanguage(false,true);
        //    ViewBag.Languages = langs.Data;

        //    var vm = new BlockGroupDto();
        //    await _svc.FillLanguagesAsync(vm, ct);
        //    return View(vm);
        //}
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Create(BlockGroupDto model, CancellationToken ct)
        //{
        //    var updateResult2 = await _svc.CreateUpdateGroupAndItemsAsync(model, ct);
        //    AddMessage(updateResult2);

        //    //ValidationResult vr = await _groupVal.ValidateAsync(model, ct);
        //    //if (!vr.IsValid)
        //    //{
        //    //    foreach (var e in vr.Errors) ModelState.AddModelError(e.PropertyName, e.ErrorMessage);
        //    //    return View(model);
        //    //}
        //    //var result = await _svc.CreateGroupAsync(model, ct);
        //    return RedirectToAction(nameof(Edit), new { id = updateResult2.Data.Id });
        //}
        //public async Task<IActionResult> Edit(int id, CancellationToken ct)
        //{
        //    var langs = _panelAppLanguageService.GetAllLanguage(false,true);
        //    ViewBag.Languages = langs.Data;

        //    var dto = await _svc.GetGroupAsync(id, ct);
        //    if (dto == null) return NotFound();
        //    return View(dto.Data);
        //}
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Edit(int id, BlockGroupDto model, CancellationToken ct)
        //{


        //    ValidationResult vr = await _groupVal.ValidateAsync(model, ct);
        //    if (!vr.IsValid)
        //    {
        //        foreach (var e in vr.Errors) ModelState.AddModelError(e.PropertyName, e.ErrorMessage);
        //        return View(model);
        //    }

        //    var updateResult2 = await _svc.CreateUpdateGroupAndItemsAsync(model, ct);
        //    AddMessage(updateResult2);

        //    //var updateResult = await _svc.UpdateGroupAsync(id, model, ct);
        //    //AddMessage(updateResult);

        //    return RedirectToAction(nameof(Edit), new { id });
        //}
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Delete(int id, CancellationToken ct)
        //{
        //    var result = await _svc.DeleteGroupAsync(id, ct);
        //    AddMessage(result);
        //    return RedirectToAction(nameof(Index));
        //}


        //// Content-grid: yeni kart üretir
        //[HttpGet("block-card-partial")]
        //public IActionResult BlockCardPartial(BlockType type, int index, List<int> languageIds)
        //{
        //    var langs = _panelAppLanguageService.GetAllLanguage(false, true);
        //    ViewBag.Languages = langs.Data;

        //    var vm = new BlockGroupBlockVm
        //    {
        //        Type = type,
        //        //SortOrder = index,
        //        IsActive = true,
        //        //Stage = ContentStage.Draft,

        //        SharedJson = type switch
        //        {
        //            // Hero → Shared: backgroundUrl, verticalAlign
        //            BlockType.Hero => """{"backgroundUrl":"/media/hero.jpg","verticalAlign":"center"}""",

        //            // Text → Shared: {}
        //            BlockType.Text => """{}""",

        //            // ImageGallery → Shared: mode, imageUrls ([])
        //            BlockType.ImageGallery => """{"mode":"grid","imageUrls":[]}""",

        //            // AmenityGroup → Shared: {}
        //            BlockType.AmenityGroup => """{}""",

        //            _ => """{}"""
        //        },

        //        Translations = languageIds.Select(lid => new BlockGroupBlockTranslationVm
        //        {
        //            LanguageId = lid,
        //            LocalizedJson = type switch
        //            {
        //                // Hero → Localized: heading, subHeading, buttonText, buttonUrl
        //                BlockType.Hero =>
        //                    """{"heading":"Başlık","subHeading":"Alt başlık","buttonText":"Devam","buttonUrl":"/"}""",

        //                // Text → Localized: heading, bodyHtml
        //                BlockType.Text =>
        //                    """{"heading":"Bölüm","bodyHtml":"<p>Metin…</p>"}""",

        //                // ImageGallery → Localized: {}
        //                BlockType.ImageGallery =>
        //                    """{}""",

        //                // AmenityGroup → Localized: groupTitle, amenities ([])
        //                BlockType.AmenityGroup =>
        //                    """{"groupTitle":"Oda Olanakları","amenities":["Ücretsiz Wi-Fi","Klima","TV"]}""",

        //                _ => """{}"""
        //            }
        //        }).ToList()
        //    };

        //    // Koleksiyon prefix’i
        //    ViewData.TemplateInfo.HtmlFieldPrefix = $"Blocks[{index}]";

        //    return PartialView("_BlockCard", vm);
        //}


        ///**/


        //[HttpGet("Tenant/BlockGroups/Edit1/{id:int}")]
        //public async Task<IActionResult> Edit1(int id, CancellationToken ct)
        //{
        //    var group = await _svc.GetGroupAsync(id,ct);
        //    if (group is null) return NotFound();

        //    // Dil ID’n varsa geçir; yoksa null
        //    var allBlocks = await _svc.GetAllBlocksAsync(languageId: null, ct);
        //    var layout = await _svc.GetGroupLayoutAsync(id, ct);

        //    var vm = new BlockGroupEditVm
        //    {
        //        GroupId = id,
        //        GroupTitle = group.Data.Translations.Select(t => t.Title).FirstOrDefault() ?? $"Group #{id}",
        //        AllBlocks = allBlocks.Data,
        //        Selected = layout.Data.OrderBy(x => x.SortOrder).ToList()
        //    };
        //    return View(vm);
        //}

        //[HttpPost("Tenant/BlockGroups/SaveLayout")]
        //public async Task<IActionResult> SaveLayout([FromBody] SaveGroupLayoutRequest model, CancellationToken ct)
        //{
        //    if (model is null || model.GroupId <= 0)
        //        return BadRequest("Geçersiz veri");

        //    // SortOrder normalize
        //    for (int i = 0; i < model.Items.Count; i++)
        //        model.Items[i] = model.Items[i] with { SortOrder = i + 1 };

        //    var res = await _svc.SaveGroupLayoutAsync(model, ct);
        //    return Ok(res);
        //}






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