using Economy.Core.Enums;
using Economy.Core.Interfaces;
using Economy.Domain.Entites.TenantEntity.EntityAppBlocks;
using Economy.Domain.Entites.TenantEntity.EntityAppLanguages;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Text.Json;

namespace Economy.Panel.UI.Areas.Tenant.Controllers
{
    [Area("Tenant")]
    [Route("tenant/block-editor")]
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

        // --- Tip-özel modeller ---
        public class HeroSharedVm { [Required] public string VerticalAlign { get; set; } = "center"; public string? BackgroundUrl { get; set; } }
        public class HeroLocVm { [Required, StringLength(60, MinimumLength = 4)] public string Heading { get; set; } = ""; [StringLength(140)] public string? SubHeading { get; set; } public string? ButtonText { get; set; } public string? ButtonUrl { get; set; } }

        public class GallerySharedVm { [Required] public string Mode { get; set; } = "grid"; }
        public class GalleryItemVm { [Required] public string Url { get; set; } = ""; [Required] public string Alt { get; set; } = ""; }
        public class GalleryLocVm { public List<GalleryItemVm> Images { get; set; } = new(); }

        public class FeatureSharedVm { [Range(2, 4)] public int Columns { get; set; } = 4; }
        public class FeatureItemVm { [Required] public string Icon { get; set; } = "wifi"; [Required] public string Title { get; set; } = ""; public string? Description { get; set; } }
        public class FeatureLocVm { public List<FeatureItemVm> Items { get; set; } = new(); }

        // Edit
        [HttpGet("page/{pageBlockId}/{languageId}")]
        public async Task<IActionResult> EditPageBlock(int pageBlockId, int languageId)
        {
            var pb = await _apppageblock.DataSet.Include(x => x.Translations).FirstOrDefaultAsync(x => x.Id == pageBlockId);
            if (pb == null) return NotFound();
            var tr = pb.Translations.FirstOrDefault(t => t.AppLanguageId == languageId);
            if (tr == null) return NotFound();

            ViewBag.LanguageId = languageId;
            ViewBag.PageBlockId = pageBlockId;

            switch (pb.Type)
            {
                case BlockType.Hero:
                    ViewBag.Shared = JsonSerializer.Deserialize<HeroSharedVm>(pb.SharedJson) ?? new();
                    return View("HeroEditor", JsonSerializer.Deserialize<HeroLocVm>(tr.LocalizedJson) ?? new());

                case BlockType.ImageGallery:
                    ViewBag.Shared = JsonSerializer.Deserialize<GallerySharedVm>(pb.SharedJson) ?? new();
                    var g = JsonSerializer.Deserialize<GalleryLocVm>(tr.LocalizedJson) ?? new();
                    if (g.Images.Count == 0) g.Images.Add(new GalleryItemVm { Url = "/media/g1.jpg", Alt = "Görsel" });
                    return View("GalleryEditor", g);

                case BlockType.FeatureGrid:
                    ViewBag.Shared = JsonSerializer.Deserialize<FeatureSharedVm>(pb.SharedJson) ?? new();
                    var f = JsonSerializer.Deserialize<FeatureLocVm>(tr.LocalizedJson) ?? new();
                    if (f.Items.Count == 0) f.Items.Add(new FeatureItemVm { Icon = "wifi", Title = "Ücretsiz Wi-Fi" });
                    return View("FeatureGridEditor", f);

                default:
                    return RedirectToAction("Edit", "BlockGroupBlocks", new { area = "Tenant", id = pb.Id });
            }
        }

        // Save
        [HttpPost("page/{pageBlockId}/{languageId}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SavePageBlock(int pageBlockId, int languageId, string type)
        {
            var pb = await _apppageblock.DataSet.Include(x => x.Translations).FirstOrDefaultAsync(x => x.Id == pageBlockId);
            if (pb == null) return NotFound();
            var tr = pb.Translations.FirstOrDefault(t => t.AppLanguageId == languageId);
            if (tr == null) return NotFound();

            switch (Enum.Parse<BlockType>(type))
            {
                case BlockType.Hero:
                    var sh = new HeroSharedVm(); var lc = new HeroLocVm();
                    await TryUpdateModelAsync(sh); await TryUpdateModelAsync(lc);
                    if (!TryValidateModel(sh) || !TryValidateModel(lc)) return View("HeroEditor", lc);
                    pb.SharedJson = JsonSerializer.Serialize(sh, _json);
                    tr.LocalizedJson = JsonSerializer.Serialize(lc, _json);
                    break;

                case BlockType.ImageGallery:
                    var gsh = new GallerySharedVm(); var glc = new GalleryLocVm();
                    await TryUpdateModelAsync(gsh); await TryUpdateModelAsync(glc, prefix: "SomePrefix");
                    pb.SharedJson = JsonSerializer.Serialize(gsh, _json);
                    tr.LocalizedJson = JsonSerializer.Serialize(glc, _json);
                    break;

                case BlockType.FeatureGrid:
                    var fsh = new FeatureSharedVm(); var flc = new FeatureLocVm();
                    await TryUpdateModelAsync(fsh); await TryUpdateModelAsync(flc);
                    pb.SharedJson = JsonSerializer.Serialize(fsh, _json);
                    tr.LocalizedJson = JsonSerializer.Serialize(flc, _json);
                    break;
            }

            await _uow.SaveHotelChangesAsync();
            TempData["ok"] = "Blok kaydedildi";
            return RedirectToAction(nameof(EditPageBlock), new { pageBlockId, languageId });
        }
    }
}
