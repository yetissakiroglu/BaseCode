using Economy.Core.Dtos;
using Economy.Panel.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Economy.Panel.UI.Controllers
{
    public class AccountController : Controller
    {
        private readonly IPanelAppUserService _panelAppUserService;
        public AccountController(IPanelAppUserService panelAppUserService)
        {
            _panelAppUserService = panelAppUserService;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(SignIn model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var result = await _panelAppUserService.LoginAsync(model);

            if (!result.IsSuccess)
            {
                ModelState.AddModelError(string.Empty,"");
                return View(model);
            }

            // Giriş başarılıysa yönlendirme yapılabilir
            return RedirectToAction("Index", "Home");
        }
    }
}
