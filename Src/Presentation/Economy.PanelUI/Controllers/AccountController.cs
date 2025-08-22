using Economy.Application.Interfaces;
using Economy.Core.Dtos;
using Economy.Domain.Entites.Identities;
using Economy.Panel.Application.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;
using System.Security.Claims;

namespace Economy.Panel.UI.Controllers
{
    public class AccountController : Controller
    {
        private readonly IPanelAppUserService _panelAppUserService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly UserManager<AppUser> _userManager;
        private readonly IAuditLogWriter _audit;

        public AccountController(IPanelAppUserService panelAppUserService, IHttpContextAccessor httpContextAccessor, SignInManager<AppUser> signInManager, UserManager<AppUser> userManager, IAuditLogWriter audit)
        {
            _panelAppUserService = panelAppUserService;
            _httpContextAccessor = httpContextAccessor;
            _signInManager = signInManager;
            _userManager = userManager;
            _audit = audit;
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
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(SignIn model, string? returnUrl = null)
        {
            if (!ModelState.IsValid) return View(model);

            // Kullanıcıyı bul (log’a UserId düşebilmek için)
            var user = await _userManager.FindByNameAsync(model.Email)
                       ?? await _userManager.FindByEmailAsync(model.Email);

            // lockoutOnFailure = true -> Identity AccessFailedCount artırır, gerekiyorsa kilitler
            var result = await _signInManager.PasswordSignInAsync(
                userName: model.Email,
                password: model.Password,
                isPersistent: model.RememberMe,
                lockoutOnFailure: true);

            if (result.Succeeded)
            {
                await _audit.LogLoginAsync(user, model.Email, true, "Login succeeded", HttpContext, 200);
                return Redirect(returnUrl ?? "/");
            }

            if (result.RequiresTwoFactor)
            {
                // Başarısız değil ama tamamlanmadı: 2FA gerekli
                await _audit.LogLoginAsync(user, model.Email, false, "Requires two-factor authentication", HttpContext, 401);
                return RedirectToAction(nameof(LoginWith2fa), new { returnUrl, rememberMe = model.RememberMe });
            }

            if (result.IsLockedOut)
            {
                await _audit.LogLoginAsync(user, model.Email, false, "User locked out", HttpContext, 423); // 423 Locked
                ModelState.AddModelError(string.Empty, "Hesabınız geçici olarak kilitlendi.");
                return View(model);
            }

            // Geçersiz kullanıcı/parola
            await _audit.LogLoginAsync(user, model.Email, false, "Invalid credentials", HttpContext, 401);
            ModelState.AddModelError(string.Empty, "Kullanıcı adı veya parola hatalı.");
            return View(model);
        }


    //    [HttpPost]
    //    public async Task<IActionResult> Login(SignIn model, string returnUrl = null)
    //    {
    //        if (!ModelState.IsValid)
    //            return View(model);

    //        var user = await _userManager.FindByNameAsync(model.Email);
    //        if (user == null)
    //        {
    //            ModelState.AddModelError("", "Kullanıcı bulunamadı");
    //            return View();
    //        }

    //        // Şifreyi kontrol et (isteğe bağlı)
    //        var passwordValid = await _userManager.CheckPasswordAsync(user, model.Password);
    //        if (!passwordValid)
    //        {
    //            ModelState.AddModelError("", "Şifre hatalı");
    //            return View();
    //        }

    //        // Roller çekiliyor
    //        var roles = await _userManager.GetRolesAsync(user);

    //        var claims = new List<Claim>
    //{
    //    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
    //    new Claim(ClaimTypes.Name, user.UserName ?? ""),
    //    new Claim(ClaimTypes.Email, user.Email ?? ""),
    //    new Claim("FirstName", user.FirstName ?? ""),
    //    new Claim("LastName", user.LastName ?? "")
    //};

    //        // Roller claim olarak ekleniyor
    //        foreach (var role in roles)
    //        {
    //            claims.Add(new Claim(ClaimTypes.Role, role));
    //        }

    //        var identity = new ClaimsIdentity(claims, IdentityConstants.ApplicationScheme);
    //        var principal = new ClaimsPrincipal(identity);

    //        await HttpContext.SignInAsync(
    //            IdentityConstants.ApplicationScheme,
    //            principal,
    //            new AuthenticationProperties
    //            {
    //                IsPersistent = model.RememberMe,
    //                ExpiresUtc = DateTimeOffset.UtcNow.AddDays(7)
    //            });

    //        return RedirectToLocal(returnUrl);
    //    }

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
