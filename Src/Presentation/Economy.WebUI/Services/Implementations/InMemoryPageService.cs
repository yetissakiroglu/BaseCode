using Economy.Web.UI.Services.Abstractions;

namespace Economy.Web.UI.Services.Implementations
{
    public class InMemoryPageService : IPageService
    {
        // cultureShort ile map (kısa kod)
        private readonly Dictionary<(int, string), string> _map = new()
    {
        {(1,"tr"), "/tr/hakkimizda"},
        {(1,"en"), "/en/about"},
        {(2,"tr"), "/tr/iletisim"},
        {(2,"en"), "/en/contact"}
    };

        public Task<string?> GetSlugAsync(int pageId, string cultureShort)
        {
            if (_map.TryGetValue((pageId, cultureShort), out var slug))
                return Task.FromResult<string?>(slug);
            return Task.FromResult<string?>(null);
        }
    }

}
