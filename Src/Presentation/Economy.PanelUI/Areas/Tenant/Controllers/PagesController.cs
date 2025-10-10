using Economy.Application.TenantUI.Dtos.AppPageDtos;
using Economy.Application.TenantUI.Interfaces;
using Economy.Core.Dtos.Custom;
using Economy.Core.Enums;
using Economy.Core.Interfaces;
using Economy.Domain.Entites.TenantEntity.EntityAppLanguages;
using Economy.Domain.Entites.TenantEntity.EntityAppPages;
using Economy.Panel.UI.Areas.Tenant.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Economy.Panel.UI.Areas.Tenant.Controllers
{
    [Area("Tenant")]
    [Authorize]
    public sealed class PagesController : Controller
    {
        private readonly IUnitOfWork _uow;
        private readonly IEntityRepository<ContentItem, int> _contentRepo;
        private readonly IEntityRepository<ContentItemTranslation, int> _trRepo;
        private readonly IEntityRepository<AppLanguage, int> _langRepo;
        private readonly IPanelAppPageService _panelAppPageService;
        public PagesController(IUnitOfWork uow, IPanelAppPageService panelAppPageService)
        {
            _uow = uow;
            _contentRepo = uow.HotelEntityRepository<ContentItem>();
            _trRepo = uow.HotelEntityRepository<ContentItemTranslation>();
            _langRepo = uow.HotelEntityRepository<AppLanguage>();
            _panelAppPageService = panelAppPageService;
        }

        // LIST
        public async Task<IActionResult> Index(CancellationToken ct)
        {
            var pageModel = await _panelAppPageService.GetPageListsync();
            return View(pageModel.Data);
        }

        // CREATE
        [HttpGet]
        public async Task<IActionResult> Create(CancellationToken ct)
        {
            var vm = new PageEditDto();
            await _panelAppPageService.FillLanguagesAsync(vm, ct);
            ViewBag.Parents = (await _panelAppPageService.GetParentOptionsAsync(ct)).Data;
            return View(vm);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PageEditVm vm, CancellationToken ct)
        {
            if (!ModelState.IsValid)
            {
                await EnsureLanguageTabsAsync(vm, ct);
                ViewBag.Parents = await GetParentOptionsAsync(ct);
                return View(vm);
            }

            var ci = new ContentItem
            {
                IsDeleted = false,
                IsActive = vm.IsActive,
                PublishAtUtc = vm.PublishAtUtc,
                SortOrder = vm.SortOrder,
                Type = ContentItemType.Page,
                OwnerId = vm.OwnerId
            };
            await _contentRepo.DataSet.AddAsync(ci, ct);
            await _uow.SaveHotelChangesAsync();

            foreach (var t in vm.Translations)
            {
                if (string.IsNullOrWhiteSpace(t.Slug) && string.IsNullOrWhiteSpace(t.Title))
                    continue;

                var tr = new ContentItemTranslation
                {
                    ContentItemId = ci.Id,
                    LanguageId = t.LanguageId,
                    IsDeleted = false,
                    Slug = t.Slug,
                    Title = t.Title,
                    Summary = t.Summary,
                    Body = t.Body,
                    MetaTitle = t.MetaTitle,
                    MetaDescription = t.MetaDescription,
                };
                await _trRepo.DataSet.AddAsync(tr, ct);
            }
            await _uow.SaveHotelChangesAsync();

            TempData["ok"] = "Sayfa oluşturuldu.";
            return RedirectToAction(nameof(Edit), new { id = ci.Id });
        }

        // EDIT
        [HttpGet]
        public async Task<IActionResult> Edit(int id, CancellationToken ct)
        {
            var ci = await _contentRepo.DataSet.FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted, ct);
            if (ci is null) return NotFound();

            var vm = new PageEditVm
            {
                Id = ci.Id,
                OwnerId = ci.OwnerId,
                IsActive = ci.IsActive,
                PublishAtUtc = ci.PublishAtUtc,
                SortOrder = ci.SortOrder,
                Type = (short)ci.Type,
            };

            await FillLanguagesAsync(vm, ct);

            var trs = await _trRepo.DataSet
                .Where(t => !t.IsDeleted && t.ContentItemId == ci.Id)
                .ToListAsync(ct);

            foreach (var t in vm.Translations)
            {
                var hit = trs.FirstOrDefault(x => x.LanguageId == t.LanguageId);
                if (hit is null) continue;

                t.Id = hit.Id;
                t.Slug = hit.Slug;
                t.Title = hit.Title;
                t.Summary = hit.Summary;
                t.Body = hit.Body;
                t.MetaTitle = hit.MetaTitle;
                t.MetaDescription = hit.MetaDescription;
            }

            ViewBag.Parents = await GetParentOptionsAsync(ct, excludeId: ci.Id);
            return View(vm);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, PageEditVm vm, CancellationToken ct)
        {
            if (!ModelState.IsValid)
            {
                await EnsureLanguageTabsAsync(vm, ct);
                ViewBag.Parents = await GetParentOptionsAsync(ct, excludeId: id);
                return View(vm);
            }

            var ci = await _contentRepo.DataSet.FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted, ct);
            if (ci is null) return NotFound();

            ci.OwnerId = vm.OwnerId;
            ci.IsActive = vm.IsActive;
            ci.PublishAtUtc = vm.PublishAtUtc;
            ci.SortOrder = vm.SortOrder;

            var existing = await _trRepo.DataSet
                .Where(t => !t.IsDeleted && t.ContentItemId == id)
                .ToListAsync(ct);

            foreach (var t in vm.Translations)
            {
                var ex = existing.FirstOrDefault(x => x.LanguageId == t.LanguageId);
                if (ex is null)
                {
                    if (string.IsNullOrWhiteSpace(t.Slug) && string.IsNullOrWhiteSpace(t.Title))
                        continue;

                    var tr = new ContentItemTranslation
                    {
                        ContentItemId = id,
                        LanguageId = t.LanguageId,
                        IsDeleted = false,
                        Slug = t.Slug,
                        Title = t.Title,
                        Summary = t.Summary,
                        Body = t.Body,
                        MetaTitle = t.MetaTitle,
                        MetaDescription = t.MetaDescription,
                    };
                    await _trRepo.DataSet.AddAsync(tr, ct);
                }
                else
                {
                    ex.Slug = t.Slug;
                    ex.Title = t.Title;
                    ex.Summary = t.Summary;
                    ex.Body = t.Body;
                    ex.MetaTitle = t.MetaTitle;
                    ex.MetaDescription = t.MetaDescription;
                }
            }

            await _uow.SaveHotelChangesAsync();
            TempData["ok"] = "Sayfa güncellendi.";
            return RedirectToAction(nameof(Edit), new { id });
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id, CancellationToken ct)
        {
            var ci = await _contentRepo.DataSet.FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted, ct);
            if (ci is null) return NotFound();
            ci.IsDeleted = true;
            await _uow.SaveHotelChangesAsync();
            TempData["ok"] = "Sayfa silindi.";
            return RedirectToAction(nameof(Index));
        }

        // ---------------- helpers ----------------
        private async Task<List<PageParentOptionVm>> GetParentOptionsAsync(CancellationToken ct, int? excludeId = null)
        {
            var q = _contentRepo.DataSet.Where(x => !x.IsDeleted && x.IsActive && x.Type == ContentItemType.Page);
            if (excludeId.HasValue) q = q.Where(x => x.Id != excludeId.Value);

            var defLangId = await _langRepo.DataSet.Where(l => !l.IsDeleted && l.IsActive && l.IsDefault)
                .Select(l => l.Id).FirstOrDefaultAsync(ct);

            return await (from ci in q
                          join tr in _trRepo.DataSet on ci.Id equals tr.ContentItemId
                          where !tr.IsDeleted && tr.LanguageId == defLangId
                          orderby ci.SortOrder, ci.Id
                          select new PageParentOptionVm { Id = ci.Id, Title = tr.Title ?? ("#" + ci.Id) })
                         .ToListAsync(ct);
        }

        private async Task FillLanguagesAsync(PageEditVm vm, CancellationToken ct)
        {
            var langs = await _langRepo.DataSet
                .Where(x => !x.IsDeleted && x.IsActive)
                .OrderByDescending(x => x.IsDefault)
                .ThenBy(x => x.Id)
                .Select(x => new { x.Id, x.Code })
                .ToListAsync(ct);

            vm.Translations = langs.Select(l => new PageTranslationVm
            {
                LanguageId = l.Id,
                LanguageCode = l.Code
            }).ToList();
        }

        private async Task EnsureLanguageTabsAsync(PageEditVm vm, CancellationToken ct)
        {
            var exist = vm.Translations.Select(t => t.LanguageId).ToHashSet();
            var langs = await _langRepo.DataSet.Where(x => !x.IsDeleted && x.IsActive)
                .Select(x => new { x.Id, x.Code }).ToListAsync(ct);

            foreach (var l in langs)
                if (!exist.Contains(l.Id))
                    vm.Translations.Add(new PageTranslationVm { LanguageId = l.Id, LanguageCode = l.Code });

            vm.Translations = vm.Translations
                .OrderByDescending(t => t.LanguageCode == "tr")
                .ThenBy(t => t.LanguageId)
                .ToList();
        }



        public IActionResult CreateTest()
        {
            var vm = new PageCreateVm
            {
                Singles =
            {
                new ImageFieldVm { Key="KapakImage", Label="Kapak Görseli" },
                new ImageFieldVm { Key="OgKapakImage", Label="OG Kapak Görseli" }
            },
                Galleries =
            {
                new GalleryGroupVm { Key="galeri", Label="Galeri Fotoğrafları" },
                new GalleryGroupVm { Key="oda", Label="Oda Fotoğrafları" }
            }
            };
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateTest(PageCreateVm vm)
        {
            if (!ModelState.IsValid) return View(vm);

            // TODO: DB kaydı (vm.Title, vm.Content, vm.ImageUrl (kapak), vm.ImageUrls (galeri))
            TempData["ok"] = "Sayfa oluşturuldu (demo).";
            return RedirectToAction(nameof(Create));
        }

        public class PageCreateVm
        {
            public string Title { get; set; } = string.Empty;
            public string? Content { get; set; }

            public string? ImageUrl { get; set; }       // Kapak
            public string? OgImageUrl { get; set; }       // Kapak

            public List<string> ImageUrls { get; set; } = new(); // Galeri


            public List<ImageFieldVm> Singles { get; set; } = new();
            public List<GalleryGroupVm> Galleries { get; set; } = new();
        }

    }
}
