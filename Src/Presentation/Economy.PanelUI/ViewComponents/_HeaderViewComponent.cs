using Economy.Panel.UI.Models.ProfileViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Economy.Panel.UI.ViewComponents
{
    public class _HeaderViewComponent : ViewComponent
    {
        // Bu metod, kullanıcının giriş yapıp yapmadığını kontrol eder
        public IViewComponentResult Invoke()
        {
            var claimsPrincipal = User as ClaimsPrincipal;

            var model = new HeaderUserViewModel
            {
                FullName = claimsPrincipal?.FindFirst("FirstName")?.Value + " " +
                           claimsPrincipal?.FindFirst("LastName")?.Value,

                Email = claimsPrincipal?.FindFirst(ClaimTypes.Email)?.Value ?? "",

                PhotoUrl = "/assets/images/avtar/woman.jpg" // Gerçek veri varsa claim'e ekle
            };

            return View("Components/Header/_HeaderComponent.cshtml", model);
        }
    }
}
