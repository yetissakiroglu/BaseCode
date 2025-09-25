using Economy.Web.UI.Services.Abstractions;

namespace Economy.Web.UI.Services.Implementations
{
    public class InMemoryGalleryService : IGalleryService
    {
        private static string Img(string w, string h, string seed) => $"https://picsum.photos/seed/{Uri.EscapeDataString(seed)}/{w}/{h}";
        private readonly List<GalleryItem> _items = Enumerable.Range(1, 12)
            .SelectMany(i => new[]{
            new GalleryItem(i,"tr-TR",$"Galeri {i}", Img("1200","800",$"g{i}"), i%2==0?"Oda":"Otel"),
            new GalleryItem(i,"en-US",$"Gallery {i}", Img("1200","800",$"g{i}"), i%2==0?"Room":"Hotel")
            }).ToList();

        public Task<(int total, List<GalleryItem> items)> ListAsync(string culture, string? category = null, int page = 1, int pageSize = 12)
        {
            var q = _items.Where(x => x.Culture == culture);
            if (!string.IsNullOrWhiteSpace(category)) q = q.Where(x => x.Category.Equals(category, StringComparison.OrdinalIgnoreCase));
            var total = q.Count();
            var items = q.Skip((page - 1) * pageSize).Take(pageSize).ToList();
            return Task.FromResult((total, items));
        }

        public Task<List<string>> CategoriesAsync(string culture)
            => Task.FromResult(_items.Where(x => x.Culture == culture).Select(x => x.Category).Distinct(StringComparer.OrdinalIgnoreCase).OrderBy(s => s).ToList());
    }

}
