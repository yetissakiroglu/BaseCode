using Economy.Application.Interfaces;
using Economy.Application.TenantUI.Dtos;
using Economy.Application.TenantUI.Dtos.AppPageDtos;
using Economy.Application.TenantUI.Interfaces;
using Economy.Core.Dtos.Custom;
using Economy.Core.Enums;
using Economy.Domain.Entites.TenantEntity.EntityAppPages;
using Economy.Panel.UI.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Economy.Panel.UI.Areas.Tenant.Controllers
{
    [Area("Tenant")]
    [Authorize]
    public class RoomsController : BaseController
    {
        private readonly IPanelAppPageService _panelAppPageService;
        private readonly IPanelAppPageMediaService _panelAppPageMediaService;
        private readonly ISlugService _slugService;
        public RoomsController(IPanelAppPageService panelAppPageService, IPanelAppPageMediaService panelAppPageMediaService, ISlugService slugService)
        {
            _panelAppPageService = panelAppPageService;
            _panelAppPageMediaService = panelAppPageMediaService;
            _slugService = slugService;
        }

        public async Task<IActionResult> Index(CancellationToken ct)
        {
            var pageModel = await _panelAppPageService.GetPageListAsync(ContentItemType.Room, ct);
            return View(pageModel.Data);
        }
     
        [HttpGet]
        public async Task<IActionResult> Create(CancellationToken ct)
        {
            var vm = new PageEditDto();
            vm.Type = ContentItemType.Room;
            await _panelAppPageService.FillLanguagesAsync(vm, ct);
            ViewBag.Parents = (await _panelAppPageService.GetParentOptionsAsync(ct)).Data;
            return View(vm);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PageEditDto vm, CancellationToken ct)
        {
            if (!ModelState.IsValid)
            {
                await _panelAppPageService.EnsureLanguageTabsAsync(vm, ct);
                ViewBag.Parents = (await _panelAppPageService.GetParentOptionsAsync(ct)).Data;
                return View(vm);
            }

            var resultNew = await _panelAppPageService.CreateEdit(vm, ct);
            AddMessage(resultNew);
            if (!resultNew.IsSuccess)
            {
                return RedirectToAction(nameof(Create), vm);
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id, CancellationToken ct)
        {
            var result = await _panelAppPageService.GetPageAsync(id, ct);
            if (!result.HasData)
            {
                AddMessage(result);
            }

            ViewBag.Parents = (await _panelAppPageService.GetParentOptionsAsync(ct, excludeId: result.Data.Id)).Data;
            return View(result.Data);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, PageEditDto vm, CancellationToken ct)
        {
            if (!ModelState.IsValid)
            {
                await _panelAppPageService.EnsureLanguageTabsAsync(vm, ct);
                ViewBag.Parents = (await _panelAppPageService.GetParentOptionsAsync(ct, excludeId: id)).Data;
                return View(vm);
            }
            vm.Id = id;
            var resultEdit = await _panelAppPageService.CreateEdit(vm, ct);
            AddMessage(resultEdit);

            return RedirectToAction(nameof(Edit), new { id });
        }

        // Oda özellikleri düzenleme ekranı
        [HttpGet]
        public async Task<IActionResult> EditAttributes(int id, CancellationToken ct) // id = RoomId
        {
            var result  = await _panelAppPageService.GetPageEditAttributesAsync(id, ct);
            return View("EditAttributes", result.Data);
        }

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public IActionResult SaveAttributes(RoomAttributeValueEditVm model)
        //{
        //    var room = _db.Rooms.FirstOrDefault(r => r.Id == model.RoomId);
        //    if (room == null)
        //        return NotFound();

        //    // Tüm attribute’lar (tip bilgisi lazım)
        //    var attrs = _db.DefRoomAttributes
        //        .Where(a => a.IsActive)
        //        .ToList();

        //    // Mevcut değerleri çek
        //    var existingValues = _db.RoomAttributeValues
        //        .Where(v => v.RoomId == model.RoomId)
        //        .ToList();

        //    var existingDict = existingValues
        //        .ToDictionary(v => v.DefRoomAttributeId, v => v);

        //    // Her attribute için gelen değeri işle
        //    foreach (var attr in attrs)
        //    {
        //        model.Values.TryGetValue(attr.Id, out var input);

        //        // Zorunlu kontrolü istersen burada yapabilirsin:
        //        // if (attr.IsRequired && (input == null || (input.OptionId == null && input.BoolValue == null && ...)))

        //        RoomAttributeValue value;
        //        var hasExisting = existingDict.TryGetValue(attr.Id, out value);

        //        // Attribute POST içinde yoksa ve gereksizse atlayabilirsin
        //        if (input == null)
        //        {
        //            // İstersen mevcut kayıt varsa ve IsRequired == false ise silebilirsin
        //            // if (hasExisting && !attr.IsRequired) _db.RoomAttributeValues.Remove(value);
        //            continue;
        //        }

        //        if (!hasExisting)
        //        {
        //            value = new RoomAttributeValue
        //            {
        //                RoomId = model.RoomId,
        //                DefRoomAttributeId = attr.Id
        //            };
        //            _db.RoomAttributeValues.Add(value);
        //            existingDict[attr.Id] = value;
        //        }

        //        // Önce eski değerleri temizle
        //        value.DefRoomAttributeOptionId = null;
        //        value.ValueBool = null;
        //        value.ValueInt = null;
        //        value.ValueText = null;

        //        // Attribute tipine göre doğru alanı doldur
        //        switch (attr.InputType)
        //        {
        //            case "Option":
        //                value.DefRoomAttributeOptionId = input.OptionId;
        //                break;

        //            case "Bool":
        //                // Bool checkbox gönderilmezse null gelir, onu false’a çekebilirsin
        //                value.ValueBool = input.BoolValue ?? false;
        //                break;

        //            case "Number":
        //                value.ValueInt = input.IntValue;
        //                break;

        //            case "Text":
        //                value.ValueText = input.TextValue;
        //                break;
        //        }

        //        // Eğer attribute zorunlu değilse ve değeri tamamen boş bırakıldıysa kayıt silebilirsin
        //        if (!attr.IsRequired
        //            && value.DefRoomAttributeOptionId == null
        //            && value.ValueBool == null
        //            && value.ValueInt == null
        //            && string.IsNullOrWhiteSpace(value.ValueText))
        //        {
        //            if (hasExisting)
        //                _db.RoomAttributeValues.Remove(value);
        //        }
        //    }

        //    _db.SaveChanges();

        //    // Oda edit sayfasına dön
        //    return RedirectToAction("Edit", new { id = model.RoomId });
        //}










        public async Task<IActionResult> test(CancellationToken ct)
        {
            var test = new RoomCreateEditDto();
            return View(test);
        }




        public record RoomCreateEditDto
        {
            public int Id { get; set; }

            // 2) Odanın Temel Özeti (Property Highlights)
            public string RoomType { get; set; } = "";          // Standart, Deluxe, Suit...
            public int Capacity { get; set; }                   // 2, 3, 4 kişi
            public int SquareMeter { get; set; }                // 25 m²
            public string BedType { get; set; } = "";           // King, Double, Twin
            public string ViewType { get; set; } = "";          // Deniz, Dağ, Bahçe
            public bool IsSmokingAllowed { get; set; }          // Sigara: İçilebilir/İçilmez
            public bool HasBalconyOrTerrace { get; set; }       // Balkon/Teras
            public string ClimateType { get; set; } = "";       // Split, Merkezi
            public bool HasWifi { get; set; }                   // Ücretsiz Wi-Fi
            public bool HasParking { get; set; }                // Ücretsiz Otopark

            // 3) Oda Açıklaması (4–6 cümlelik ikna metni)
            public string Description { get; set; } = "";

            // 4) Oda Özellikleri (Amenity Listesi)

            // GENEL (bunları zaten üstten alıyoruz, ayrı bool gerek yok; 
            // gösterim tarafında SquareMeter/BedType/Capacity/IsSmokingAllowed kullanırsın)

            // KONFOR
            public bool Amenity_Comfort_Heating { get; set; }          // Isıtma
            public bool Amenity_Comfort_SoundIsolation { get; set; }   // Ses yalıtımı
            public bool Amenity_Comfort_BlackoutCurtain { get; set; }  // Karartma perdesi

            // TEKNOLOJİ
            public bool Amenity_Tech_SmartTv { get; set; }
            public bool Amenity_Tech_NetflixYoutube { get; set; }
            public bool Amenity_Tech_UsbSocket { get; set; }
            public bool Amenity_Tech_FastWifi { get; set; }            // (HasWifi ile aynı mantık, 
                                                                       // ama listede ayrı satır istersen)

            // BANYO
            public bool Amenity_Bath_Shower { get; set; }              // Duş
            public bool Amenity_Bath_Bathtub { get; set; }             // Küvet
            public bool Amenity_Bath_HairDryer { get; set; }           // Saç kurutma
            public bool Amenity_Bath_TowelSet { get; set; }            // Havlu seti
            public bool Amenity_Bath_Toiletries { get; set; }          // Buklet ürünler

            // MUTFAK (varsa)
            public bool Amenity_Kitchen_MiniFridge { get; set; }       // Mini buzdolabı
            public bool Amenity_Kitchen_Kettle { get; set; }           // Su ısıtıcısı
            public bool Amenity_Kitchen_TeaCoffeeSet { get; set; }     // Çay–kahve seti
            public bool Amenity_Kitchen_StoveOrMicrowave { get; set; } // Ocak / Mikro dalga

            // GÜVENLİK
            public bool Amenity_Security_SmokeDetector { get; set; }   // Yangın dedektörü
            public bool Amenity_Security_SafeBox { get; set; }         // Kasa
            public bool Amenity_Security_24hHotWater { get; set; }     // 24 saat sıcak su

            // DIŞ ALAN
            public bool Amenity_Outdoor_BalconyTerrace { get; set; }   // Balkon/Teras (HasBalconyOrTerrace ile aynı, ama listede)
            public bool Amenity_Outdoor_GardenAccess { get; set; }     // Bahçe çıkışı
        }
    }
}
