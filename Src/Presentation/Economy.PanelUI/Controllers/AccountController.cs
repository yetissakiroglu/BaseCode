using Economy.Application.AdminUI.Dtos.AppAccountDtos;
using Economy.Application.AdminUI.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Economy.Panel.UI.Controllers
{
    public class AccountController : BaseController
    {
        private readonly IPanelAppAccountService _panelAppAccountService;

        public AccountController(IPanelAppAccountService panelAppAccountService)
        {
            _panelAppAccountService = panelAppAccountService;
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
            if (loginResult.IsSuccess)
            {
                var target = RoleLandingUrl(CurrentUserRoles);
                return Redirect(string.IsNullOrWhiteSpace(loginResult.RedirectUrl) ?  target : loginResult.RedirectUrl);
            }
            return View(model);
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
