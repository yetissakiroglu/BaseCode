using Economy.Application.AdminUI.Interfaces;
using Economy.Panel.UI.Models.ProfileViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Economy.Panel.UI.ViewComponents
{
    public class _HeaderViewComponent : ViewComponent
    {
        private readonly IPanelSuperAdminService _panelSuperAdminService;

        public _HeaderViewComponent(IPanelSuperAdminService panelAppUserService)
        {
            _panelSuperAdminService = panelAppUserService;
        }

        public IViewComponentResult Invoke()
        {
            var claimsPrincipal = User as ClaimsPrincipal;
            var CurrentUserId = claimsPrincipal?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var user = (_panelSuperAdminService.GetUserAsync(Convert.ToInt32(CurrentUserId))).Result;
            var model = new HeaderUserViewModel
            {
                FullName = user?.Data.FirstName + " " + user?.Data.LastName  ?? "Kullanıcı Adı", // Gerçek veri varsa claim'e ekle
                Email = user.Data.Email,
                PhotoUrl = "/assets/images/avtar/woman.jpg" // Gerçek veri varsa claim'e ekle
            };

            return View("Components/Header/_HeaderComponent.cshtml", model);
        }
    }
}
