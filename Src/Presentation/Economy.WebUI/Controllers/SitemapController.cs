using Economy.Web.UI.Services.Abstractions;
using Microsoft.AspNetCore.Mvc;

namespace Economy.Web.UI.Controllers
{
    public class SitemapController : Controller
    {
        private readonly ILanguageService _langs;
        private readonly IContentService _content;
        private readonly IBlogService _blog;

        public SitemapController(ILanguageService l, IContentService c, IBlogService b) { _langs = l; _content = c; _blog = b; }

        [ResponseCache(Duration = 300)]
        [HttpGet("/sitemap.xml")]
        public async Task<IActionResult> Index()
        {
            var langs = await _langs.GetAsync();
            var baseUrl = "https://www.ornekotel.com".TrimEnd('/'); // appsettings'ten de alabilirsin
            var urls = new List<(string loc, DateTime? lastmod, decimal priority)>();

            foreach (var l in langs)
            {
                // anasayfa
                urls.Add(($"{baseUrl}/{l.ShortCode}", DateTime.UtcNow.AddDays(-1), 1.0m));

                // odalar listesi
                urls.Add(($"{baseUrl}/{l.ShortCode}/Rooms", DateTime.UtcNow.AddDays(-2), 0.9m));

                // odalar detay
                var rooms = await _content.GetRoomsAsync(l.Code);
                foreach (var r in rooms)
                    urls.Add(($"{baseUrl}/{l.ShortCode}/rooms/{r.Slug}", DateTime.UtcNow.AddDays(-2), 0.8m));

                // blog list
                urls.Add(($"{baseUrl}/{l.ShortCode}/Blog", DateTime.UtcNow.AddDays(-3), 0.6m));
                // blog detay
                var (total, posts) = await _blog.ListAsync(l.Code, null, 1, 50);
                foreach (var p in posts)
                    urls.Add(($"{baseUrl}/{l.ShortCode}/blog/{p.Slug}", p.UpdatedAt, 0.6m));
            }

            var xml = BuildSitemapXml(urls);
            return Content(xml, "application/xml");
        }

        private static string BuildSitemapXml(IEnumerable<(string loc, DateTime? lastmod, decimal priority)> urls)
        {
            var sb = new System.Text.StringBuilder();
            sb.AppendLine(@"<?xml version=""1.0"" encoding=""UTF-8""?>");
            sb.AppendLine(@"<urlset xmlns=""http://www.sitemaps.org/schemas/sitemap/0.9"">");
            foreach (var u in urls)
            {
                sb.AppendLine("  <url>");
                sb.AppendLine($"    <loc>{System.Security.SecurityElement.Escape(u.loc)}</loc>");
                if (u.lastmod is DateTime dt) sb.AppendLine($"    <lastmod>{dt:yyyy-MM-dd}</lastmod>");
                sb.AppendLine($"    <priority>{u.priority:0.0}</priority>");
                sb.AppendLine("  </url>");
            }
            sb.AppendLine("</urlset>");
            return sb.ToString();
        }
    }

}
