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
        public IActionResult Create(int attributeId)
        {
            //var attr = _db.DefRoomAttributes.FirstOrDefault(x => x.Id == attributeId);
            //if (attr == null) return NotFound();

            var vm = new RoomAttributeOptionEditVm
            {
                DefRoomAttributeId = attributeId,
                //AttributeCode = attr.Code,
                SortOrder = 0
            };

            return View("Edit", vm);
        }

        //[HttpGet]
        //public IActionResult Edit(int id)
        //{
        //    var opt = _db.DefRoomAttributeOptions
        //        .Where(o => o.Id == id)
        //        .Select(o => new
        //        {
        //            Option = o,
        //            Attr = o.DefRoomAttribute,
        //            Tr = o.Translations
        //                .Where(t => t.AppLanguage.Code == "tr")
        //                .FirstOrDefault(),
        //            En = o.Translations
        //                .Where(t => t.AppLanguage.Code == "en")
        //                .FirstOrDefault()
        //        })
        //        .FirstOrDefault();

        //    if (opt == null) return NotFound();

        //    var vm = new RoomAttributeOptionEditVm
        //    {
        //        Id = opt.Option.Id,
        //        DefRoomAttributeId = opt.Option.DefRoomAttributeId,
        //        AttributeCode = opt.Attr.Code,
        //        Value = opt.Option.Value,
        //        SortOrder = opt.Option.SortOrder,
        //        IsActive = opt.Option.IsActive,
        //        DisplayNameTr = opt.Tr?.DisplayName ?? "",
        //        DisplayNameEn = opt.En?.DisplayName ?? ""
        //    };

        //    return View(vm);
        //}

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public IActionResult Edit(RoomAttributeOptionEditVm model)
        //{
        //    if (!ModelState.IsValid)
        //        return View(model);

        //    DefRoomAttributeOption entity;

        //    if (model.Id == 0)
        //    {
        //        entity = new DefRoomAttributeOption
        //        {
        //            DefRoomAttributeId = model.DefRoomAttributeId,
        //            Value = model.Value.Trim(),
        //            SortOrder = model.SortOrder,
        //            IsActive = model.IsActive
        //        };
        //        _db.DefRoomAttributeOptions.Add(entity);
        //        _db.SaveChanges();
        //    }
        //    else
        //    {
        //        entity = _db.DefRoomAttributeOptions
        //            .FirstOrDefault(o => o.Id == model.Id)!;

        //        if (entity == null) return NotFound();

        //        entity.Value = model.Value.Trim();
        //        entity.SortOrder = model.SortOrder;
        //        entity.IsActive = model.IsActive;

        //        _db.DefRoomAttributeOptions.Update(entity);
        //        _db.SaveChanges();
        //    }

        //    UpsertOptionTranslation(entity.Id, "tr", model.DisplayNameTr);
        //    UpsertOptionTranslation(entity.Id, "en", model.DisplayNameEn);

        //    _db.SaveChanges();

        //    return RedirectToAction(nameof(Index), new { attributeId = entity.DefRoomAttributeId });
        //}

        //private void UpsertOptionTranslation(int optionId, string langCode, string displayName)
        //{
        //    var lang = _db.AppLanguages.First(l => l.Code == langCode);

        //    var tr = _db.DefRoomAttributeOptionTranslations
        //        .FirstOrDefault(t => t.DefRoomAttributeOptionId == optionId
        //                             && t.AppLanguageId == lang.Id);

        //    if (tr == null)
        //    {
        //        tr = new DefRoomAttributeOptionTranslation
        //        {
        //            DefRoomAttributeOptionId = optionId,
        //            AppLanguageId = lang.Id,
        //            DisplayName = displayName
        //        };
        //        _db.DefRoomAttributeOptionTranslations.Add(tr);
        //    }
        //    else
        //    {
        //        tr.DisplayName = displayName;
        //        _db.DefRoomAttributeOptionTranslations.Update(tr);
        //    }
        //}
    }
}
