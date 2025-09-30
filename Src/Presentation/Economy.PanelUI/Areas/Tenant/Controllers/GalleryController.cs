using Economy.Core.Enums;
using Economy.Core.Interfaces;
using Economy.Domain.Entites.EntityAppLanguage;
using Economy.Domain.Entites.EntityAppNewPages;
using Economy.Panel.UI.Areas.Tenant.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Economy.Panel.UI.Areas.Tenant.Controllers
{
    [Area("Tenant")]
    [Authorize]
    public sealed class GalleryController : Controller
    {
        private readonly IUnitOfWork _uow;
        private readonly IEntityRepository<ContentMedia, int> _mediaRepo;
        private readonly IEntityRepository<ContentMediaTranslation, int> _mediaTrRepo;
        private readonly IEntityRepository<AppLanguage, int> _langRepo;

        public GalleryController(IUnitOfWork uow)
        {
            _uow = uow;
            _mediaRepo = uow.HotelEntityRepository<ContentMedia>();
            _mediaTrRepo = uow.HotelEntityRepository<ContentMediaTranslation>();
            _langRepo = uow.HotelEntityRepository<AppLanguage>();
        }

        public async Task<IActionResult> Index(int pageId, CancellationToken ct)
        {
            var def = await _langRepo.DataSet.Where(l => !l.IsDeleted && l.IsActive && l.IsDefault)
                .Select(l => l.Id).FirstOrDefaultAsync(ct);

            var list = await (from m in _mediaRepo.DataSet
                              where !m.IsDeleted && m.IsActive
                                    && m.OwnerType == MediaOwnerType.Content
                                    && m.OwnerId == pageId
                              orderby m.SortOrder, m.Id
                              select new MediaListItemVm
                              {
                                  Id = m.Id,
                                  Url = m.Url,
                                  SortOrder = m.SortOrder,
                                  Alt = _mediaTrRepo.DataSet
                                       .Where(t => !t.IsDeleted && t.IsActive && t.ContentMediaId == m.Id && t.LanguageId == def)
                                       .Select(t => t.Alt).FirstOrDefault(),
                                  Caption = _mediaTrRepo.DataSet
                                       .Where(t => !t.IsDeleted && t.IsActive && t.ContentMediaId == m.Id && t.LanguageId == def)
                                       .Select(t => t.Caption).FirstOrDefault()
                              }).ToListAsync(ct);

            ViewBag.PageId = pageId;
            return View(list);
        }

        [HttpGet]
        public async Task<IActionResult> Create(int pageId, CancellationToken ct)
        {
            var def = await _langRepo.DataSet.Where(l => !l.IsDeleted && l.IsActive && l.IsDefault)
                .Select(l => new { l.Id, l.Code }).FirstOrDefaultAsync(ct);

            var vm = new MediaEditVm { PageId = pageId, LanguageId = def!.Id, LanguageCode = def.Code, SortOrder = 0 };
            return View(vm);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(MediaEditVm vm, CancellationToken ct)
        {
            if (!ModelState.IsValid) return View(vm);

            var m = new ContentMedia
            {
                OwnerType = MediaOwnerType.Content,
                OwnerId = vm.PageId,
                Url = vm.Url,
                SortOrder = vm.SortOrder,
                IsActive = vm.IsActive,
                IsDeleted = false
            };
            await _mediaRepo.DataSet.AddAsync(m, ct);
            await _uow.SaveHotelChangesAsync();

            var tr = new ContentMediaTranslation
            {
                ContentMediaId = m.Id,
                LanguageId = vm.LanguageId,
                Alt = vm.Alt,
                Caption = vm.Caption,
                IsActive = true,
                IsDeleted = false
            };
            await _mediaTrRepo.DataSet.AddAsync(tr, ct);
            await _uow.SaveHotelChangesAsync();

            return RedirectToAction(nameof(Index), new { pageId = vm.PageId });
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id, int pageId, CancellationToken ct)
        {
            var m = await _mediaRepo.DataSet.FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted, ct);
            if (m is null) return NotFound();
            m.IsDeleted = true;
            await _uow.SaveHotelChangesAsync();
            return RedirectToAction(nameof(Index), new { pageId });
        }
    }
}
