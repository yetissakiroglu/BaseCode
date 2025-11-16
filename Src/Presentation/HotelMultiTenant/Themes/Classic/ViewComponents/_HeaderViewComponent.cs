using HotelMultiTenant.Services;
using Microsoft.AspNetCore.Mvc;

namespace HotelMultiTenant.Themes.Classic.ViewComponents
{
    public class _HeaderViewComponent : ViewComponent
    {
        private readonly IContentService _contentService;
        public _HeaderViewComponent(IContentService contentService)
        {
            _contentService = contentService;
        }

        public async Task<IViewComponentResult> InvokeAsync(CancellationToken ct)
        {
            var header = new Models.HeaderComponentViewModel();
            var lang = (string?)HttpContext.Items["Lang"];
            var menus = await _contentService.GetMenusAsync(lang, ct);
            header.Menus = menus;
            // Eğer özel yol kullanacaksan (opsiyonel):
            return View("Default", header);
        }
    }
}
