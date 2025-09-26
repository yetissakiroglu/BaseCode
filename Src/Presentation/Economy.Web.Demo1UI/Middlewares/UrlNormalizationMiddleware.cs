using Microsoft.AspNetCore.Http;

namespace MyHotelSite.Middlewares;

public class UrlNormalizationMiddleware
{
    private readonly RequestDelegate _next;
    public UrlNormalizationMiddleware(RequestDelegate next) => _next = next;

    public async Task Invoke(HttpContext ctx)
    {
        var req = ctx.Request;
        var path = req.Path.Value ?? "/";
        var norm = path.ToLowerInvariant();

        // Trailing slash: kök hariç kaldır
        if (norm != "/" && norm.EndsWith('/')) norm = norm.TrimEnd('/');
        // Çoklu slash → tek slash
        while (norm.Contains("//")) norm = norm.Replace("//", "/");

        // Sitemap/robots/favicon hariç; bunları normalize etme
        var skip = norm.StartsWith("/sitemap") || norm == "/robots.txt" || norm == "/favicon.ico";
        if (!skip && norm != path)
        {
            var url = $"{req.Scheme}://{req.Host}{norm}{req.QueryString}";
            ctx.Response.Redirect(url, permanent: true);
            return;
        }

        await _next(ctx);
    }
}
