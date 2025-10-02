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
    public sealed class SnippetsController : Controller
    {
        private readonly IUnitOfWork _uow;
        private readonly IEntityRepository<ContentItem, int> _contentRepo;
        private readonly IEntityRepository<ContentItemTranslation, int> _trRepo;
        private readonly IEntityRepository<AppLanguage, int> _langRepo;

        public SnippetsController(IUnitOfWork uow)
        {
            _uow = uow;
            _contentRepo = uow.HotelEntityRepository<ContentItem>();
            _trRepo = uow.HotelEntityRepository<ContentItemTranslation>();
            _langRepo = uow.HotelEntityRepository<AppLanguage>();
        }

        public async Task<IActionResult> Index(CancellationToken ct)
        {
            var list = await _contentRepo.DataSet
                .Where(x => !x.IsDeleted && x.Type == ContentItemType.Snippet)
                .OrderBy(x => x.SortOrder).ThenBy(x => x.Id)
                .Select(x => new SnippetListItemVm
                {
                    Id = x.Id,
                    Code = x.Code!,
                    IsActive = x.IsActive
                }).ToListAsync(ct);
            return View(list);
        }

        [HttpGet]
        public async Task<IActionResult> Create(CancellationToken ct)
        {
            var vm = new SnippetEditVm { IsActive = true, Translations = await BuildLangTabs(ct) };
            return View(vm);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SnippetEditVm vm, CancellationToken ct)
        {
            if (string.IsNullOrWhiteSpace(vm.Code))
            {
                ModelState.AddModelError("Code", "Kod zorunlu");
                vm.Translations = await BuildLangTabs(ct);
                return View(vm);
            }

            var s = new ContentItem
            {
                Type = ContentItemType.Snippet,
                Code = vm.Code,
                IsActive = vm.IsActive,
                IsDeleted = false,
                SortOrder = 0
            };
            await _contentRepo.DataSet.AddAsync(s, ct);
            await _uow.SaveHotelChangesAsync();

            foreach (var t in vm.Translations)
            {
                if (string.IsNullOrWhiteSpace(t.Body) && string.IsNullOrWhiteSpace(t.Title))
                    continue;

                var tr = new ContentItemTranslation
                {
                    ContentItemId = s.Id,
                    LanguageId = t.LanguageId,
                    Title = t.Title,
                    Body = t.Body,
                    IsDeleted = false
                };
                await _trRepo.DataSet.AddAsync(tr, ct);
            }
            await _uow.SaveHotelChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id, CancellationToken ct)
        {
            var s = await _contentRepo.DataSet.FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted, ct);
            if (s is null) return NotFound();

            var vm = new SnippetEditVm
            {
                Id = s.Id,
                Code = s.Code!,
                IsActive = s.IsActive,
                Translations = await BuildLangTabs(ct)
            };

            var trs = await _trRepo.DataSet.Where(t => !t.IsDeleted && t.ContentItemId == id).ToListAsync(ct);
            foreach (var t in vm.Translations)
            {
                var ex = trs.FirstOrDefault(x => x.LanguageId == t.LanguageId);
                if (ex is null) continue;
                t.Title = ex.Title;
                t.Body = ex.Body;
            }
            return View(vm);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, SnippetEditVm vm, CancellationToken ct)
        {
            var s = await _contentRepo.DataSet.FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted, ct);
            if (s is null) return NotFound();

            s.Code = vm.Code;
            s.IsActive = vm.IsActive;

            var existing = await _trRepo.DataSet.Where(t => !t.IsDeleted && t.ContentItemId == id).ToListAsync(ct);

            foreach (var t in vm.Translations)
            {
                var ex = existing.FirstOrDefault(x => x.LanguageId == t.LanguageId);
                if (ex is null)
                {
                    if (string.IsNullOrWhiteSpace(t.Title) && string.IsNullOrWhiteSpace(t.Body))
                        continue;

                    var tr = new ContentItemTranslation
                    {
                        ContentItemId = id,
                        LanguageId = t.LanguageId,
                        Title = t.Title,
                        Body = t.Body,
                        IsDeleted = false
                    };
                    await _trRepo.DataSet.AddAsync(tr, ct);
                }
                else
                {
                    ex.Title = t.Title;
                    ex.Body = t.Body;
                }
            }

            await _uow.SaveHotelChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private async Task<List<PageTranslationVm>> BuildLangTabs(CancellationToken ct)
        {
            var langs = await _langRepo.DataSet
                .Where(x => !x.IsDeleted && x.IsActive)
                .OrderByDescending(x => x.IsDefault).ThenBy(x => x.Id)
                .Select(x => new { x.Id, x.Code }).ToListAsync(ct);

            return langs.Select(l => new PageTranslationVm
            {
                LanguageId = l.Id,
                LanguageCode = l.Code
            }).ToList();
        }
    }
}
