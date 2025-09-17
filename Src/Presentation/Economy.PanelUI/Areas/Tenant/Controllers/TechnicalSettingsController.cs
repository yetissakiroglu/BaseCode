using Economy.Panel.UI.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Economy.Panel.UI.Areas.Tenant.Controllers
{
    [Area("Tenant")]
    [Authorize]
    public class TechnicalSettingsController : BaseController
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
