using Economy.Application.Interfaces;
using Economy.Core.Dtos;
using Economy.Domain.Entites.Identities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Data;

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
  
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(SignIn model, string? returnUrl = null)
        {
            if (!ModelState.IsValid) return View(model);
            var loginResult = await _panelAppAccountService.LoginAsync(model, returnUrl, HttpContext, ModelState);
            if (loginResult.IsSuccess)
            {
                if (loginResult.Data.User.Roles.Contains("Super Admin"))
                    return RedirectToAction("Index", "Home", new { area = "Admin" });

                if (loginResult.Data.User.Roles.Contains("Tenant Admin"))
                    return RedirectToAction("Index", "Home", new { area = "Tenant" });

                return Redirect(loginResult.RedirectUrl);
            }
            return View(model);
        }

        public async Task<IActionResult> Logout()
        {
             var logoutResult = await _panelAppAccountService.LogoutAsync(HttpContext);
            if (logoutResult.IsSuccess) {
                return Redirect(logoutResult.RedirectUrl);
            }
            return Redirect("/");
        }
    }
}
