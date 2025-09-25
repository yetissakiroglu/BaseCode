using Economy.Application.Dtos.AppMenuDtos;
using Economy.Application.Interfaces;
using Economy.Core.Enums;
using Economy.Domain.Entites.EntityMenuItems;
using Economy.Panel.Application.Interfaces;
using Economy.Panel.UI.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;

namespace Economy.Panel.UI.Areas.Tenant.Controllers
{

    [Area("Tenant")]
    [Authorize]
    public class MenusController : BaseController
    {
        private readonly IPanelAppMenuService _svc;
        private readonly IPanelAppLanguageService _panelAppLanguageService;

        public MenusController(IPanelAppMenuService svc, IPanelAppLanguageService panelAppLanguageService)
        {
            _svc = svc;
            _panelAppLanguageService = panelAppLanguageService;
        }

        [HttpGet]
        public async Task<IActionResult> Index(string location = "main")
        {
            var treeRes = await _svc.GetTreeAsync(location, onlyActive: false);
            if (!treeRes.IsSuccess && treeRes.StatusCode >= 500)
            {
                TempData["error"] = treeRes.Message;
                return View(DefaultModel(location));
            }

            var tree = treeRes.Data ?? new List<MenuNodeDto>();
            ViewBag.Tree = tree;

            // Parent dropdown için düz liste
            var flat = Flatten(tree);
            ViewBag.ParentList = flat.Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Path }).ToList();

            // Diller
            var langs = _panelAppLanguageService.GetAllLanguage(false, true);
            ViewBag.Languages = langs.Data;

            // Form için boş model (dillerle)
            var model = DefaultModel(location, langs.Data.Select(l => l.Id));
            return View(model);
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<IActionResult> Save(MenuItemDto model)
        {
            var user = User?.Identity?.Name ?? "system";
            var res = await _svc.UpsertAsync(model, user);

            if (!res.IsSuccess)
            {
                if (res.ValidationErrors is not null)
                {
                    //foreach (var kv in res.ValidationErrors)
                    //    ModelState.AddModelError(kv.Key, kv.Value);
                    //TempData["error"] = res.Message;

                    // Yeniden viewbag’leri hazırla
                    var tree = (await _svc.GetTreeAsync(model.Location, false)).Data ?? new List<MenuNodeDto>();
                    ViewBag.Tree = tree;
                    ViewBag.ParentList = Flatten(tree).Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Path }).ToList();
                    //ViewBag.Languages = await _db.AppLanguages.AsNoTracking().OrderBy(x => x.Id).ToListAsync();

                    return View("Index", model);
                }
                TempData["error"] = res.Message;
            }
            else TempData["ok"] = res.Message;

            return RedirectToAction(nameof(Index), new { location = model.Location });
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<IActionResult> Delete(int id, string location)
        {
            var res = await _svc.DeleteAsync(id);
            TempData[res.IsSuccess ? "ok" : "error"] = res.Message;
            return RedirectToAction(nameof(Index), new { location });
        }

        public class ReorderRequest { public List<MenuReorderItem> Items { get; set; } = new(); }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<IActionResult> Reorder([FromBody] ReorderRequest req)
        {
            var user = User?.Identity?.Name ?? "system";
            var res = await _svc.ReorderAsync(req.Items, user);
            if (!res.IsSuccess) return StatusCode(res.StatusCode, new { ok = false, msg = res.Message });
            return Ok(new { ok = true });
        }

        // helpers
        private static MenuItemDto DefaultModel(string location, IEnumerable<int>? langIds = null)
        {
            return new MenuItemDto(
                Id: null,
                Location: location,
                PageId: null,
                OpenTarget: MenuOpenTarget.SameTab,
                SortOrder: 0,
                IsActive: true,
                IsExternal: false,
                ParentId: null,
                Translations: (langIds ?? Array.Empty<int>()).Select(id => new MenuTranslationDto(id, "", null)).ToList()
            );
        }

        private static List<FlatMenu> Flatten(List<MenuNodeDto> roots, string prefix = "")
        {
            var list = new List<FlatMenu>();
            foreach (var r in roots.OrderBy(x => x.SortOrder))
            {
                var path = string.IsNullOrEmpty(prefix) ? r.Translations.FirstOrDefault()?.Title ?? $"#{r.Id}" : $"{prefix} > {r.Translations.FirstOrDefault()?.Title ?? $"#{r.Id}"}";
                list.Add(new FlatMenu { Id = r.Id, Path = path });
                if (r.Children?.Any() == true)
                    list.AddRange(Flatten(r.Children.ToList(), path));
            }
            return list;
        }
        private class FlatMenu { public int Id { get; set; } public string Path { get; set; } = ""; }
    }

}
