using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Routing;
using MyHotelSite.Models;
using MyHotelSite.Repositories;

namespace MyHotelSite.Services;

public interface ISeoHelper
{
    Task<SeoViewModel> BuildAsync(
        int appId, SeoSeed seed, string controller, string action,
        object? routeValuesBase = null, string? currentLang = null,
        string? xDefaultUrl = null
    );
}

public class SeoHelper : ISeoHelper
{
    private readonly ILanguageService _langs;
    private readonly IActionContextAccessor _ac;
    private readonly IUrlHelperFactory _urlFactory;
    private readonly ISiteConfigAccessor _cfg;

    public SeoHelper(ILanguageService langs, IActionContextAccessor ac, IUrlHelperFactory urlFactory, ISiteConfigAccessor cfg)
    { _langs = langs; _ac = ac; _urlFactory = urlFactory; _cfg = cfg; }

    public async Task<SeoViewModel> BuildAsync(int appId, SeoSeed seed, string controller, string action, object? routeValuesBase = null, string? currentLang = null, string? xDefaultUrl = null)
    {
        var ac = _ac.ActionContext ?? throw new InvalidOperationException("No ActionContext");
        var url = _urlFactory.GetUrlHelper(ac);
        var req = ac.HttpContext.Request;
        var detected = currentLang ?? (ac.RouteData.Values.TryGetValue("lang", out var r) ? r?.ToString() : null) ?? "tr";

        var langs = await _langs.GetAllAsync(appId);
        var cfg = await _cfg.GetAsync(appId);
        var domainMap = cfg.Technical?.HreflangDomainMap ?? new Dictionary<string, string>();

        string BuildAbs(string langCode, object? rvBase)
        {
            var u = url.Action(action, controller, Merge(rvBase, new { lang = langCode }), req.Scheme) ?? req.GetDisplayUrl();
            if (domainMap.TryGetValue(langCode, out var baseUrl) && !string.IsNullOrWhiteSpace(baseUrl))
            {
                var uri = new Uri(u);
                var pathAndQuery = uri.PathAndQuery + uri.Fragment;
                return baseUrl.TrimEnd('/') + pathAndQuery;
            }
            return u;
        }

        var canonical = BuildAbs(detected, routeValuesBase);
        var hreflangs = langs.Select(l => (l.Code, BuildAbs(l.Code, routeValuesBase))).ToList();
        if (!string.IsNullOrWhiteSpace(xDefaultUrl))
            hreflangs.Add(("x-default", xDefaultUrl!));

        return new SeoViewModel
        {
            MetaTitle = seed.Title,
            MetaDescription = seed.Description,
            CanonicalUrl = canonical,
            ShareImage = seed.ShareImage,
            OgType = seed.OgType,
            Hreflangs = hreflangs,
            JsonLd = seed.JsonLd
        };
    }

    private static object Merge(object? a, object b)
    {
        var d = a is null ? new RouteValueDictionary() : new RouteValueDictionary(a);
        foreach (var kv in new RouteValueDictionary(b)) d[kv.Key] = kv.Value;
        return d;
    }
}
