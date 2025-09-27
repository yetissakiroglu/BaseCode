using MyHotelSite.Repositories;
using MyHotelSite.Services;

namespace MyHotelSite.Middlewares;

public class LangResolverMiddleware
{
    private readonly RequestDelegate _next;
    private const string LangCookieName = "site.lang";

    public LangResolverMiddleware(RequestDelegate next) => _next = next;

    public async Task Invoke(HttpContext ctx, ILanguageService langs, ISiteConfigAccessor techRepo)
    {
        var p = ctx.Request.Path.Value ?? "/";
        if (p.StartsWith("/css/", StringComparison.OrdinalIgnoreCase) ||
            p.StartsWith("/js/", StringComparison.OrdinalIgnoreCase) ||
            p.StartsWith("/images/", StringComparison.OrdinalIgnoreCase) ||
            p.StartsWith("/img/", StringComparison.OrdinalIgnoreCase) ||
            p.StartsWith("/fonts/", StringComparison.OrdinalIgnoreCase) ||
            p.StartsWith("/lib/", StringComparison.OrdinalIgnoreCase) ||
            p.StartsWith("/assets/", StringComparison.OrdinalIgnoreCase) ||
            p.StartsWith("/sitemap", StringComparison.OrdinalIgnoreCase) ||
            string.Equals(p, "/robots.txt", StringComparison.OrdinalIgnoreCase) ||
            string.Equals(p, "/favicon.ico", StringComparison.OrdinalIgnoreCase))
        { await _next(ctx); return; }

        var appId = 1;
        var supported = (await langs.GetAllAsync(appId)).Select(x => x.Code)
                         .ToHashSet(StringComparer.OrdinalIgnoreCase);
        var defaultLang = (await techRepo.GetAsync("tr")).Technical?.DefaultLanguage ?? "tr";

        // İlk segment dil mi?
        var seg = p.Split('/', StringSplitOptions.RemoveEmptyEntries).FirstOrDefault();
        if (!string.IsNullOrWhiteSpace(seg) && supported.Contains(seg))
        {
            var lang = seg;

            // Request yaşamına yaz
            ctx.Items["Lang"] = lang;

            // Kültür
            var culture = lang switch { "tr" => "tr-TR", "de" => "de-DE", _ => "en-US" };
            var ci = new System.Globalization.CultureInfo(culture);
            System.Globalization.CultureInfo.CurrentCulture = ci;
            System.Globalization.CultureInfo.CurrentUICulture = ci;

            // Hatırlatma çerezi
            ctx.Response.Cookies.Append(LangCookieName, lang,
                new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1), HttpOnly = false, SameSite = SameSiteMode.Lax });

            await _next(ctx);
            return;
        }

        // Dil YOKSA → sadece GET/HEAD isteklerinde dil ekleyip yönlendir
        var cookieLang = ctx.Request.Cookies.TryGetValue(LangCookieName, out var cval) && supported.Contains(cval)
            ? cval : defaultLang;

        if (HttpMethods.IsGet(ctx.Request.Method) || HttpMethods.IsHead(ctx.Request.Method))
        {
            var rest = p.TrimStart('/');
            var newPath = "/" + cookieLang + (string.IsNullOrEmpty(rest) ? "" : "/" + rest);
            var url = $"{ctx.Request.Scheme}://{ctx.Request.Host}{newPath}{ctx.Request.QueryString}";
            ctx.Response.Redirect(url, permanent: false);
            return;
        }

        // POST/PUT/PATCH/DELETE → redirect yapma
        await _next(ctx);
    }
}
