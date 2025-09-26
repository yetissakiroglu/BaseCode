using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

public class SecurityHeadersMiddleware
{
    private readonly RequestDelegate _next;
    public SecurityHeadersMiddleware(RequestDelegate next) => _next = next;

    public async Task Invoke(HttpContext ctx)
    {
        var rsp = ctx.Response.Headers;
        rsp["X-Content-Type-Options"] = "nosniff";
        rsp["X-Frame-Options"] = "SAMEORIGIN";
        rsp["Referrer-Policy"] = "strict-origin-when-cross-origin";
        rsp["Permissions-Policy"] = "geolocation=(), microphone=(), camera=()";

        // Basit CSP (CDN’ini ekle)
        var cdn = "https://cdn.example.com";
        rsp["Content-Security-Policy"] =
            $"default-src 'self'; " +
            $"img-src 'self' {cdn} data: https:; " +
            $"script-src 'self' {cdn} https://www.googletagmanager.com; " +
            $"style-src 'self' {cdn} 'unsafe-inline'; " + // inline stil varsa
            $"font-src 'self' {cdn} data:; " +
            $"connect-src 'self' {cdn}; " +
            $"frame-src https://www.googletagmanager.com; " +
            $"object-src 'none'; base-uri 'self'";

        await _next(ctx);
    }
}
