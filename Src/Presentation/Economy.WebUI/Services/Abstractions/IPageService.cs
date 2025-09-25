namespace Economy.Web.UI.Services.Abstractions
{
    public interface IPageService
    {
        // cultureShort: "tr" / "en"
        Task<string?> GetSlugAsync(int pageId, string cultureShort);
    }
}
