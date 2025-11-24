using Economy.Application.Interfaces;
using Economy.Application.TenantUI.Dtos.AppPageDtos;
using Economy.Application.TenantUI.Interfaces;
using Economy.Core.Dtos.Custom;
using Economy.Core.Enums;
using Economy.Panel.UI.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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
