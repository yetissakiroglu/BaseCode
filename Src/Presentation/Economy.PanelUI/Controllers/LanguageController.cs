using Microsoft.AspNetCore.Mvc;

namespace Economy.Panel.UI.Controllers
{
    public class LanguageController : BaseController
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
