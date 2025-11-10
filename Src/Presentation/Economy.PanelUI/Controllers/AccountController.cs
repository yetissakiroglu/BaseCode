using Economy.Application.AdminUI.Dtos.AppAccountDtos;
using Economy.Application.AdminUI.Interfaces;
using Economy.Domain.Entites.AdminEntity.EntityApp;
using Economy.Persistence.Contexts;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Economy.Panel.UI.Controllers
{
    public class AccountController : BaseController
    {
        private readonly IPanelAppAccountService _panelAppAccountService;
        private readonly DefaultDbContext _defaultDb;

        public AccountController(IPanelAppAccountService panelAppAccountService, DefaultDbContext defaultDb)
        {
            _panelAppAccountService = panelAppAccountService;
            _defaultDb = defaultDb;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(AppSignInDto model, string? returnUrl = null)
        {
            var loginResult = await _panelAppAccountService.LoginAsync(model, returnUrl, HttpContext);

            AddValidationErrorsToModelState(loginResult.ValidationErrors);
            AddMessage(loginResult);

            if (!loginResult.IsSuccess)
                return View(model);

            var target = RoleLandingUrl(CurrentUserRoles);
            var redirectUrl = string.IsNullOrWhiteSpace(loginResult.RedirectUrl) ? target : loginResult.RedirectUrl;

            // Güncel principal
            var authResult = await HttpContext.AuthenticateAsync(IdentityConstants.ApplicationScheme);
            var principal = authResult?.Principal ?? HttpContext.User;

            if (principal?.Identity?.IsAuthenticated == true && principal.IsInRole("Tenant Admin"))
            {
                var userIdStr = principal.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (!int.TryParse(userIdStr, out var userId))
                    return Redirect(redirectUrl);

                var appIds = await _defaultDb.Set<AppManager>()
                    .Where(m => m.UserId == userId)
                    .Select(m => m.AppId)
                    .ToListAsync();

                if (appIds.Count == 0)
                {
                    await HttpContext.SignOutAsync(IdentityConstants.ApplicationScheme);
                    TempData["Error"] = "Bu hesap için atanmış bir otel bulunamadı.";
                    return RedirectToAction(nameof(Login));
                }

                var identity = (ClaimsIdentity)principal.Identity!;
                var existing = identity.FindFirst("appId");
                if (existing != null) identity.RemoveClaim(existing);

                if (appIds.Count == 1)
                {
                    identity.AddClaim(new Claim("appId", appIds[0].ToString()));
                    await HttpContext.SignInAsync(
                        IdentityConstants.ApplicationScheme,
                        new ClaimsPrincipal(identity),
                        new AuthenticationProperties { IsPersistent = true }
                    );

                    if (!string.IsNullOrWhiteSpace(returnUrl) &&
                        returnUrl.StartsWith("/tenant/", StringComparison.OrdinalIgnoreCase))
                        return Redirect(returnUrl);

                    return Redirect("/tenant");
                }

                return Redirect("/tenant/select");
            }

            return Redirect(redirectUrl);
        }

        public async Task<IActionResult> Logout()
        {
            var logoutResult = await _panelAppAccountService.LogoutAsync(HttpContext);
            if (logoutResult.IsSuccess)
            {
                return Redirect("/");
            }
            AddMessage(logoutResult);
            return Redirect("/");
        }
    }
}
