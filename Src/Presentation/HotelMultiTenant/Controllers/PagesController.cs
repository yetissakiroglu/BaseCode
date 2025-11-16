using Economy.UI.Dtos;
using HotelMultiTenant.Multitenancy;
using HotelMultiTenant.Services;
using Microsoft.AspNetCore.Mvc;

namespace HotelMultiTenant.Controllers
{
    public class PagesController : BaseController
    {
        private readonly IContentService _contentService;
        private readonly ISeoHelper _seo;
        public PagesController(IContentService contentService, ISeoHelper seo)
        {
            _contentService = contentService;
            _seo = seo;
        }


        [HttpGet]
        public async Task<IActionResult> DefaultAsync()
        {
            var lang = (string?)HttpContext.Items["Lang"];

            if(lang is null)
            {
                var t = HttpContext.GetTenant()?.Current;
                lang = t.DefaultLanguage;
            }

            var homePage = await _contentService.GetHomeAsync(lang);
            var seed = new SeoSeed
            {
                Title = homePage.MetaTitle ?? "Site",
                Description = homePage?.MetaDescription ?? "",
                ShareImage = homePage?.OgImageUrl,
                OgType = "website"
            };

            var origin = $"{Request.Scheme}://{Request.Host}";
            ViewData["Seo"] = await _seo.BuildAsync(homePage.Hreflangs, seed, currentLang: lang, xDefaultUrl: origin);

            return View("Default", homePage);
        }

        // /tr  -> Anasayfa
        [HttpGet]
        public async Task<IActionResult> AnasayfaAsync(string lang)
        {
            var homePage = await _contentService.GetHomeAsync(lang);
            var seed = new SeoSeed
            {
                Title = homePage.MetaTitle ?? "Site",
                Description = homePage?.MetaDescription ?? "",
                ShareImage = homePage?.OgImageUrl,
                OgType = "website"
            };

            var origin = $"{Request.Scheme}://{Request.Host}";
            ViewData["Seo"] = await _seo.BuildAsync(homePage.Hreflangs, seed, currentLang: lang, xDefaultUrl: origin);

            return View("Default", homePage);
        }

        // /tr/odalarimiz -> Index (lang ve slug dolu gelmeli)
        [HttpGet]
        public async Task<IActionResult> IndexAsync(string lang, string slug)
        {
            var homePage = await _contentService.GetPageAsync(lang,slug);
            var seed = new SeoSeed
            {
                Title = homePage.MetaTitle ?? "Site",
                Description = homePage?.MetaDescription ?? "",
                ShareImage = homePage?.OgImageUrl,
                OgType = "website"
            };

            var origin = $"{Request.Scheme}://{Request.Host}";
            ViewData["Seo"] = await _seo.BuildAsync(homePage.Hreflangs, seed, currentLang: lang, xDefaultUrl: origin);

            return View("Default", homePage);
        }

        [HttpGet]
        public async Task<IActionResult> IndexParentAsync(string lang, string parentSlug, string slug)
        {

            return View("Default");
        }
    }
}
