using Economy.Application.TenantUI.Dtos;
using Economy.Application.TenantUI.Interfaces;
using Economy.Panel.UI.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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


        // CREATE GET
        [HttpGet]
        public IActionResult Create()
        {
            var vm = new RoomAttributeEditVm
            {
                SortOrder = 0,
                InputType = "Option"
            };
            return View("Edit", vm);
        }

        //// EDIT GET
        //[HttpGet]
        //public IActionResult Edit(int id)
        //{
        //    var attr = _db.DefRoomAttributes
        //        .Where(x => x.Id == id)
        //        .Select(a => new
        //        {
        //            Attribute = a,
        //            Tr = a.Translations
        //                .Where(t => t.AppLanguage.Code == "tr")
        //                .FirstOrDefault(),
        //            En = a.Translations
        //                .Where(t => t.AppLanguage.Code == "en")
        //                .FirstOrDefault()
        //        })
        //        .FirstOrDefault();

        //    if (attr == null) return NotFound();

        //    var vm = new RoomAttributeEditVm
        //    {
        //        Id = attr.Attribute.Id,
        //        Code = attr.Attribute.Code,
        //        Group = attr.Attribute.Group,
        //        InputType = attr.Attribute.InputType,
        //        SortOrder = attr.Attribute.SortOrder,
        //        IsFilterable = attr.Attribute.IsFilterable,
        //        IsRequired = attr.Attribute.IsRequired,
        //        IsActive = attr.Attribute.IsActive,
        //        NameTr = attr.Tr?.Name ?? "",
        //        NameEn = attr.En?.Name ?? ""
        //    };

        //    return View(vm);
        //}

        //// CREATE / EDIT POST
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public IActionResult Edit(RoomAttributeEditVm model)
        //{
        //    if (!ModelState.IsValid)
        //        return View(model);

        //    DefRoomAttribute entity;

        //    if (model.Id == 0)
        //    {
        //        entity = new DefRoomAttribute
        //        {
        //            Code = model.Code.Trim(),
        //            Group = model.Group.Trim(),
        //            InputType = model.InputType.Trim(),
        //            SortOrder = model.SortOrder,
        //            IsFilterable = model.IsFilterable,
        //            IsRequired = model.IsRequired,
        //            IsActive = model.IsActive
        //        };

        //        _db.DefRoomAttributes.Add(entity);
        //        _db.SaveChanges(); // Id oluşsun
        //    }
        //    else
        //    {
        //        entity = _db.DefRoomAttributes
        //            .FirstOrDefault(x => x.Id == model.Id)!;

        //        if (entity == null) return NotFound();

        //        entity.Code = model.Code.Trim();
        //        entity.Group = model.Group.Trim();
        //        entity.InputType = model.InputType.Trim();
        //        entity.SortOrder = model.SortOrder;
        //        entity.IsFilterable = model.IsFilterable;
        //        entity.IsRequired = model.IsRequired;
        //        entity.IsActive = model.IsActive;

        //        _db.DefRoomAttributes.Update(entity);
        //        _db.SaveChanges();
        //    }

        //    // TR / EN translation güncelle
        //    UpsertAttributeTranslation(entity.Id, "tr", model.NameTr);
        //    UpsertAttributeTranslation(entity.Id, "en", model.NameEn);

        //    _db.SaveChanges();

        //    return RedirectToAction(nameof(Index));
        //}

        //private void UpsertAttributeTranslation(int attributeId, string langCode, string name)
        //{
        //    var lang = _db.AppLanguages.First(l => l.Code == langCode);

        //    var tr = _db.DefRoomAttributeTranslations
        //        .FirstOrDefault(t => t.DefRoomAttributeId == attributeId
        //                             && t.AppLanguageId == lang.Id);

        //    if (tr == null)
        //    {
        //        tr = new DefRoomAttributeTranslation
        //        {
        //            DefRoomAttributeId = attributeId,
        //            AppLanguageId = lang.Id,
        //            Name = name
        //        };
        //        _db.DefRoomAttributeTranslations.Add(tr);
        //    }
        //    else
        //    {
        //        tr.Name = name;
        //        _db.DefRoomAttributeTranslations.Update(tr);
        //    }
        //}
    }
}
