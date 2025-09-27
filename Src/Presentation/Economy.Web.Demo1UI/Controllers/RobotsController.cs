using Microsoft.AspNetCore.Mvc;
using MyHotelSite.Repositories;
using MyHotelSite.Services;

public class RobotsController : Controller
{
    private readonly ISiteConfigAccessor _tech;
    public RobotsController(ISiteConfigAccessor tech) { _tech = tech; }

    [HttpGet("robots.txt")]
    public async Task<IActionResult> Robots()
    {
        var tech = await _tech.GetAsync("tr");
        var host = $"{Request.Scheme}://{Request.Host}";
        var disallow = tech.Technical?.MaintenanceModeEnabled == true ? "Disallow: /" : "Disallow:";
        var body = $"User-agent: *\n{disallow}\n\nSitemap: {host}/sitemap.xml\n";
        return Content(body, "text/plain");
    }
}
