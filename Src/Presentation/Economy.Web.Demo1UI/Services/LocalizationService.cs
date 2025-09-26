using Microsoft.Extensions.Caching.Memory;
using MyHotelSite.Repositories;

namespace MyHotelSite.Services;

public interface ILocalizationService { Task<string> T(int appId, string lang, string key, string fallback = ""); }
public class LocalizationService : ILocalizationService
{
    private readonly ILocalizationRepository _repo;
    private readonly IMemoryCache _cache;
    public LocalizationService(ILocalizationRepository repo, IMemoryCache cache) { _repo = repo; _cache = cache; }

    public async Task<string> T(int appId, string lang, string key, string fallback = "")
    {
        var cacheKey = $"loc:{appId}:{lang}:{key}";
        if (_cache.TryGetValue<string>(cacheKey, out var val))
            return val;
        var v = await _repo.GetAsync(appId, lang, key) ?? fallback;
        _cache.Set(cacheKey, v, TimeSpan.FromMinutes(10));
        return v;
    }
}

public interface IViewLocalizerHelper { Task<string> T(HttpContext ctx, string key, string fallback = ""); }
public class ViewLocalizerHelper : IViewLocalizerHelper
{
    private readonly ILocalizationService _svc;
    public ViewLocalizerHelper(ILocalizationService svc) { _svc = svc; }
    public Task<string> T(HttpContext ctx, string key, string fallback = "")
    {
        var appId = 1;
        var lang = (string?)ctx.Items["Lang"] ?? "tr";
        return _svc.T(appId, lang, key, fallback);
    }
}
