using Economy.Application.AdminUI.Interfaces;
using Economy.Domain.Entites.AdminEntity.EntityAppUsers;
using Economy.Panel.UI.Models.ProfileViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Economy.Panel.UI.Controllers
{
    public class ProfileController : BaseController
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IPanelSuperAdminService _panelSuperAdminService;

        public ProfileController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, IPanelSuperAdminService panelSuperAdminService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _panelSuperAdminService = panelSuperAdminService;
        }

        public async Task<IActionResult> Index()
        {

            var profil = await _panelSuperAdminService.GetUserAsync(CurrentUserId);
            if (!profil.HasData)
            {
                AddMessage(profil);
                return View(profil.Data);
            }
            var resılt = new UserProfileViewModel()
            {
                Email = profil?.Data?.Email,
                FullName = $"{profil?.Data?.FirstName} {profil?.Data?.LastName}",
                JobTitle = profil?.Data?.JobTitle,
                Phone = profil?.Data?.PhoneNumber,
                Roles = profil?.Data?.RolesName
            };
            return View(resılt);       
        }


        [HttpGet]
        public IActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var result = await _userManager.ChangePasswordAsync(user, model.CurrentPassword, model.NewPassword);
            if (result.Succeeded)
            {
                await _signInManager.RefreshSignInAsync(user);
                TempData["SuccessMessage"] = "Şifreniz başarıyla değiştirildi.";
                return RedirectToAction("ChangePassword");
            }

            foreach (var error in result.Errors)
                ModelState.AddModelError(string.Empty, error.Description);

            return View(model);
        }

        public IActionResult Settings()
        {
            return View();
        }
        public IActionResult Faq()
        {
            return View();
        }
        


    }
}
