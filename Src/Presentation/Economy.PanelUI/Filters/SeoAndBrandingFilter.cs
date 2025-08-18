using Economy.Application.Providers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Economy.Panel.UI.Filters
{
    public class SeoAndBrandingFilter : IAsyncActionFilter
    {
        private readonly IAppSettingsProvider _provider;

        public SeoAndBrandingFilter(IAppSettingsProvider provider) => _provider = provider;

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var executed = await next();

            // Sadece ViewResult'larda çalışalım
            if (executed.Result is ViewResult vr)
            {
                var s = await _provider.GetGeneralAsync();

                // Başlık
                var currentTitle = vr.ViewData["TitleDefault"]?.ToString();
                var titleSuffix = s?.MetaTitleSuffix;
                if (!string.IsNullOrWhiteSpace(currentTitle) && !string.IsNullOrWhiteSpace(titleSuffix))
                    vr.ViewData["TitleDefault"] = currentTitle + titleSuffix;
                else
                    vr.ViewData["TitleDefault"] = currentTitle ?? s?.SiteName ?? "Uygulama";

                // Meta
                if (vr.ViewData["MetaDescriptionDefault"] is null && !string.IsNullOrWhiteSpace(s?.DefaultMetaDescription))
                    vr.ViewData["MetaDescriptionDefault"] = s!.DefaultMetaDescription;
                            
                // Tema/Logo/SiteName (Layout'ta kullanacağız)
                vr.ViewData["Theme"] = s?.Theme ?? "light";
                vr.ViewData["LogoUrl"] = s?.LogoUrl;
                vr.ViewData["SiteName"] = s?.SiteName ?? "Uygulama";
            }
        }
    }
}
