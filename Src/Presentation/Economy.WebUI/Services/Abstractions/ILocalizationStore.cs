namespace Economy.Web.UI.Services.Abstractions
{
    public interface ILocalizationStore
    {
        Task<IDictionary<string, string>> GetNamespaceAsync(string culture, string ns);
    }

}
