using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;
using MyHotelSite.Repositories;
using MyHotelSite.Services;
using MyHotelSite.Models;

namespace MyHotelSite.Controllers;

[OutputCache(PolicyName = "LangAnon300")]
public class CampaignController : Controller
{
    private readonly ICampaignRepository _repo;
    private readonly ISeoHelper _seo;
    private readonly IBreadcrumbService _bc;

    public CampaignController(ICampaignRepository repo, ISeoHelper seo, IBreadcrumbService bc) { _repo = repo; _seo = seo; _bc = bc; }

    [Route("{lang}/campaigns")]
    public async Task<IActionResult> List(string lang = "tr")
    {
        var appId = 1;
        var list = await _repo.ListAsync(appId, lang);
        var seed = new SeoSeed { Title = lang == "tr" ? "Kampanyalar" : "Campaigns", Description = "Güncel teklifler" };
        ViewData["Seo"] = await _seo.BuildAsync(appId, seed, "Campaign", "List", null, lang);
        return View(list);
    }

    [Route("{lang}/campaign/{slug}")]
    public async Task<IActionResult> Details(string lang, string slug)
    {
        var appId = 1;
        var c = await _repo.GetBySlugAsync(appId, lang, slug);
        if (c == null) return NotFound();

        var origin = $"{Request.Scheme}://{Request.Host}";
        var url = $"{origin}/{lang}/campaign/{slug}";

        var offer = $$"""
        {
          "@context":"https://schema.org",
          "@type":"Offer",
          "name":"{{c.Title}}",
          "url":"{{url}}",
          "priceCurrency":"TRY"
        }
        """;

        var bc = _bc.BuildJsonLd(
            new BreadcrumbItem(lang == "tr" ? "Ana Sayfa" : "Home", $"{origin}/{lang}"),
            new BreadcrumbItem(lang == "tr" ? "Kampanyalar" : "Campaigns", $"{origin}/{lang}/campaigns"),
            new BreadcrumbItem(c.Title, url)
        );

        var seed = new SeoSeed
        {
            Title = c.Title,
            Description = c.ShortDescription ?? "",
            ShareImage = c.HeroImage,
            OgType = "product",
            JsonLd = _bc.CombineJsonLd(offer, bc)
        };
        ViewData["Seo"] = await _seo.BuildAsync(appId, seed, "Campaign", "Details", new { slug }, lang);
        return View(c);
    }
}
