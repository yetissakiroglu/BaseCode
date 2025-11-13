using HotelMultiTenant.Services;
using Microsoft.AspNetCore.Mvc;

namespace HotelMultiTenant.Controllers
{
    public class PagesController : Controller
    {
        private readonly IContentService _contentService;
        public PagesController(IContentService contentService)
        {
            _contentService = contentService;
        }


        [HttpGet]
        public async Task<IActionResult> DefaultAsync()
        {
            var homePage = await _contentService.GetHomeAsync("tr");
            return View("Default", homePage);
        }

        // /tr  -> Anasayfa
        [HttpGet]
        public async Task<IActionResult> AnasayfaAsync(string lang)
        {
            var homePage = await _contentService.GetHomeAsync(lang);
            return View("Anasayfa", homePage);

        }

        // /tr/odalarimiz -> Index (lang ve slug dolu gelmeli)
        [HttpGet]
        public async Task<IActionResult> IndexAsync(string lang, string slug)
        {

            return View("Index");
        }

        [HttpGet]
        public async Task<IActionResult> IndexParentAsync(string lang, string parentSlug, string slug)
        {

            return View("Index");
        }
    }
}
