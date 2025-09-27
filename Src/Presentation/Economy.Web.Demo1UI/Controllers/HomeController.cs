using Microsoft.AspNetCore.Mvc;
using MyHotelSite.Models;
using MyHotelSite.Repositories;
using MyHotelSite.Services;

namespace MyHotelSite.Controllers;

public class HomeController : Controller
{
    private readonly ISiteConfigAccessor _cfg;
    private readonly IRoomRepository _rooms;
    private readonly ICampaignRepository _campaigns;
    private readonly IGalleryRepository _gallery;
    private readonly ISeoHelper _seo;

    public HomeController(ISiteConfigAccessor cfg, IRoomRepository rooms, ICampaignRepository campaigns, IGalleryRepository gallery, ISeoHelper seo)
    { _cfg = cfg; _rooms = rooms; _campaigns = campaigns; _gallery = gallery; _seo = seo; }

    public async Task<IActionResult> Index(string lang = "tr")
    {
        var appId = 1;
        var setting = await _cfg.GetAsync(lang);
        var rooms = await _rooms.ListAsync(appId, lang);
        var camps = await _campaigns.ListAsync(appId, lang);
        var gal = await _gallery.ListAsync(appId, lang);

        var seed = new SeoSeed
        {
            Title = setting.Setting?.SiteTitle ?? "Site",
            Description = setting.Setting?.Description ?? "",
            ShareImage = setting.Setting?.ShareImagePath,
            OgType = "website"
        };

        var origin = $"{Request.Scheme}://{Request.Host}";
        ViewData["Seo"] = await _seo.BuildAsync(seed, controller: "Home", action: "Index", currentLang: lang, xDefaultUrl: origin);

        var vm = new HomeViewModel
        {
            Rooms = rooms.Take(6).ToList(),
            Campaigns = camps.Take(6).ToList(),
            Gallery = gal.Take(5).ToList(),
            SliderVideoUrl = null, // ör: "/videos/hero.mp4"
            SliderImages = new() { "/images/hero/hero1.jpg", "/images/hero/hero2.jpg", "/images/hero/hero3.jpg" }
        };
        return View(vm);
    }
}
