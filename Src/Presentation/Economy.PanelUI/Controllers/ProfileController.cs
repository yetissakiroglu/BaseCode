using Economy.Panel.Application.Interfaces;
using Economy.Panel.UI.Models.ProfileViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Economy.Panel.UI.Controllers
{
    public class ProfileController : BaseController
    {
        private readonly IPanelAppUserService _panelAppUserService;
        public ProfileController(IPanelAppUserService panelAppUserService)
        {
            _panelAppUserService = panelAppUserService;
        }

        public IActionResult Index()
        {

            var profil = _panelAppUserService.GetUser(CurrentUserId, false);
            if (!profil.HasData)
            {
                AddMessage(profil);
                return View(profil.Data);
            }
            var resılt = new UserProfileViewModel()
            {
                Email = profil?.Data?.Email,
                FullName = profil?.Data?.FirstName + " " + profil?.Data?.LastName,
                JobTitle = profil?.Data?.JobTitle,
                Phone = profil?.Data?.PhoneNumber,
                PhotoUrl = profil?.Data?.PhotoUrl
            };
            return View(resılt);
        }
    }
}
