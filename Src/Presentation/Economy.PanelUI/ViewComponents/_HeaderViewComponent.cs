using Economy.Panel.Application.Interfaces;
using Economy.Panel.UI.Models.ProfileViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Economy.Panel.UI.ViewComponents
{
    public class _HeaderViewComponent : ViewComponent
    {
        // Bu metod, kullanıcının giriş yapıp yapmadığını kontrol eder
        private readonly IPanelAppUserService _panelAppUserService;

        public _HeaderViewComponent(IPanelAppUserService panelAppUserService)
        {
            _panelAppUserService = panelAppUserService;
        }

        public IViewComponentResult Invoke()
        {
            var claimsPrincipal = User as ClaimsPrincipal;
            var CurrentUserId = claimsPrincipal?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var user = _panelAppUserService.GetUser(Convert.ToInt32(CurrentUserId),false);
            var model = new HeaderUserViewModel
            {
                FullName = user.Result?.Data.FirstName + " " + user.Result?.Data.LastName  ?? "Kullanıcı Adı", // Gerçek veri varsa claim'e ekle
                Email = user.Result.Data.Email,
                PhotoUrl = user.Result.Data.PhotoUrl ?? "/assets/images/avtar/woman.jpg" // Gerçek veri varsa claim'e ekle
            };

            return View("Components/Header/_HeaderComponent.cshtml", model);
        }
    }
}
