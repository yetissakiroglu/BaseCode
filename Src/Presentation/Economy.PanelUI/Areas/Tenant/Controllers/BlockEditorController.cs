using Economy.Core.Enums;
using Economy.Core.Interfaces;
using Economy.Domain.Entites.TenantEntity.EntityAppBlocks;
using Economy.Domain.Entites.TenantEntity.EntityAppLanguages;
using Economy.Panel.UI.Areas.Tenant.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace Economy.Panel.UI.Areas.Tenant.Controllers
{
    [Area("Tenant")]
    [Authorize]
    public class BlockEditorController : Controller
    {
        private readonly JsonSerializerOptions _json = new() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase, WriteIndented = true };
        private readonly IUnitOfWork _uow;
        private readonly IEntityRepository<BlockGroupBlock, int> _apppageblock;
        private readonly IEntityRepository<AppLanguage, int> _langRepo;

        public BlockEditorController(IUnitOfWork uow)
        {
            _uow = uow;
            _apppageblock = uow.HotelEntityRepository<BlockGroupBlock>();
            _langRepo = uow.HotelEntityRepository<AppLanguage>();
        }


        // Edit
        // Faz-1 uyumlu, güvenli ve esnek EditBlockEditor (Hero, Text, ImageGallery, AmenityGroup)
        [HttpGet("tenant/blockeditor/{pageBlockId:int}/{languageId:int}")]
        public async Task<IActionResult> EditBlockEditor(int pageBlockId, int languageId)
        {
            // 1) Blok + çeviriler
            var pb = await _apppageblock.DataSet
                .Include(x => x.Translations)
                .FirstOrDefaultAsync(x => x.Id == pageBlockId);

            if (pb == null) return NotFound();

            // 2) İstenen dilde çeviri (yoksa boş bir taslak üretelim – GET’te 404 vermeyelim)
            var tr = pb.Translations.FirstOrDefault(t => t.AppLanguageId == languageId);
            if (tr == null)
            {
                tr = new BlockGroupBlockTranslation
                {
                    AppLanguageId = languageId,
                    LocalizedJson = "{}"
                };
                // Not: DB'ye eklemiyoruz (GET). Sadece editorü doldurmak için geçici.
            }

            // 3) ViewBag meta
            ViewBag.LanguageId = languageId;
            ViewBag.PageBlockId = pageBlockId;

            // (opsiyonel) Dil adını göster: servisinden veya cache’inden al
            try
            {
                var lang = _langRepo.DataSet.FirstOrDefault(x=>x.Id==languageId);
                ViewBag.LanguageName = lang?.Name;
            }
            catch { /* yoksa sorun değil */ }

            // 4) JSON deserialize ayarları (camelCase desteği)
            var jsonOpts = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            // 5) Şema gereği varsayılanları güvenli şekilde uygula ve ilgili editöre yönlendir
            switch (pb.Type)
            {
                case BlockType.Hero:
                    {
                        var shared = SafeDeserialize(pb.SharedJson, () => new HeroSharedVm
                        {
                            BackgroundUrl = "/media/hero.jpg",
                            VerticalAlign = "center"
                        }, jsonOpts);

                        var loc = SafeDeserialize(tr.LocalizedJson, () => new HeroLocVm
                        {
                            Heading = "Başlık",
                            SubHeading = "Alt başlık",
                            ButtonText = "Devam",
                            ButtonUrl = "/"
                        }, jsonOpts);

                        // Son güvenlik (eksik alanları tamamla)
                        shared.BackgroundUrl ??= "/media/hero.jpg";
                        shared.VerticalAlign ??= "center";
                        loc.ButtonUrl ??= "/";

                        ViewBag.Shared = shared;
                        return View("HeroEditor", loc);
                    }

                case BlockType.Text:
                    {
                        // Shared {}
                        ViewBag.Shared = new object();

                        var loc = SafeDeserialize(tr.LocalizedJson, () => new TextLocVm
                        {
                            Heading = "Bölüm",
                            BodyHtml = "<p>Metin…</p>"
                        }, jsonOpts);

                        loc.Heading ??= "Bölüm";
                        loc.BodyHtml ??= "<p>Metin…</p>";

                        return View("TextEditor", loc);
                    }

                case BlockType.ImageGallery:
                    {
                        var shared = SafeDeserialize(pb.SharedJson, () => new GallerySharedVm
                        {
                            Mode = "grid",
                            ImageUrls = new List<string>()
                        }, jsonOpts);

                        shared.Mode ??= "grid";
                        shared.ImageUrls ??= new List<string>();
                        if (shared.ImageUrls.Count == 0)
                            shared.ImageUrls.Add("/media/g1.jpg"); // örnek

                        // Localized {} (Faz-1’de boş)
                        var loc = SafeDeserialize(tr.LocalizedJson, () => new GalleryLocVm(), jsonOpts);

                        ViewBag.Shared = shared;
                        return View("GalleryEditor", loc);
                    }

                case BlockType.AmenityGroup:
                    {
                        // Shared {}
                        ViewBag.Shared = new object();

                        var loc = SafeDeserialize(tr.LocalizedJson, () => new AmenityLocVm
                        {
                            GroupTitle = "Oda Olanakları",
                            Amenities = new List<string> { "Ücretsiz Wi-Fi", "Klima", "TV" }
                        }, jsonOpts);

                        loc.GroupTitle ??= "Oda Olanakları";
                        loc.Amenities ??= new List<string>();
                        if (loc.Amenities.Count == 0)
                            loc.Amenities.AddRange(new[] { "Ücretsiz Wi-Fi", "Klima", "TV" });

                        return View("AmenityEditor", loc);
                    }

                default:
                    // Faz-1 dışı tipler için ana editöre dön
                    return RedirectToAction("Edit", "BlockGroupBlocks", new { area = "Tenant", id = pb.Id });
            }

            // Lokal helper: hatalı JSON’da bile fallback üreten güvenli deserialize
            static T SafeDeserialize<T>(string? json, Func<T> fallback, JsonSerializerOptions opts)
            {
                if (string.IsNullOrWhiteSpace(json)) return fallback();
                try { return JsonSerializer.Deserialize<T>(json, opts) ?? fallback(); }
                catch { return fallback(); }
            }
        }


        // Save
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SaveBlockEditor(int pageBlockId, int languageId, string type)
        {
            var pb = await _apppageblock.DataSet
                .Include(x => x.Translations)
                .FirstOrDefaultAsync(x => x.Id == pageBlockId);
            if (pb == null) return NotFound();

            var tr = pb.Translations.FirstOrDefault(t => t.AppLanguageId == languageId);
            if (tr == null) return NotFound();

            // Ortak ViewBag'ler: hata durumunda aynı editöre dönerken lazım
            ViewBag.LanguageId = languageId;
            ViewBag.PageBlockId = pageBlockId;
            try
            {
                var lang = _langRepo.DataSet.FirstOrDefault(x => x.Id == languageId);
                ViewBag.LanguageName = lang?.Name;
            }
            catch { /* opsiyonel */ }

            if (!Enum.TryParse<BlockType>(type, ignoreCase: true, out var bt))
                return BadRequest("Geçersiz blok tipi.");

            switch (bt)
            {
                case BlockType.Hero:
                    {
                        var sh = new HeroSharedVm();
                        var lc = new HeroLocVm();

                        // Shared.* alanları için prefix kullan
                        await TryUpdateModelAsync(sh, prefix: "Shared");
                        await TryUpdateModelAsync(lc);

                        if (!TryValidateModel(sh) | !TryValidateModel(lc))
                        {
                            // View'a geri dönerken Shared'ı da tekrar gönder
                            ViewBag.Shared = sh;
                            return View("HeroEditor", lc);
                        }

                        pb.SharedJson = JsonSerializer.Serialize(sh, _json);
                        tr.LocalizedJson = JsonSerializer.Serialize(lc, _json);
                        break;
                    }

                case BlockType.Text:
                    {
                        // Shared sözleşmesi boş {}
                        var lc = new TextLocVm();
                        await TryUpdateModelAsync(lc);

                        if (!TryValidateModel(lc))
                        {
                            ViewBag.Shared = new object();
                            return View("TextEditor", lc);
                        }

                        // SharedJson'u boş sözleşmeye sabitliyoruz
                        pb.SharedJson = JsonSerializer.Serialize(new { }, _json);
                        tr.LocalizedJson = JsonSerializer.Serialize(lc, _json);
                        break;
                    }

                case BlockType.ImageGallery:
                    {
                        var gsh = new GallerySharedVm();
                        // Faz-1'de Localized {} — yine de bind etmeye çalışmak zararsız
                        var glc = new GalleryLocVm();

                        await TryUpdateModelAsync(gsh, prefix: "Shared");
                        await TryUpdateModelAsync(glc); // alan yoksa ModelState etkilenmez

                        if (!TryValidateModel(gsh))
                        {
                            ViewBag.Shared = gsh;
                            return View("GalleryEditor", glc);
                        }

                        pb.SharedJson = JsonSerializer.Serialize(gsh, _json);
                        // Boş sözleşme: {} — istersen mevcut LocalizedJson'u korumak da mümkün
                        tr.LocalizedJson = JsonSerializer.Serialize(glc, _json);
                        break;
                    }

                case BlockType.AmenityGroup:
                    {
                        // Shared sözleşmesi yok {}
                        var lc = new AmenityLocVm();
                        await TryUpdateModelAsync(lc);

                        if (!TryValidateModel(lc))
                        {
                            ViewBag.Shared = new object();
                            return View("AmenityEditor", lc);
                        }

                        pb.SharedJson = JsonSerializer.Serialize(new { }, _json);
                        tr.LocalizedJson = JsonSerializer.Serialize(lc, _json);
                        break;
                    }

                default:
                    return BadRequest("Bu blok tipi bu ekranda güncellenemez.");
            }

            await _uow.SaveHotelChangesAsync();
            TempData["ok"] = "Blok kaydedildi";
            return RedirectToAction(nameof(EditBlockEditor), new { pageBlockId, languageId });
        }

    }
}
