using Economy.Web.UI.Services.Abstractions;

namespace Economy.Web.UI.Services.Implementations
{
    public class InMemoryLanguageService : ILanguageService
    {
        private readonly List<LangItem> _langs = new() {
    new("tr-TR","tr","Türkçe", true,  false),
    new("en-US","en","English", false, false),
  };
        public Task<IReadOnlyList<LangItem>> GetAsync() => Task.FromResult<IReadOnlyList<LangItem>>(_langs);
        public Task<LangItem> GetDefaultAsync() => Task.FromResult(_langs.First(l => l.IsDefault));
        public Task<bool> IsSupportedAsync(string c) => Task.FromResult(_langs.Any(l => l.Code.Equals(c, StringComparison.OrdinalIgnoreCase) || l.ShortCode.Equals(c, StringComparison.OrdinalIgnoreCase)));
        public Task<string?> ToFullAsync(string c) => Task.FromResult(_langs.FirstOrDefault(l => l.Code.Equals(c, StringComparison.OrdinalIgnoreCase) || l.ShortCode.Equals(c, StringComparison.OrdinalIgnoreCase))?.Code);
        public Task<string?> ToShortAsync(string culture) => Task.FromResult(_langs.FirstOrDefault(l => l.Code.Equals(culture, StringComparison.OrdinalIgnoreCase))?.ShortCode);
    }

}
