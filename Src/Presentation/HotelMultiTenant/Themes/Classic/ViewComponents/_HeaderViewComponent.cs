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
            var header = new Models.HeaderComponentViewModel
            {
                Title = "Welcome to Our Hotel",
                Subtitle = "Experience luxury and comfort",
                BackgroundImageUrl = "/images/header-background.jpg"
            };
         

            var menus = await _contentService.GetMenusAsync(ct);
            header.Menus = menus;
            // Eğer özel yol kullanacaksan (opsiyonel):
            return View("Default", header);
        }
    }
}
