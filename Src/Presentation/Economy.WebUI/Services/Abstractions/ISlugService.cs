namespace Economy.Web.UI.Services.Abstractions
{
    public record SlugRef(string Type, int RefId, Dictionary<string, string> Slugs);
    public interface ISlugService
    {
        Task<List<SlugRef>> GetAllAsync();
        Task<SlugRef?> GetByRefAsync(string type, int id);
        string? FindSlug(SlugRef sref, string culture);
        (string type, int id, string culture)? Resolve(string type, string slug);
    }

}
