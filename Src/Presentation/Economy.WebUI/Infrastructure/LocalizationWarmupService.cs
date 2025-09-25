namespace Economy.Web.UI.Infrastructure
{
    using Economy.Web.UI.Services.Abstractions;
    using Microsoft.Extensions.Hosting;

    public class LocalizationWarmupService : IHostedService, IDisposable
    {
        private readonly ILanguageService _langs; private readonly ILocalizationStore _store; private readonly LanguageCache _cache;
        private Timer? _timer;
        public LocalizationWarmupService(ILanguageService langs, ILocalizationStore store, LanguageCache cache) { _langs = langs; _store = store; _cache = cache; }

        public async Task StartAsync(CancellationToken ct)
        {
            await WarmAsync();
            _timer = new Timer(async _ => await WarmAsync(), null, TimeSpan.FromMinutes(10), TimeSpan.FromMinutes(10));
        }
        private async Task WarmAsync()
        {
            var list = await _langs.GetAsync();
            _cache.Set(list.Select(x => (x.ShortCode, x.Code)));
            var def = await _langs.GetDefaultAsync();
            _ = await _store.GetNamespaceAsync(def.Code, "Shared");
            _ = await _store.GetNamespaceAsync(def.Code, "Views.Home");
        }
        public Task StopAsync(CancellationToken ct) { _timer?.Dispose(); return Task.CompletedTask; }
        public void Dispose() { _timer?.Dispose(); }
    }

}
