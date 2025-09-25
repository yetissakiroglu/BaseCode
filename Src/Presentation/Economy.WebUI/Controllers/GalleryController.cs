using Economy.Web.UI.Services.Abstractions;
using Microsoft.AspNetCore.Mvc;

namespace Economy.Web.UI.Controllers
{
    public class GalleryController : Controller
    {
        private readonly IGalleryService _svc;
        public GalleryController(IGalleryService svc) => _svc = svc;

        public async Task<IActionResult> Index(string? category, int page = 1)
        {
            var c = Thread.CurrentThread.CurrentUICulture.Name;
            ViewBag.Categories = await _svc.CategoriesAsync(c);
            var (total, items) = await _svc.ListAsync(c, category, page, 12);
            ViewBag.Total = total; ViewBag.Category = category; ViewBag.Page = page;
            return View(items);
        }
    }

}
