using Economy.Core.Core;
using Economy.Core.Options;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Economy.Panel.UI.Areas.Tenant.Controllers
{
    [Area("Tenant")]
    [Authorize] // demo: sadece login gerekli; istersen Roles="Admin"
    public class ImageManagerController : Controller
    {
        private readonly IImageStorage _storage;
        private readonly FileManagerOptions _opt;

        public ImageManagerController(IImageStorage storage, Microsoft.Extensions.Options.IOptions<FileManagerOptions> opt)
        {
            _storage = storage;
            _opt = opt.Value;
        }

        public async Task<IActionResult> Index(string? dir, string? q)
        {
            dir ??= string.Empty;
            var items = await _storage.ListAsync(dir);
            var dirs = await _storage.ListDirsAsync(dir);

            if (!string.IsNullOrWhiteSpace(q))
                items = items.Where(x => x.Name.Contains(q, StringComparison.OrdinalIgnoreCase)).ToList();

            ViewBag.CurrentDir = dir;
            ViewBag.Dirs = dirs;
            ViewBag.Breadcrumb = BuildCrumbs(dir);
            ViewBag.Query = q ?? string.Empty;
            ViewBag.PublicBase = _opt.PublicRequestPath;
            return View(items);
        }

        public async Task<IActionResult> Picker(string? dir, string? q)
        {
            dir ??= string.Empty;
            var items = await _storage.ListAsync(dir);
            var dirs = await _storage.ListDirsAsync(dir);

            if (!string.IsNullOrWhiteSpace(q))
                items = items.Where(x => x.Name.Contains(q, StringComparison.OrdinalIgnoreCase)).ToList();

            ViewBag.CurrentDir = dir;
            ViewBag.Dirs = dirs;
            ViewBag.Breadcrumb = BuildCrumbs(dir);
            ViewBag.Query = q ?? string.Empty;
            return View(items);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UploadAjax(string? dir, List<IFormFile> files, string? aspect = null)
        {
            if (files == null || files.Count == 0)
                return BadRequest(new { ok = false, error = "Dosya yok." });

            var rels = new List<string>();
            var urls = new List<string>();
            foreach (var f in files)
            {
                var rel = await _storage.UploadAsync(dir ?? string.Empty, f, aspect);
                rels.Add(rel);
                var webp = _storage.ToPublicWebpIfExists(rel);
                urls.Add(webp ?? _storage.ToPublicUrl(rel));
            }
            return Ok(new { ok = true, rels, urls });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(string path, string? dir)
        {
            await _storage.DeleteAsync(path);
            return RedirectToAction(nameof(Index), new { dir });
        }

        private static List<(string label, string? path)> BuildCrumbs(string dir)
        {
            var list = new List<(string, string?)> { ("Root", "") };
            if (string.IsNullOrWhiteSpace(dir)) return list;
            var parts = dir.Replace('\\', '/').Split('/', StringSplitOptions.RemoveEmptyEntries);
            var acc = "";
            foreach (var p in parts)
            {
                acc = string.IsNullOrEmpty(acc) ? p : $"{acc}/{p}";
                list.Add((p, acc));
            }
            return list;
        }
    }


}
