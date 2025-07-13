using Economy.Core.Dtos;
using Economy.Domain.Entites.Identities;
using Economy.Panel.Application.Interfaces;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;

namespace Economy.Panel.UI.Controllers
{
    public class AccountController : Controller
    {
        private readonly IPanelAppUserService _panelAppUserService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly UserManager<AppUser> _userManager;
        public AccountController(IPanelAppUserService panelAppUserService, IHttpContextAccessor httpContextAccessor, SignInManager<AppUser> signInManager, UserManager<AppUser> userManager)
        {
            _panelAppUserService = panelAppUserService;
            _httpContextAccessor = httpContextAccessor;
            _signInManager = signInManager;
            _userManager = userManager;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        public IActionResult Test()
        {
            return Content("Oturum açık: " + User.Identity.Name);
        }
        [HttpPost]
        public async Task<IActionResult> Login(SignIn model, string returnUrl = null)
        {
            if (!ModelState.IsValid)
                return View(model);

            var user = await _userManager.FindByNameAsync(model.Email);
            if (user == null)
            {
                ModelState.AddModelError("", "Kullanıcı bulunamadı");
                return View();
            }

            // Şifreyi kontrol et (isteğe bağlı)
            var passwordValid = await _userManager.CheckPasswordAsync(user, model.Password);
            if (!passwordValid)
            {
                ModelState.AddModelError("", "Şifre hatalı");
                return View();
            }

            // Roller çekiliyor
            var roles = await _userManager.GetRolesAsync(user);

            var claims = new List<Claim>
    {
        new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
        new Claim(ClaimTypes.Name, user.UserName ?? ""),
        new Claim(ClaimTypes.Email, user.Email ?? ""),
        new Claim("FirstName", user.FirstName ?? ""),
        new Claim("LastName", user.LastName ?? "")
    };

            // Roller claim olarak ekleniyor
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var identity = new ClaimsIdentity(claims, IdentityConstants.ApplicationScheme);
            var principal = new ClaimsPrincipal(identity);

            await HttpContext.SignInAsync(
                IdentityConstants.ApplicationScheme,
                principal,
                new AuthenticationProperties
                {
                    IsPersistent = model.RememberMe,
                    ExpiresUtc = DateTimeOffset.UtcNow.AddDays(7)
                });

            return RedirectToLocal(returnUrl);
        }

        // POST: Account/Logout

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
