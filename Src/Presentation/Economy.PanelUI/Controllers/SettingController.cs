using Microsoft.AspNetCore.Mvc;

namespace Economy.Panel.UI.Controllers
{
    public class SettingController : BaseController
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
