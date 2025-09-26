using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;
using Microsoft.AspNetCore.RateLimiting;
using MyHotelSite.Models;
using MyHotelSite.Services;

namespace MyHotelSite.Controllers;

[OutputCache(PolicyName = "LangAnon300")]
public class ContactController : Controller
{
    private readonly ISiteConfigAccessor _cfg;
    private readonly ISeoHelper _seo;
    public ContactController(ISiteConfigAccessor cfg, ISeoHelper seo) { _cfg = cfg; _seo = seo; }

    [Route("{lang}/contact")]
    public async Task<IActionResult> Index(string lang = "tr")
    {
        var appId = 1;
        var cfg = await _cfg.GetAsync(appId);
        var seed = new SeoSeed { Title = lang == "tr" ? "İletişim" : "Contact", Description = lang == "tr" ? "Bize ulaşın" : "Get in touch" };
        ViewData["Seo"] = await _seo.BuildAsync(appId, seed, "Contact", "Index", null, lang);
        return View(cfg);
    }

    [EnableRateLimiting("contact-post")]
    [ValidateAntiForgeryToken]
    [HttpPost("{lang}/Contact/Send")]
    public IActionResult Send(string lang, [FromForm] ContactPostModel m)
    {
        if (!ModelState.IsValid) return BadRequest();
        return Redirect($"/{lang}/subscribe/thanks");
    }

    public record ContactPostModel(string Name, string Email, string? Phone, string Message);

}
