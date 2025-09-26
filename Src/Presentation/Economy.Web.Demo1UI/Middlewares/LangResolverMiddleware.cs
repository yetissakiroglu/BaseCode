using MyHotelSite.Repositories;

namespace MyHotelSite.Middlewares;

public class LangResolverMiddleware
{
    private readonly RequestDelegate _next;
    private const string LangCookieName = "site.lang";
    public LangResolverMiddleware(RequestDelegate next) => _next = next;

    public async Task Invoke(HttpContext ctx, ILanguageService langs, IAppSettingTechnicalRepository techRepo)
    {
        var appId = 1;
        var supported = (await langs.GetAllAsync(appId)).Select(x => x.Code).ToHashSet(StringComparer.OrdinalIgnoreCase);
        var defaultLang = (await techRepo.GetAsync(appId))?.DefaultLanguage ?? "tr";

        // sitemap/robots/favicon dışarıda bırak
        var path = ctx.Request.Path.Value ?? "/";
        if (path.StartsWith("/sitemap") || path == "/robots.txt" || path == "/favicon.ico")
        {
            await _next(ctx);
            return;
        }

        var seg = path.Split('/', StringSplitOptions.RemoveEmptyEntries).FirstOrDefault();
        var lang = seg;
        if (string.IsNullOrWhiteSpace(lang) || !supported.Contains(lang))
        {
            if (ctx.Request.Cookies.TryGetValue(LangCookieName, out var cval) && supported.Contains(cval))
                lang = cval;
            else
                lang = defaultLang;

            var rest = path.TrimStart('/');
            var newPath = $"/{lang}/{rest}";
            var url = $"{ctx.Request.Scheme}://{ctx.Request.Host}{newPath}{ctx.Request.QueryString}";
            ctx.Response.Redirect(url, permanent: false);
            return;
        }

        ctx.Items["Lang"] = lang;
        ctx.Response.Cookies.Append(LangCookieName, lang, new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1), HttpOnly = false, SameSite = SameSiteMode.Lax });

        var culture = lang switch
        {
            "tr" => "tr-TR",
            "de" => "de-DE",
            _ => "en-US"
        };
        var ci = new System.Globalization.CultureInfo(culture);
        System.Globalization.CultureInfo.CurrentCulture = ci;
        System.Globalization.CultureInfo.CurrentUICulture = ci;

        await _next(ctx);
    }
}
