namespace Economy.Web.UI.Services.Implementations
{
    using Economy.Web.UI.Services.Abstractions;
    using Microsoft.AspNetCore.Localization;
    using Microsoft.Extensions.Caching.Memory;
    using Microsoft.Extensions.Localization;

    public class ServiceStringLocalizerFactory : IStringLocalizerFactory
    {
        private readonly IServiceProvider _sp;
        public ServiceStringLocalizerFactory(IServiceProvider sp) => _sp = sp;

        public IStringLocalizer Create(Type resourceSource) => Create(resourceSource.FullName ?? resourceSource.Name, null);

        public IStringLocalizer Create(string baseName, string? location)
        {
            baseName = NormalizeBase(baseName);
            var http = _sp.GetRequiredService<IHttpContextAccessor>().HttpContext;
            var culture = (string?)http?.Features.Get<IRequestCultureFeature>()?.RequestCulture.UICulture.Name
                          ?? Thread.CurrentThread.CurrentUICulture.Name;
            return ActivatorUtilities.CreateInstance<ServiceStringLocalizer>(_sp, baseName, culture);
        }

        private static string NormalizeBase(string baseName)
        {
            if (baseName.StartsWith("Views.", StringComparison.OrdinalIgnoreCase))
            {
                var parts = baseName.Split('.'); if (parts.Length >= 3) return $"{parts[0]}.{parts[1]}"; // Views.Home
            }
            if (baseName.Contains("Shared", StringComparison.OrdinalIgnoreCase)) return "Shared";
            return "Shared";
        }
    }

    public class ServiceStringLocalizer : IStringLocalizer
    {
        private readonly string _ns, _culture;
        private readonly ILocalizationStore _store;
        private readonly IMemoryCache _cache;
        public ServiceStringLocalizer(string ns, string culture, ILocalizationStore store, IMemoryCache cache)
        {
            _ns = ns; _culture = culture; _store = store; _cache = cache;
        }
        private string CK => $"loc:{_culture}:{_ns}";
        private async Task<IDictionary<string, string>> GetNs() =>
          await _cache.GetOrCreateAsync(CK, async e => {
              e.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10);
              return await _store.GetNamespaceAsync(_culture, _ns);
          }) ?? new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

        public LocalizedString this[string name]
        {
            get
            {
                var dict = GetNs().GetAwaiter().GetResult();
                var full = $"{_ns}.{name}";
                if (dict.TryGetValue(full, out var v)) return new LocalizedString(name, v, false);
                if (!_ns.Equals("Shared", StringComparison.OrdinalIgnoreCase))
                {
                    var shared = _cache.GetOrCreate($"loc:{_culture}:Shared", e => {
                        e.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10);
                        return _store.GetNamespaceAsync(_culture, "Shared").GetAwaiter().GetResult();
                    })!;
                    if (shared.TryGetValue("Shared." + name, out var sv)) return new LocalizedString(name, sv, false);
                }
                return new LocalizedString(name, name, true);
            }
        }

        public LocalizedString this[string name, params object[] arguments]
          => new(name, string.Format(this[name].Value, arguments), false);

        public IEnumerable<LocalizedString> GetAllStrings(bool includeParentCultures)
        {
            var dict = GetNs().GetAwaiter().GetResult();
            foreach (var kv in dict)
            {
                var display = kv.Key.StartsWith(_ns + ".") ? kv.Key.Substring(_ns.Length + 1) : kv.Key;
                yield return new LocalizedString(display, kv.Value, false);
            }
        }
    }

}
