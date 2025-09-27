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
        SeoSeed seed, string controller, string action,
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

    public async Task<SeoViewModel> BuildAsync(
        SeoSeed seed, string controller, string action,
        object? routeValuesBase = null, string? currentLang = null,
        string? xDefaultUrl = null)
    {
        var ac = _ac.ActionContext ?? throw new InvalidOperationException("No ActionContext");
        var url = _urlFactory.GetUrlHelper(ac);
        var req = ac.HttpContext.Request;

        // dil tespiti
        var detected = currentLang
                       ?? (ac.RouteData.Values.TryGetValue("lang", out var r) ? r?.ToString() : null)
                       ?? "tr";

        var langs = await _langs.GetAllAsync(1);
        // Config'i mevcut/tespit dil ile getir (önceden currentLang gönderiliyordu)
        var cfg = await _cfg.GetAsync(detected);

        string BuildAbs(string langCode, object? rvBase)
        {
            // 1) Route'a dil paramını enjekte ederek mutlak URL üret
            var u = url.Action(action, controller, Merge(rvBase, new { lang = langCode }), req.Scheme)
                    ?? req.GetDisplayUrl();

            // 2) ForceSSL/DomainName varsa base kısmını override et
            var original = new Uri(u);

            var targetScheme = (cfg.Technical?.ForceSSL == true) ? "https" : original.Scheme;
            var targetHost = !string.IsNullOrWhiteSpace(cfg.Technical?.DomainName)
                               ? cfg.Technical!.DomainName
                               : original.Host;

            // Port kullanımını istersen koruyabilirsin; çoğu senaryoda gerekmez:
            // var portPart = original.IsDefaultPort ? "" : $":{original.Port}";
            // return $"{targetScheme}://{targetHost}{portPart}{original.PathAndQuery}{original.Fragment}";

            return $"{targetScheme}://{targetHost}{original.PathAndQuery}{original.Fragment}";
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
