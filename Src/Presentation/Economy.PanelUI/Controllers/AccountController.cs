using Economy.Core.Dtos;
using Economy.Domain.Entites.Identities;
using Economy.Panel.Application.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Economy.Panel.UI.Controllers
{
    public class AccountController : Controller
    {
        private readonly IPanelAppUserService _panelAppUserService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly SignInManager<AppUser> _signInManager;

        public AccountController(IPanelAppUserService panelAppUserService, IHttpContextAccessor httpContextAccessor, SignInManager<AppUser> signInManager)
        {
            _panelAppUserService = panelAppUserService;
            _httpContextAccessor = httpContextAccessor;
            _signInManager = signInManager;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(SignIn model, string returnUrl = null)
        {
            if (!ModelState.IsValid)
                return View(model);

            // Kullanıcıyı doğrula
            var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, false);

            if (result.Succeeded)
            {
                // Yönlendirme yapılacak URL
                return RedirectToLocal(returnUrl);
            }

            // Hata mesajını loglayın
            if (result.IsLockedOut)
            {
                // Kullanıcı hesabı kilitliyse
                ModelState.AddModelError(string.Empty, "Hesabınız kilitlenmiş.");
            }
            else if (result.RequiresTwoFactor)
            {
                // İki faktörlü kimlik doğrulama gerekiyorsa
                ModelState.AddModelError(string.Empty, "İki faktörlü kimlik doğrulama gerekiyor.");
            }
            else
            {
                // Diğer tüm hatalar için genel bir mesaj
                ModelState.AddModelError(string.Empty, "Geçersiz giriş denemesi.");
            }

            return View(model);
        }

        // POST: Account/Logout
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            // Kullanıcıyı oturumdan çıkartıyoruz
            await _signInManager.SignOutAsync();

            // Çıkış işleminden sonra, kullanıcıyı ana sayfaya yönlendiriyoruz
            return RedirectToAction("Index", "Home");
        }


        private IActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }

        }
    }
}
