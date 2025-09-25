namespace Economy.Web.UI.Services.Abstractions
{
    public record LangItem(string Code, string ShortCode, string Name, bool IsDefault, bool IsRtl);
    public interface ILanguageService
    {
        Task<IReadOnlyList<LangItem>> GetAsync();
        Task<LangItem> GetDefaultAsync();
        Task<bool> IsSupportedAsync(string cultureOrShort);
        Task<string?> ToFullAsync(string cultureOrShort);
        Task<string?> ToShortAsync(string culture);
    }

}
