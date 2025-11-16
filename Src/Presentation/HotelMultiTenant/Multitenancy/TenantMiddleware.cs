namespace HotelMultiTenant.Multitenancy
{
    public class TenantMiddleware : IMiddleware
    {
        private readonly ITenantDirectory _directory;
        private const string LangCookieName = "site.lang";

        public TenantMiddleware(ITenantDirectory directory) => _directory = directory;

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            var host = context.Request.Host.Host ?? string.Empty;
            var tenant = await _directory.ResolveByHostAsync(host, context.RequestAborted);
            context.SetTenant(new TenantContext { Current = tenant });

            var supported = (tenant?.SupportedLanguages ?? Array.Empty<string>())
                .ToHashSet(StringComparer.OrdinalIgnoreCase);

            var defaultLang = string.IsNullOrWhiteSpace(tenant?.DefaultLanguage)
                ? "tr"
                : tenant.DefaultLanguage;

            var path = context.Request.Path.Value ?? "/";

            // 0) Statik dosyalar ve bazı özel path'ler için DİL ZORLAMA YAPMA
            if (IsStaticPath(path))
            {
                await next(context);
                return;
            }

            // 1) İlk segmentten dil tespiti
            var seg = path
                .Split('/', StringSplitOptions.RemoveEmptyEntries)
                .FirstOrDefault();

            var lang = defaultLang;

            if (!string.IsNullOrWhiteSpace(seg) && supported.Contains(seg))
            {
                lang = seg;
            }
            else
            {
                if (context.Request.Cookies.TryGetValue(LangCookieName, out var langx))
                {
                    lang = langx;
                    // lang değişkeni çerezi içerir
                }
            }



            // Request yaşamına yaz
            context.Items["Lang"] = lang;

            // Tenant SEO meta (dil bazlı)
            var tenantSeo = await _directory.ResolveByLangSeoMetaAsync(lang, context.RequestAborted);
            context.SetSeoMetaTenant(new SeoMetaContext { Current = tenantSeo });

            // Kültür
            var culture = lang switch
            {
                "tr" => "tr-TR",
                "de" => "de-DE",
                _ => "en-US"
            };

            var ci = new System.Globalization.CultureInfo(culture);
            System.Globalization.CultureInfo.CurrentCulture = ci;
            System.Globalization.CultureInfo.CurrentUICulture = ci;

            // Dil hatırlatma çerezi
            context.Response.Cookies.Append(
                LangCookieName,
                lang,
                new CookieOptions
                {
                    Expires = DateTimeOffset.UtcNow.AddMinutes(1),
                    HttpOnly = false,
                    SameSite = SameSiteMode.Lax
                });


            await next(context);
        }

        private static bool IsStaticPath(string path)
        {
            if (string.IsNullOrEmpty(path))
                return false;

            var p = path.ToLowerInvariant();

            // Klasör bazlı whitelist
            if (p.StartsWith("/assets") ||
                p.StartsWith("/themes") ||
                p.StartsWith("/lib") ||
                p.StartsWith("/css") ||
                p.StartsWith("/js"))
                return true;

            // Özel dosyalar
            if (p.Equals("/favicon.ico") ||
                p.Equals("/robots.txt") ||
                p.StartsWith("/sitemap", StringComparison.OrdinalIgnoreCase))
                return true;

            // Uzantı bazlı (css, js, img, font vs.)
            var ext = System.IO.Path.GetExtension(p);
            if (string.IsNullOrEmpty(ext))
                return false;

            switch (ext)
            {
                case ".css":
                case ".js":
                case ".png":
                case ".jpg":
                case ".jpeg":
                case ".gif":
                case ".svg":
                case ".ico":
                case ".webp":
                case ".avif":
                case ".bmp":
                case ".ttf":
                case ".otf":
                case ".woff":
                case ".woff2":
                case ".eot":
                case ".map":
                    return true;
                default:
                    return false;
            }
        }
    }
}
