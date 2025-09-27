using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;
using MyHotelSite.Repositories;
using MyHotelSite.Services;
using MyHotelSite.Models;

namespace MyHotelSite.Controllers;

[OutputCache(PolicyName = "LangAnon300")]
public class RoomController : Controller
{
    private readonly IRoomRepository _rooms;
    private readonly ISeoHelper _seo;
    private readonly IBreadcrumbService _bc;

    public RoomController(IRoomRepository rooms, ISeoHelper seo, IBreadcrumbService bc) { _rooms = rooms; _seo = seo; _bc = bc; }

    [Route("{lang}/rooms")]
    public async Task<IActionResult> List(string lang = "tr")
    {
        var appId = 1;
        var list = await _rooms.ListAsync(appId, lang);
        var seed = new SeoSeed { Title = lang == "tr" ? "Odalar" : "Rooms", Description = "Konforlu odalarımız" };
        ViewData["Seo"] = await _seo.BuildAsync(seed, "Room", "List", null, lang);
        return View(list);
    }

    [Route("{lang}/room/{slug}")]
    public async Task<IActionResult> Details(string lang, string slug)
    {
        var appId = 1;
        var r = await _rooms.GetBySlugAsync(appId, lang, slug);
        if (r == null) return NotFound();

        var origin = $"{Request.Scheme}://{Request.Host}";
        var url = $"{origin}/{lang}/room/{slug}";

        var jsonLd = $$"""
        {
          "@context": "https://schema.org",
          "@type": "HotelRoom",
          "name": "{{r.RoomName}}",
          "description": {{System.Text.Json.JsonSerializer.Serialize(r.ShortDescription ?? "")}},
          "image": {{System.Text.Json.JsonSerializer.Serialize(r.MainImageUrl ?? "")}},
          "containedInPlace": { "@type": "Hotel", "name": {{System.Text.Json.JsonSerializer.Serialize(r.HotelName)}} },
          "offers": {
            "@type": "Offer",
            "url": "{{url}}",
            "price": {{(r.Price.HasValue ? r.Price.Value.ToString(System.Globalization.CultureInfo.InvariantCulture) : "null")}},
            "priceCurrency": "TRY",
            "availability": "{{(r.IsAvailable ? "https://schema.org/InStock" : "https://schema.org/OutOfStock")}}"
          }
        }
        """;

        var bc = _bc.BuildJsonLd(
            new BreadcrumbItem(lang == "tr" ? "Ana Sayfa" : "Home", $"{origin}/{lang}"),
            new BreadcrumbItem(lang == "tr" ? "Odalar" : "Rooms", $"{origin}/{lang}/rooms"),
            new BreadcrumbItem(r.RoomName, url)
        );

        var seed = new SeoSeed
        {
            Title = r.RoomName,
            Description = r.ShortDescription ?? "",
            ShareImage = r.MainImageUrl,
            OgType = "product",
            JsonLd = _bc.CombineJsonLd(jsonLd, bc)
        };
        ViewData["Seo"] = await _seo.BuildAsync( seed, "Room", "Details", new { slug }, lang);
        return View(r);
    }
}
