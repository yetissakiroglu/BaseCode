using Economy.Web.Demo1.Services;
using Microsoft.AspNetCore.Mvc;

namespace Economy.Web.Demo1.ViewComponents
{
   
    public class _SlideViewComponent : ViewComponent
    {
        private readonly ISiteConfigAccessor _siteConfigAccessor;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public _SlideViewComponent(ISiteConfigAccessor siteConfigAccessor,
                                  IHttpContextAccessor httpContextAccessor)
        {
            _siteConfigAccessor = siteConfigAccessor;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            // HttpContext doğrudan ViewComponent içinde erişilebilir
            var lang = (string?)HttpContext.Items["Lang"] ?? "tr";
            // .Result yerine await kullan
            var slide = await _siteConfigAccessor.GetSlidesAsync(lang);

            // View keşif kuralını kullan (önerilen)
            // /Views/Shared/Components/Slide/Default.cshtml

            // Eğer özel yol kullanacaksan (opsiyonel):
            return View("Default", slide);
        }
    }

}
