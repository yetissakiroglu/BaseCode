namespace Economy.Web.UI.Services.Abstractions
{
    public record GalleryItem(int Id, string Culture, string Title, string ImageUrl, string Category);

    public interface IGalleryService
    {
        Task<(int total, List<GalleryItem> items)> ListAsync(string culture, string? category = null, int page = 1, int pageSize = 12);
        Task<List<string>> CategoriesAsync(string culture);
    }

}
