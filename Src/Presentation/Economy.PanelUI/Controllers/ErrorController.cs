using Microsoft.AspNetCore.Mvc;

namespace Economy.Panel.UI.Controllers
{
    public class ErrorController : BaseController
    {
        [Route("Error/Forbidden")]
        public IActionResult Forbidden(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }
        public IActionResult DbContextNotInitialized()
        {
            return View();
        }
        public IActionResult General()
        {
            return View();
        }
    }
}
