using Economy.Panel.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Economy.Panel.UI.Controllers
{
    public class SettingController : BaseController
    {
        private readonly IPanelAppSettingService _panelAppSettingService;

        public SettingController(IPanelAppSettingService panelAppSettingService)
        {
            _panelAppSettingService = panelAppSettingService;
        }

        public IActionResult Index()
        {
            var response = _panelAppSettingService.GetAppSetting(1, false);
            return View(response.Data);
        }
    }
}
