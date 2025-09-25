using Economy.Web.UI.Services.Abstractions;

namespace Economy.Web.UI.Services.Implementations
{
    public class InMemoryBlogService : IBlogService
    {
        private readonly List<BlogPost> _posts = new()
    {
        new(1,"tr-TR","sonbaharda-karadeniz","Sonbaharda Karadeniz","Kısa özet...","<p>TR içerik</p>",DateTime.UtcNow.AddDays(-2), new[]{"gezi","otel"}),
        new(2,"en-US","black-sea-in-fall","Black Sea in Fall","Short excerpt...","<p>EN content</p>",DateTime.UtcNow.AddDays(-2), new[]{"travel","hotel"}),
    };

        public Task<(int total, List<BlogPost> items)> ListAsync(string culture, string? tag = null, int page = 1, int pageSize = 10)
        {
            var q = _posts.Where(p => p.Culture == culture);
            if (!string.IsNullOrWhiteSpace(tag)) q = q.Where(p => p.Tags.Contains(tag, StringComparer.OrdinalIgnoreCase));
            var total = q.Count();
            var items = q.OrderByDescending(p => p.UpdatedAt).Skip((page - 1) * pageSize).Take(pageSize).ToList();
            return Task.FromResult((total, items));
        }

        public Task<BlogPost?> GetAsync(string culture, string slug)
            => Task.FromResult(_posts.FirstOrDefault(p => p.Culture == culture && p.Slug == slug));
    }

}
