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
    public sealed class BlocksController : Controller
    {
        private readonly IUnitOfWork _uow;
        private readonly IEntityRepository<ContentItem, int> _contentRepo;
        private readonly IEntityRepository<ContentItemTranslation, int> _trRepo;
        private readonly IEntityRepository<AppLanguage, int> _langRepo;

        public BlocksController(IUnitOfWork uow)
        {
            _uow = uow;
            _contentRepo = uow.HotelEntityRepository<ContentItem>();
            _trRepo = uow.HotelEntityRepository<ContentItemTranslation>();
            _langRepo = uow.HotelEntityRepository<AppLanguage>();
        }

        public async Task<IActionResult> Index(int pageId, CancellationToken ct)
        {
            var defLangId = await _langRepo.DataSet.Where(l => !l.IsDeleted && l.IsActive && l.IsDefault)
                .Select(l => l.Id).FirstOrDefaultAsync(ct);

            var list = await (from b in _contentRepo.DataSet
                              where !b.IsDeleted && b.IsActive
                                    && b.Type == ContentItemType.Block
                                    && b.OwnerId == pageId
                              join tr in _trRepo.DataSet on b.Id equals tr.ContentItemId into trx
                              from tr in trx.Where(t => !t.IsDeleted && t.LanguageId == defLangId).DefaultIfEmpty()
                              orderby b.SortOrder, b.Id
                              select new BlockListItemVm
                              {
                                  Id = b.Id,
                                  Template = b.BlockTemplate.HasValue ? b.BlockTemplate.Value.ToString() : "Custom",
                                  SortOrder = b.SortOrder,
                                  Title = tr.Title
                              })
                              .ToListAsync(ct);

            ViewBag.PageId = pageId;
            return View(list);
        }

        [HttpGet]
        public async Task<IActionResult> Create(int pageId, CancellationToken ct)
        {
            var def = await _langRepo.DataSet.Where(l => !l.IsDeleted && l.IsActive && l.IsDefault)
                .Select(l => new { l.Id, l.Code }).FirstOrDefaultAsync(ct);

            var vm = new BlockEditVm
            {
                PageId = pageId,
                LanguageId = def!.Id,
                LanguageCode = def.Code,
                SortOrder = 0,
                BlockTemplate = BlockTemplate.RichText
            };
            return View(vm);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(BlockEditVm vm, CancellationToken ct)
        {
            if (!ModelState.IsValid) return View(vm);

            var b = new ContentItem
            {
                IsDeleted = false,
                IsActive = vm.IsActive,
                SortOrder = vm.SortOrder,
                Type = ContentItemType.Block,
                OwnerId = vm.PageId,
                BlockTemplate = vm.BlockTemplate,
                Image = vm.Image,
                OgImage = vm.OgImage,
                JsonData = vm.JsonData,
            };
            await _contentRepo.DataSet.AddAsync(b, ct);
            await _uow.SaveHotelChangesAsync();

            var tr = new ContentItemTranslation
            {
                ContentItemId = b.Id,
                LanguageId = vm.LanguageId,
                IsDeleted = false,
                Title = vm.Title,
                Summary = vm.Summary,
                Body = vm.Body,
                ButtonText = vm.ButtonText,
                ButtonUrl = vm.ButtonUrl,
            };
            await _trRepo.DataSet.AddAsync(tr, ct);
            await _uow.SaveHotelChangesAsync();

            return RedirectToAction(nameof(Index), new { pageId = vm.PageId });
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id, int pageId, CancellationToken ct)
        {
            var b = await _contentRepo.DataSet.FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted, ct);
            if (b is null) return NotFound();

            var def = await _langRepo.DataSet.Where(l => !l.IsDeleted && l.IsActive && l.IsDefault)
                .Select(l => new { l.Id, l.Code }).FirstOrDefaultAsync(ct);

            var tr = await _trRepo.DataSet
                .Where(t => !t.IsDeleted && t.ContentItemId == id && t.LanguageId == def!.Id)
                .FirstOrDefaultAsync(ct);

            var vm = new BlockEditVm
            {
                Id = b.Id,
                PageId = pageId,
                SortOrder = b.SortOrder,
                IsActive = b.IsActive,
                BlockTemplate = b.BlockTemplate ?? BlockTemplate.RichText,
                LanguageId = def!.Id,
                LanguageCode = def.Code,
                Title = tr?.Title,
                Summary = tr?.Summary,
                Body = tr?.Body,
                Image = b?.Image,
                ButtonText = tr?.ButtonText,
                ButtonUrl = tr?.ButtonUrl,
                JsonData = b?.JsonData
            };
            return View(vm);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, BlockEditVm vm, CancellationToken ct)
        {
            var b = await _contentRepo.DataSet.FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted, ct);
            if (b is null) return NotFound();

            b.SortOrder = vm.SortOrder;
            b.IsActive = vm.IsActive;
            b.BlockTemplate = vm.BlockTemplate;
            b.Image = vm.Image;
            b.OgImage = vm.OgImage;
            b.JsonData = vm.JsonData;

            var tr = await _trRepo.DataSet
                .FirstOrDefaultAsync(t => !t.IsDeleted && t.ContentItemId == id && t.LanguageId == vm.LanguageId, ct);

            if (tr is null)
            {
                tr = new ContentItemTranslation
                {
                    ContentItemId = id,
                    LanguageId = vm.LanguageId,
                    IsDeleted = false
                };
                await _trRepo.DataSet.AddAsync(tr, ct);
            }

            tr.Title = vm.Title;
            tr.Summary = vm.Summary;
            tr.Body = vm.Body;
            tr.ButtonText = vm.ButtonText;
            tr.ButtonUrl = vm.ButtonUrl;

            await _uow.SaveHotelChangesAsync();
            return RedirectToAction(nameof(Index), new { pageId = vm.PageId });
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id, int pageId, CancellationToken ct)
        {
            var b = await _contentRepo.DataSet.FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted, ct);
            if (b is null) return NotFound();
            b.IsDeleted = true;
            await _uow.SaveHotelChangesAsync();
            return RedirectToAction(nameof(Index), new { pageId });
        }
    }
}
