using Economy.Web.UI.Services.Abstractions;

namespace Economy.Web.UI.Services.Implementations
{
    public class InMemoryCommentsService : ICommentsService
    {
        private readonly List<Comment> _list = new();
        private int _id = 1;
        public Task<List<Comment>> ListAsync(string culture, string type, string slug, int take = 50) =>
          Task.FromResult(_list.Where(x => x.Culture == culture && x.Type == type && x.Slug == slug)
                                .OrderByDescending(x => x.CreatedAt).Take(take).ToList());
        public Task<bool> CreateAsync(Comment c)
        {
            _list.Add(c with { Id = _id++, CreatedAt = DateTime.UtcNow });
            return Task.FromResult(true);
        }
    }

}
