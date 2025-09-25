namespace Economy.Web.UI.Infrastructure
{
    using Economy.Web.UI.Services.Abstractions;
    using Microsoft.AspNetCore.Localization;

    public class DynamicRouteRequestCultureProvider : RequestCultureProvider
    {
        public override async Task<ProviderCultureResult?> DetermineProviderCultureResult(HttpContext httpContext)
        {
            var seg = (httpContext.Request.Path.Value ?? "").Trim('/').Split('/', StringSplitOptions.RemoveEmptyEntries);
            var first = seg.Length > 0 ? seg[0] : null;
            var langSvc = httpContext.RequestServices.GetRequiredService<ILanguageService>();
            var def = await langSvc.GetDefaultAsync();

            if (!string.IsNullOrWhiteSpace(first) && await langSvc.IsSupportedAsync(first))
            {
                var full = await langSvc.ToFullAsync(first) ?? def.Code;
                return new ProviderCultureResult(full, full);
            }
            var cookieProvider = new CookieRequestCultureProvider();
            var cookieRes = await cookieProvider.DetermineProviderCultureResult(httpContext);
            var cookieCulture = cookieRes?.UICultures?.FirstOrDefault().Value;
            if (!string.IsNullOrWhiteSpace(cookieCulture) && await langSvc.IsSupportedAsync(cookieCulture))
                return new ProviderCultureResult(cookieCulture, cookieCulture);
            return new ProviderCultureResult(def.Code, def.Code);
        }
    }

}
