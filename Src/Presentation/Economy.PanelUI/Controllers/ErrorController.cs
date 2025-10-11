using Economy.PanelUI.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

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

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
