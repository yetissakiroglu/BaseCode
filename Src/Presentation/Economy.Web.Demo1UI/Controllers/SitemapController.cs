using Microsoft.AspNetCore.Mvc;
using System.Text;
using Microsoft.AspNetCore.OutputCaching;
using MyHotelSite.Repositories;

namespace MyHotelSite.Controllers;

public class SitemapController : Controller
{
    private readonly ILanguageService _langs;
    private readonly IRoomRepository _rooms;
    private readonly ICampaignRepository _campaigns;
    private readonly IGalleryRepository _gallery;

    public SitemapController(ILanguageService langs, IRoomRepository rooms, ICampaignRepository campaigns, IGalleryRepository gallery)
    { _langs = langs; _rooms = rooms; _campaigns = campaigns; _gallery = gallery; }

    [Route("sitemap.xml")]
    [ResponseCache(Duration = 300, Location = ResponseCacheLocation.Any, NoStore = false)]
    public async Task<IActionResult> IndexXml()
    {
        var appId = 1;
        var origin = $"{Request.Scheme}://{Request.Host}";
        var langs = await _langs.GetAllAsync(appId);

        var sb = new StringBuilder();
        sb.AppendLine(@"<?xml version=""1.0"" encoding=""UTF-8""?>");
        sb.AppendLine(@"<sitemapindex xmlns=""http://www.sitemaps.org/schemas/sitemap/0.9"">");
        foreach (var l in langs)
            sb.AppendLine($"  <sitemap><loc>{origin}/sitemap-{l.Code}.xml</loc></sitemap>");
        sb.AppendLine("</sitemapindex>");
        return Content(sb.ToString(), "application/xml", Encoding.UTF8);
    }

    [Route("sitemap-{lang}.xml")]
    [ResponseCache(Duration = 300, Location = ResponseCacheLocation.Any, NoStore = false)]
    public async Task<IActionResult> LangXml(string lang)
    {
        var appId = 1;
        var origin = $"{Request.Scheme}://{Request.Host}";
        var now = DateTime.UtcNow.ToString("yyyy-MM-dd");

        var rooms = await _rooms.ListAsync(appId, lang);
        var camps = await _campaigns.ListAsync(appId, lang);
        var gal = await _gallery.ListAsync(appId, lang);

        var urls = new List<(string loc, string lastmod)>
        {
            ($"{origin}/{lang}", now),
            ($"{origin}/{lang}/rooms", now),
            ($"{origin}/{lang}/campaigns", now),
            ($"{origin}/{lang}/gallery", now),
            ($"{origin}/{lang}/contact", now),
        };
        urls.AddRange(rooms.Select(r => ($"{origin}/{lang}/room/{r.Slug}", now)));
        urls.AddRange(camps.Select(c => ($"{origin}/{lang}/campaign/{c.Slug}", now)));
        urls.AddRange(gal.Select(g => ($"{origin}/{lang}/gallery/{g.Slug}", now)));

        var sb = new StringBuilder();
        sb.AppendLine(@"<?xml version=""1.0"" encoding=""UTF-8""?>");
        sb.AppendLine(@"<urlset xmlns=""http://www.sitemaps.org/schemas/sitemap/0.9"">");
        foreach (var u in urls.DistinctBy(x => x.loc))
        {
            sb.AppendLine("  <url>");
            sb.AppendLine($"    <loc>{u.loc}</loc>");
            sb.AppendLine($"    <lastmod>{u.lastmod}</lastmod>");
            sb.AppendLine("  </url>");
        }
        sb.AppendLine("</urlset>");
        return Content(sb.ToString(), "application/xml", Encoding.UTF8);
    }
}
