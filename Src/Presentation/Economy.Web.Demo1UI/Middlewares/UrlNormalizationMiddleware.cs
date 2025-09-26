namespace MyHotelSite.Middlewares;

public class UrlNormalizationMiddleware
{
    private readonly RequestDelegate _next;
    public UrlNormalizationMiddleware(RequestDelegate next) => _next = next;

    public async Task Invoke(HttpContext ctx)
    {
        var req = ctx.Request;
        var path = req.Path.Value ?? "/";

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



        var norm = path;

        // Lowercase
        norm = norm.ToLowerInvariant();

        // Trailing slash (kök hariç) → kaldır
        if (norm != "/" && norm.EndsWith('/')) norm = norm.TrimEnd('/');

        // Çoklu slash → tek slash
        while (norm.Contains("//")) norm = norm.Replace("//", "/");

        if (norm != path)
        {
            var url = $"{req.Scheme}://{req.Host}{norm}{req.QueryString}";
            ctx.Response.Redirect(url, permanent: true);
            return;
        }

        await _next(ctx);
    }
}
