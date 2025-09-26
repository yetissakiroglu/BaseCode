using Microsoft.AspNetCore.Mvc;
using MyHotelSite.Repositories;

public class RobotsController : Controller
{
    private readonly IAppSettingTechnicalRepository _tech;
    public RobotsController(IAppSettingTechnicalRepository tech) { _tech = tech; }

    [HttpGet("robots.txt")]
    public async Task<IActionResult> Robots()
    {
        var tech = await _tech.GetAsync(1);
        var host = $"{Request.Scheme}://{Request.Host}";
        var disallow = tech?.MaintenanceModeEnabled == true ? "Disallow: /" : "Disallow:";
        var body = $"User-agent: *\n{disallow}\n\nSitemap: {host}/sitemap.xml\n";
        return Content(body, "text/plain");
    }
}
