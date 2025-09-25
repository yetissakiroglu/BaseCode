using Economy.Web.UI.Services.Abstractions;

namespace Economy.Web.UI.Services.Implementations
{
    public class InMemorySlugService : ISlugService
    {
        private readonly List<SlugRef> _list = new() {
    new("room",1,new(){["tr-TR"]="standart-oda",["en-US"]="standard-room"}),
    new("room",2,new(){["tr-TR"]="deluxe-oda",  ["en-US"]="deluxe-room"})
  };
        public Task<List<SlugRef>> GetAllAsync() => Task.FromResult(_list);
        public Task<SlugRef?> GetByRefAsync(string type, int id) => Task.FromResult(_list.FirstOrDefault(x => x.Type == type && x.RefId == id));
        public string? FindSlug(SlugRef sref, string culture) => sref.Slugs.TryGetValue(culture, out var v) ? v : null;
        public (string type, int id, string culture)? Resolve(string type, string slug)
        {
            foreach (var s in _list.Where(x => x.Type == type))
            {
                var hit = s.Slugs.FirstOrDefault(kv => kv.Value.Equals(slug, StringComparison.OrdinalIgnoreCase));
                if (!hit.Equals(default(KeyValuePair<string, string>))) return (s.Type, s.RefId, hit.Key);
            }
            return null;
        }
    }

}
