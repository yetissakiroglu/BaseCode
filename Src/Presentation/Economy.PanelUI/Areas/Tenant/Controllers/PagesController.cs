// Areas/Admin/Controllers/PagesController.cs
using Economy.Core.Interfaces;
using Economy.Domain.Entites.TenantEntity.EntityAppContents;
using Economy.Domain.Entites.TenantEntity.EntityAppLanguages;
using Economy.Panel.UI.Areas.Tenant.Models.AppPageViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Economy.Panel.UI.Areas.Tenant.Controllers
{


    [Area("Tenant")]
    [Route("tenant/pages")]
    public class PagesController : Controller
    {
        private readonly IUnitOfWork _uow;
        private readonly IEntityRepository<AppPage, int> _page;
        private readonly IEntityRepository<AppPageBlock, int> _apppageblock;
        private readonly IEntityRepository<AppLanguage, int> _langRepo;

        public PagesController(IUnitOfWork uow)
        {
            _uow = uow;
            _page = uow.HotelEntityRepository<AppPage>();
            _apppageblock = uow.HotelEntityRepository<AppPageBlock>();
            _langRepo = uow.HotelEntityRepository<AppLanguage>();
        }

        [HttpGet("")]
        public async Task<IActionResult> Index()
        {
            var pages = await _page.DataSet.Where(p => !p.IsDeleted)
                .OrderBy(p => p.Slug).ToListAsync();
            return View(pages);
        }

        [HttpGet("edit/{id?}")]
        public async Task<IActionResult> Edit(int? id)
        {
            var langs = await _langRepo.DataSet.Where(x => x.IsActive).OrderBy(x => x.Id).ToListAsync();
            ViewBag.Languages = langs;

            if (id == null)
            {
                return View(new PageEditVm
                {
                    Slug = "",
                    Stage = ContentStage.Draft,
                    Translations = langs.Select(l => new PageTranslationVm { LanguageId = l.Id }).ToList()
                });
            }

            var p = await _page.DataSet
                .Include(x => x.Translations)
                .Include(x => x.AppPageBlocks).ThenInclude(b => b.Translations)
                .FirstOrDefaultAsync(x => x.Id == id);
            if (p == null) return NotFound();

            var vm = new PageEditVm
            {
                Id = p.Id,
                Slug = p.Slug,
                IsActive = p.IsActive,
                Stage = p.Stage,
                Translations = langs.Select(l =>
                {
                    var tr = p.Translations.FirstOrDefault(t => t.AppLanguageId == l.Id);
                    return new PageTranslationVm { Id = tr?.Id, LanguageId = l.Id, Title = tr?.Title ?? "", SeoTitle = tr?.SeoTitle, SeoDescription = tr?.SeoDescription };
                }).ToList(),
                Blocks = p.AppPageBlocks.OrderBy(b => b.SortOrder).Select(b => new PageBlockVm
                {
                    Id = b.Id,
                    Type = b.Type,
                    SortOrder = b.SortOrder,
                    IsActive = b.IsActive,
                    Stage = b.Stage,
                    SharedJson = b.SharedJson,
                    LibraryBlockId = b.AppBlockLibraryId,
                    IsLinkedToLibrary = b.IsLinkedToLibrary,
                    Translations = langs.Select(l => {
                        var bt = b.Translations.FirstOrDefault(t => t.AppLanguageId == l.Id);
                        return new PageBlockTranslationVm { Id = bt?.Id, LanguageId = l.Id, LocalizedJson = bt?.LocalizedJson ?? "{}" };
                    }).ToList()
                }).ToList()
            };
            return View(vm);
        }

        [HttpPost("edit/{id?}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(PageEditVm vm)
        {
            var langs = await _langRepo.DataSet.Where(x => x.IsActive).ToListAsync();
            if (!ModelState.IsValid) return View(vm);

            AppPage p;
            if (vm.Id == null)
            {
                p = new AppPage { Slug = vm.Slug, IsActive = vm.IsActive, Stage = vm.Stage };
                foreach (var t in vm.Translations)
                    p.Translations.Add(new AppPageTranslation { AppLanguageId = t.LanguageId, Title = t.Title, SeoTitle = t.SeoTitle, SeoDescription = t.SeoDescription });
                _page.DataSet.Add(p);
            }
            else
            {
                p = await _page.DataSet.Include(x => x.Translations).Include(x => x.AppPageBlocks).ThenInclude(x => x.Translations)
                    .FirstAsync(x => x.Id == vm.Id.Value);
                p.Slug = vm.Slug; p.IsActive = vm.IsActive; p.Stage = vm.Stage;

                // Sayfa çevirileri
                foreach (var l in langs)
                {
                    var incoming = vm.Translations.First(t => t.LanguageId == l.Id);
                    var cur = p.Translations.FirstOrDefault(t => t.AppLanguageId == l.Id);
                    if (cur == null) p.Translations.Add(new AppPageTranslation { AppLanguageId = l.Id, Title = incoming.Title, SeoTitle = incoming.SeoTitle, SeoDescription = incoming.SeoDescription });
                    else { cur.Title = incoming.Title; cur.SeoTitle = incoming.SeoTitle; cur.SeoDescription = incoming.SeoDescription; }
                }

                // Silinen bloklar
                var keep = vm.Blocks.Where(b => b.Id.HasValue).Select(b => b.Id!.Value).ToHashSet();
                var toRemove = p.AppPageBlocks.Where(x => !keep.Contains(x.Id)).ToList();
                _apppageblock.DataSet.RemoveRange(toRemove);
            }

            // Blok upsert + sıralama
            int order = 0;
            foreach (var bvm in vm.Blocks.OrderBy(x => x.SortOrder))
            {
                AppPageBlock e;
                if (bvm.Id == null)
                {
                    e = new AppPageBlock
                    {
                        Type = bvm.Type,
                        SortOrder = order++,
                        IsActive = bvm.IsActive,
                        Stage = bvm.Stage,
                        SharedJson = bvm.SharedJson,
                        AppBlockLibraryId = bvm.LibraryBlockId,
                        IsLinkedToLibrary = bvm.IsLinkedToLibrary
                    };
                    foreach (var bt in bvm.Translations)
                        e.Translations.Add(new AppPageBlockTranslation { AppLanguageId = bt.LanguageId, LocalizedJson = bt.LocalizedJson });
                    p.AppPageBlocks.Add(e);
                }
                else
                {
                    e = p.AppPageBlocks.First(x => x.Id == bvm.Id.Value);
                    e.Type = bvm.Type; e.SortOrder = order++; e.IsActive = bvm.IsActive; e.Stage = bvm.Stage; e.SharedJson = bvm.SharedJson;
                    e.AppBlockLibraryId = bvm.LibraryBlockId; e.IsLinkedToLibrary = bvm.IsLinkedToLibrary;

                    foreach (var l in langs)
                    {
                        var incoming = bvm.Translations.First(t => t.LanguageId == l.Id);
                        var cur = e.Translations.FirstOrDefault(t => t.AppLanguageId == l.Id);
                        if (cur == null) e.Translations.Add(new AppPageBlockTranslation { AppLanguageId = l.Id, LocalizedJson = incoming.LocalizedJson });
                        else cur.LocalizedJson = incoming.LocalizedJson;
                    }
                }
            }

            await _uow.SaveHotelChangesAsync();
            TempData["ok"] = "Sayfa kaydedildi";
            return RedirectToAction(nameof(Edit), new { id = p.Id });
        }

        // Drag&drop reorder (AJAX)
        [HttpPost("reorder")]
        public async Task<IActionResult> Reorder([FromBody] ReorderReq req)
        {
            var list = await _apppageblock.DataSet.Where(x => x.AppPageId == req.PageId && req.Items.Select(i => i.Id).Contains(x.Id)).ToListAsync();
            foreach (var i in req.Items) list.First(x => x.Id == i.Id).SortOrder = i.NewOrder;
            await _uow.SaveDefaultChangesAsync();
            return Ok();
        }

        public record ReorderReq(int PageId, List<Row> Items);
        public record Row(int Id, int NewOrder);

        // Content-grid: yeni kart üretir
        [HttpGet("block-card-partial")]
        public IActionResult BlockCardPartial(BlockType type, int index, List<int> languageIds)
        {
            var langs = _langRepo.DataSet.Where(x => x.IsActive).OrderBy(x => x.Id).ToList();
            ViewBag.Languages = langs;

            var vm = new PageBlockVm
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
                Translations = languageIds.Select(lid => new PageBlockTranslationVm
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
    }


}
