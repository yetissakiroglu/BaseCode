using Microsoft.AspNetCore.Mvc;

namespace AppWeb.Controllers
{
    public class PageController : BaseController
    {
        public IActionResult Index(string url, string lang)
        {
            return View();
        }
    }
}
