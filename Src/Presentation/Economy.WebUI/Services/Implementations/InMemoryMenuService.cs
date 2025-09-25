using Economy.Web.UI.Models;
using Economy.Web.UI.Services.Abstractions;

namespace Economy.Web.UI.Services.Implementations
{

    public class InMemoryMenuService : IMenuService
    {
        private static Dictionary<string, string> T(string tr, string en) => new()
        {
            ["tr-TR"] = tr,
            ["en-US"] = en
        };

        public Task<List<MenuItem>> GetMainAsync()
        {
            var list = new List<MenuItem>
        {
            new("home",    T("Ana Sayfa","Home"), PageID: 1), // PageID öncelikli
            new("rooms",   T("Odalarımız","Rooms"), "Rooms",  "Index"),
            new("blog",    T("Blog","Blog"),        "Blog",   "Index"),
            new("gallery", T("Galeri","Gallery"),   "Gallery","Index"),
            new("faq",     T("SSS","FAQ"),          "Faq",    "Index"),
            new("contact", T("İletişim","Contact"), PageID: 2)
        };

            // Örnek dropdown (istersen aç)
            // list[1] = list[1] with { Children = new()
            // {
            //     new("std", T("Standart Oda","Standard"), "Rooms","Detail", new { slug="standart-oda" }),
            //     new("dlx", T("Deluxe Oda","Deluxe"),     "Rooms","Detail", new { slug="deluxe-oda" })
            // }};

            return Task.FromResult(list);
        }

        public Task<List<MenuItem>> GetFooterAsync()
        {
            var list = new List<MenuItem>
        {
            new("about",  T("Hakkımızda","About"),  PageID: 1),
            new("policy", T("Gizlilik","Privacy"),  "Home","Privacy"),
            new("terms",  T("Koşullar","Terms"),    "Home","Terms"),
            new("map",    T("Site Haritası","Sitemap"), Url:"/sitemap.xml", External:true)
        };
            return Task.FromResult(list);
        }

        public string TitleFor(MenuItem item, string culture)
            => item.Titles.TryGetValue(culture, out var t) ? t : item.Titles.Values.First();
    }
}
