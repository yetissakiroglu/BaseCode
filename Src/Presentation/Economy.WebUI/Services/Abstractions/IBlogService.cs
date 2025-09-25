namespace Economy.Web.UI.Services.Abstractions
{
    public record BlogPost(int Id, string Culture, string Slug, string Title, string Excerpt, string Html, DateTime UpdatedAt, string[] Tags);

    public interface IBlogService
    {
        Task<(int total, List<BlogPost> items)> ListAsync(string culture, string? tag = null, int page = 1, int pageSize = 10);
        Task<BlogPost?> GetAsync(string culture, string slug);
    }

}
