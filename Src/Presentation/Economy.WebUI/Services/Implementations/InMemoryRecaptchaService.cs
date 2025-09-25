using Economy.Web.UI.Services.Abstractions;

namespace Economy.Web.UI.Services.Implementations
{
    public class InMemoryRecaptchaService : IRecaptchaService
    {
        public Task<bool> VerifyAsync(string token) => Task.FromResult(!string.IsNullOrWhiteSpace(token));
        public string? GetSiteKey() => null; // demo
    }

}
