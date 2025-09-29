using Economy.Web.Demo1.Helpers;
using Economy.Web.Demo1.Models;
using Economy.Web.Demo1.Services;
using Microsoft.AspNetCore.Mvc;

namespace Economy.Web.Demo1.Controllers
{
    public class PagesController : Controller
    {
        private readonly ISeoHelper _seo;
        private readonly ISiteConfigAccessor _cfg;

        public PagesController(ISeoHelper seo, ISiteConfigAccessor cfg)
        {
            _seo = seo;
            _cfg = cfg;
        }

        // /tr  -> Anasayfa
        [HttpGet]
        public async Task<IActionResult> AnasayfaAsync(string lang)
        {
            var setting = await _cfg.GetAsync(lang);
            var seed = new SeoSeed
            {
                Title = setting.Setting?.SiteTitle ?? "Site",
                Description = setting.Setting?.Description ?? "",
                ShareImage = setting.Setting?.ShareImagePath,
                OgType = "website"
            };

            var origin = $"{Request.Scheme}://{Request.Host}";
            ViewData["Seo"] = await _seo.BuildAsync(seed, controller: "Home", action: "Index", currentLang: lang, xDefaultUrl: origin);

            lang = (lang ?? "tr").ToLowerInvariant();

            var vm = await _cfg.GetHomepageAsync(lang!);
            if (vm is null) return NotFound();

            // Görünüm yönlendirmesi
            if (vm.Type == "list")
                return View("List", vm);

            return View("Detail", vm);

        }

        // /tr/odalarimiz -> Index (lang ve slug dolu gelmeli)
        [HttpGet]
        public async Task<IActionResult> IndexAsync(string lang, string slug)
        {
            var vm = await _cfg.GetPageAsync(lang, slug!);
            if (vm is null) return NotFound();

            // Görünüm yönlendirmesi
            if (vm.Type == "list")
                return View("List", vm);

            return View("Detail", vm);
        }

        [HttpGet]
        public async Task<IActionResult> IndexParentAsync(string lang,string parentSlug, string slug)
        {
            var vm = await _cfg.GetPageAsync(lang, slug!);
            if (vm is null) return NotFound();

            // Görünüm yönlendirmesi
            if (vm.Type == "list")
                return View("List", vm);

            return View("Detail", vm);
        }
    }
}
