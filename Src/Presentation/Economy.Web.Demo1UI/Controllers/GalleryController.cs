using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;
using MyHotelSite.Repositories;
using MyHotelSite.Services;
using MyHotelSite.Models;

namespace MyHotelSite.Controllers;

[OutputCache(PolicyName = "LangAnon300")]
public class GalleryController : Controller
{
    private readonly IGalleryRepository _repo;
    private readonly ISeoHelper _seo;

    public GalleryController(IGalleryRepository repo, ISeoHelper seo) { _repo = repo; _seo = seo; }

    [Route("{lang}/gallery")]
    public async Task<IActionResult> List(string lang = "tr")
    {
        var appId = 1;
        var items = await _repo.ListAsync(appId, lang);
        var seed = new SeoSeed { Title = "Galeri", Description = "Tesisimizden kareler", OgType = "website" };
        ViewData["Seo"] = await _seo.BuildAsync(seed, "Gallery", "List", null, lang);
        return View(items);
    }

    [Route("{lang}/gallery/{slug}")]
    public async Task<IActionResult> Details(string lang, string slug)
    {
        var appId = 1;
        var it = await _repo.GetBySlugAsync(appId, lang, slug);
        if (it == null) return NotFound();

        var origin = $"{Request.Scheme}://{Request.Host}";
        var jsonLd = $$"""
        {
          "@context":"https://schema.org",
          "@type":"ImageObject",
          "contentUrl":"{{origin}}{{it.Url}}",
          "name": {{System.Text.Json.JsonSerializer.Serialize(it.Caption ?? "Gallery")}},
          "caption": {{System.Text.Json.JsonSerializer.Serialize(it.Caption ?? "")}},
          "representativeOfPage": true
        }
        """;

        var seed = new SeoSeed
        {
            Title = it.Caption ?? "Galeri",
            Description = it.Caption ?? "Galeri görseli",
            ShareImage = it.Url,
            OgType = "website",
            JsonLd = jsonLd
        };

        ViewData["Seo"] = await _seo.BuildAsync(seed, "Gallery", "Details", new { slug }, lang);
        return View(it);
    }
}
