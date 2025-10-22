using Economy.Application.TenantUI.Dtos;
using Economy.Core.Enums;
using Economy.Core.Interfaces;
using Economy.Domain.Entites.TenantEntity.EntityAppBlocks;
using Economy.Domain.Entites.TenantEntity.EntityAppLanguages;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Economy.Panel.UI.Areas.Tenant.Controllers
{
    [Area("Tenant")]
    [Authorize]
    public class BlockGroupBlocksController : Controller
    {
        private readonly IUnitOfWork _uow;
        private readonly IEntityRepository<BlockGroup, int> _page;
        private readonly IEntityRepository<BlockGroupBlock, int> _apppageblock;
        private readonly IEntityRepository<AppLanguage, int> _langRepo;
        public BlockGroupBlocksController(IUnitOfWork uow)
        {
            _uow = uow;
            _page = uow.HotelEntityRepository<BlockGroup>();
            _apppageblock = uow.HotelEntityRepository<BlockGroupBlock>();
            _langRepo = uow.HotelEntityRepository<AppLanguage>();
        }
        public async Task<IActionResult> Index(int? pageId, BlockType? type, string? q, int page = 1, int size = 20)
        {
            var query = _apppageblock.DataSet.Include(pb => pb.BlockGroup).AsQueryable();
            if (pageId.HasValue) query = query.Where(x => x.BlockGroupId == pageId.Value);
            if (type.HasValue) query = query.Where(x => x.Type == type.Value);
            if (!string.IsNullOrWhiteSpace(q)) query = query.Where(x => x.SharedJson.Contains(q));

            var total = await query.CountAsync();
            var items = await query.OrderBy(x => x.BlockGroupId).ThenBy(x => x.SortOrder)
                .Skip((page - 1) * size).Take(size)
                .Select(x => new PageBlockListItemVm
                {
                    Id = x.Id,
                    BlockGroupId = x.BlockGroupId,
                    Type = x.Type,
                    SortOrder = x.SortOrder,
                    IsActive = x.IsActive,
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
            var b = await _apppageblock.DataSet.Include(x => x.Translations).Include(x => x.BlockGroup).FirstOrDefaultAsync(x => x.Id == id);
            if (b == null) return NotFound();

            var langs = await _langRepo.DataSet.Where(x => x.IsActive).ToListAsync();
            ViewBag.Languages = langs;
            ViewBag.PageInfo = new { PageId = b.BlockGroupId };

            var vm = new BlockGroupBlockVm
            {
                Id = b.Id,
                Type = b.Type,
                SortOrder = b.SortOrder,
                IsActive = b.IsActive,
                Stage = b.Stage,
                SharedJson = b.SharedJson,
                Translations = langs.Select(l => {
                    var bt = b.Translations.FirstOrDefault(t => t.AppLanguageId == l.Id);
                    return new BlockGroupBlockTranslationVm { Id = bt?.Id, LanguageId = l.Id, LocalizedJson = bt?.LocalizedJson ?? "{}" };
                }).ToList()
            };
            return View(vm);
        }

        [HttpPost("edit/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, BlockGroupBlockVm vm)
        {
            var b = await _apppageblock.DataSet.Include(x => x.Translations).FirstOrDefaultAsync(x => x.Id == id);
            if (b == null) return NotFound();

            b.Type = vm.Type; b.SortOrder = vm.SortOrder; b.IsActive = vm.IsActive; b.Stage = vm.Stage;
            b.SharedJson = vm.SharedJson;

            var langs = await _langRepo.DataSet.Where(x => x.IsActive).ToListAsync();
            foreach (var l in langs)
            {
                var incoming = vm.Translations.First(t => t.LanguageId == l.Id);
                var cur = b.Translations.FirstOrDefault(t => t.AppLanguageId == l.Id);
                if (cur == null) b.Translations.Add(new BlockGroupBlockTranslation { AppLanguageId = l.Id, LocalizedJson = incoming.LocalizedJson });
                else cur.LocalizedJson = incoming.LocalizedJson;
            }
            await _uow.SaveHotelChangesAsync();
            TempData["ok"] = "Blok kaydedildi";
            return RedirectToAction(nameof(Edit), new { id });
        }

    }


}
