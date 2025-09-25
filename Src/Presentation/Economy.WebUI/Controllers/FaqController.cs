using Economy.Web.UI.Services.Abstractions;
using Microsoft.AspNetCore.Mvc;

namespace Economy.Web.UI.Controllers
{
    public class FaqController : Controller
    {
        private readonly IFaqService _svc;
        public FaqController(IFaqService svc) => _svc = svc;

        public async Task<IActionResult> Index(string? q)
        {
            var c = Thread.CurrentThread.CurrentUICulture.Name;
            var list = await _svc.SearchAsync(c, q);
            ViewBag.Query = q;
            return View(list);
        }
    }

}
