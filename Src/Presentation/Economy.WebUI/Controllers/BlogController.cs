using Economy.Web.UI.Services.Abstractions;
using Microsoft.AspNetCore.Mvc;

namespace Economy.Web.UI.Controllers
{
    public class BlogController : Controller
    {
        private readonly IBlogService _svc;
        public BlogController(IBlogService svc) => _svc = svc;

        public async Task<IActionResult> Index(string? tag, int page = 1)
        {
            var c = Thread.CurrentThread.CurrentUICulture.Name;
            var (total, items) = await _svc.ListAsync(c, tag, page, 10);
            ViewBag.Total = total; ViewBag.Tag = tag; ViewBag.Page = page;
            return View(items);
        }

        [Route("{culture?}/blog/{slug}")]
        public async Task<IActionResult> Detail(string slug)
        {
            var c = Thread.CurrentThread.CurrentUICulture.Name;
            var p = await _svc.GetAsync(c, slug);
            if (p is null) return NotFound();
            ViewData["Title"] = p.Title;
            ViewData["OgDescription"] = p.Excerpt;
            return View(p);
        }
    }

}
