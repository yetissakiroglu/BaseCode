using Economy.Core.Interfaces;
using Economy.Domain.Entites.TenantEntity.EntityAppContents;
using Economy.Domain.Entites.TenantEntity.EntityAppLanguages;

// Areas/Admin/Controllers/PageBlocksController.cs
using Economy.Panel.UI.Areas.Tenant.Models.AppPageViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Economy.Panel.UI.Areas.Tenant.Controllers
{

    [Area("Tenant")]
    [Route("tenant/page-blocks")]
    public class PageBlocksController : Controller
    {
        private readonly IUnitOfWork _uow;
        private readonly IEntityRepository<AppPage, int> _page;
        private readonly IEntityRepository<AppPageBlock, int> _apppageblock;
        private readonly IEntityRepository<AppLanguage, int> _langRepo;
        private readonly IEntityRepository<AppBlockLibrary, int> _appBlockLibrary;
        public PageBlocksController(IUnitOfWork uow)
        {
            _uow = uow;
            _page = uow.HotelEntityRepository<AppPage>();
            _apppageblock = uow.HotelEntityRepository<AppPageBlock>();
            _langRepo = uow.HotelEntityRepository<AppLanguage>();
            _appBlockLibrary = uow.HotelEntityRepository<AppBlockLibrary>();
        }
        [HttpGet("")]
        public async Task<IActionResult> Index(int? pageId, BlockType? type, string? q, int page = 1, int size = 20)
        {
            var query = _apppageblock.DataSet.Include(pb => pb.AppPage).AsQueryable();
            if (pageId.HasValue) query = query.Where(x => x.AppPageId == pageId.Value);
            if (type.HasValue) query = query.Where(x => x.Type == type.Value);
            if (!string.IsNullOrWhiteSpace(q)) query = query.Where(x => x.SharedJson.Contains(q) || x.AppPage.Slug.Contains(q));

            var total = await query.CountAsync();
            var items = await query.OrderBy(x => x.AppPageId).ThenBy(x => x.SortOrder)
                .Skip((page - 1) * size).Take(size)
                .Select(x => new PageBlockListItemVm
                {
                    Id = x.Id,
                    PageId = x.AppPageId,
                    PageSlug = x.AppPage.Slug,
                    Type = x.Type,
                    SortOrder = x.SortOrder,
                    IsActive = x.IsActive,
                    LibraryBlockId = x.AppBlockLibraryId,
                    IsLinkedToLibrary = x.IsLinkedToLibrary,
                    Stage = x.Stage
                }).ToListAsync();

            return View(new PageBlockIndexVm
            {
                Items = items,
                Page = page,
                Size = size,
                Total = total,
                FilterPageId = pageId,
                FilterType = type,
                Q = q
            });
        }

        [HttpGet("edit/{id}")]
        public async Task<IActionResult> Edit(int id)
        {
            var b = await _apppageblock.DataSet.Include(x => x.Translations).Include(x => x.AppPage).FirstOrDefaultAsync(x => x.Id == id);
            if (b == null) return NotFound();

            var langs = await _langRepo.DataSet.Where(x => x.IsActive).ToListAsync();
            ViewBag.Languages = langs;
            ViewBag.PageInfo = new { PageId=b.AppPageId, b.AppPage.Slug};

            var vm = new PageBlockVm
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
            };
            return View(vm);
        }

        [HttpPost("edit/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, PageBlockVm vm)
        {
            var b = await _apppageblock.DataSet.Include(x => x.Translations).FirstOrDefaultAsync(x => x.Id == id);
            if (b == null) return NotFound();

            b.Type = vm.Type; b.SortOrder = vm.SortOrder; b.IsActive = vm.IsActive; b.Stage = vm.Stage;
            b.SharedJson = vm.SharedJson; b.AppBlockLibraryId = vm.LibraryBlockId; b.IsLinkedToLibrary = vm.IsLinkedToLibrary;

            var langs = await _langRepo.DataSet.Where(x => x.IsActive).ToListAsync();
            foreach (var l in langs)
            {
                var incoming = vm.Translations.First(t => t.LanguageId == l.Id);
                var cur = b.Translations.FirstOrDefault(t => t.AppLanguageId == l.Id);
                if (cur == null) b.Translations.Add(new AppPageBlockTranslation { AppLanguageId = l.Id, LocalizedJson = incoming.LocalizedJson });
                else cur.LocalizedJson = incoming.LocalizedJson;
            }
            await _uow.SaveHotelChangesAsync();
            TempData["ok"] = "Blok kaydedildi";
            return RedirectToAction(nameof(Edit), new { id });
        }

        [HttpPost("clone-to-page")]
        public async Task<IActionResult> CloneToPage(int id, int targetPageId, bool keepLink = false)
        {
            var src = await _apppageblock.DataSet.Include(x => x.Translations).FirstOrDefaultAsync(x => x.Id == id);
            if (src == null) return NotFound();

            var last = await _apppageblock.DataSet.Where(x => x.AppPageId == targetPageId).Select(x => (int?)x.SortOrder).MaxAsync() ?? -1;

            var copy = new AppPageBlock
            {
                AppPageId = targetPageId,
                Type = src.Type,
                SortOrder = last + 1,
                IsActive = src.IsActive,
                Stage = src.Stage,
                SharedJson = keepLink ? "{}" : src.SharedJson,
                AppBlockLibraryId = keepLink ? src.AppBlockLibraryId : null,
                IsLinkedToLibrary = keepLink && src.AppBlockLibraryId.HasValue
            };
            foreach (var t in src.Translations)
                copy.Translations.Add(new AppPageBlockTranslation { AppLanguageId = t.AppLanguageId, LocalizedJson = keepLink ? "{}" : t.LocalizedJson });

            _apppageblock.DataSet.Add(copy);
            await _uow.SaveHotelChangesAsync();
            return Ok(copy.Id);
        }

        [HttpPost("detach")]
        public async Task<IActionResult> Detach(int id)
        {
            var b = await _apppageblock.DataSet.Include(x => x.AppBlockLibrary).ThenInclude(l => l.Translations)
                .Include(x => x.Translations).FirstOrDefaultAsync(x => x.Id == id);
            if (b == null) return NotFound();
            if (b.AppBlockLibraryId == null) return Ok("Zaten bağlı değil");

            if (string.IsNullOrWhiteSpace(b.SharedJson) || b.SharedJson == "{}")
                b.SharedJson = b.AppBlockLibrary?.SharedJson ?? "{}";

            foreach (var tr in b.Translations)
            {
                var libTr = b.AppBlockLibrary?.Translations.FirstOrDefault(x => x.AppLanguageId == tr.AppLanguageId);
                if (tr.LocalizedJson == "{}" && libTr != null) tr.LocalizedJson = libTr.LocalizedJson;
            }
            b.AppBlockLibraryId = null; b.IsLinkedToLibrary = false;
            await _uow.SaveHotelChangesAsync();
            return Ok();
        }
    }


}
