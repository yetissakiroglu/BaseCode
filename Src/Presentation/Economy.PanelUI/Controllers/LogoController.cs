using Economy.Panel.Application.Interfaces;
using Economy.Panel.UI.Models.SettingLogoViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Economy.Panel.UI.Controllers
{
    public class LogoController : BaseController
    {
        private readonly IPanelAppSettingLogoService _panelAppSettingLogoService;

        public LogoController(IPanelAppSettingLogoService panelAppSettingLogoService)
        {
            _panelAppSettingLogoService = panelAppSettingLogoService;
        }

        public IActionResult Index()
        {
            var result = _panelAppSettingLogoService.GetAppSettingLogo(false);

            var resultDto = new AppSettingLogoCreateEditViewModel()
            {
                FaviconPath = result.Data.FaviconPath,
                LogoPath = result.Data.LogoPath,
                MobileLogoPath = result.Data.MobileLogoPath,
                Id = result.Data.Id
            };

            return View(resultDto);
        }


        public IActionResult CreateEditLogo()
        {
            return View();
        }
    }
}
