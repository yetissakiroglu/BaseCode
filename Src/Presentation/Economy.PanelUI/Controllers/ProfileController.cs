using Economy.Domain.Entites.Identities;
using Economy.Panel.Application.Interfaces;
using Economy.Panel.UI.Models.ProfileViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Economy.Panel.UI.Controllers
{
    public class ProfileController : BaseController
    {
        private readonly IPanelAppUserService _panelAppUserService;
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;


        public ProfileController(IPanelAppUserService panelAppUserService, UserManager<AppUser> userManager, SignInManager<AppUser> signInManager)
        {
            _panelAppUserService = panelAppUserService;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public async Task<IActionResult> Index()
        {

            var profil = await _panelAppUserService.GetUser(CurrentUserId, false);
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
                PhotoUrl = profil?.Data?.PhotoUrl,
                Roles = profil?.Data?.Roles?.ToList() ?? new List<string>()
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
