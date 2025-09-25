namespace Economy.Web.UI.Controllers
{
    using Economy.Web.UI.Services.Abstractions;
    using Microsoft.AspNetCore.Localization;
    using Microsoft.AspNetCore.Mvc;

    public class LanguageController : Controller
    {
        private readonly ILanguageService _langs;
        public LanguageController(ILanguageService l) => _langs = l;

        [HttpPost("/Language/Set")]
        [HttpPost("{culture:shortculture}/Language/Set")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Set(string culture, string? returnUrl = "/")
        {
            var full = await _langs.ToFullAsync(culture) ?? (await _langs.GetDefaultAsync()).Code;
            Response.Cookies.Append(
              CookieRequestCultureProvider.DefaultCookieName,
              CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(full)),
              new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1) });

            var shortCode = await _langs.ToShortAsync(full) ?? "en";
            var path = returnUrl ?? "/";
            var seg = path.Trim('/').Split('/', StringSplitOptions.RemoveEmptyEntries).ToList();
            if (seg.Count == 0) return LocalRedirect($"/{shortCode}");
            var all = await _langs.GetAsync();
            if (all.Any(l => seg[0].Equals(l.ShortCode, StringComparison.OrdinalIgnoreCase) || seg[0].Equals(l.Code, StringComparison.OrdinalIgnoreCase)))
                seg[0] = shortCode;
            else seg.Insert(0, shortCode);
            return LocalRedirect("/" + string.Join('/', seg));
        }
    }

}
