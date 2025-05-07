using Economy.Panel.Application.Dtos.AppDtos;
using Economy.Panel.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Economy.Panel.UI.Controllers
{
    public class AppController : BaseController
    {
        private readonly IPanelAppService _panelAppService;

        public AppController(IPanelAppService panelAppService)
        {
            _panelAppService = panelAppService;
        }

        public IActionResult AppList()
        {
            var result = _panelAppService.Apps(false);
            return View(result.Data);
        }

        [HttpGet]
        public IActionResult CreateApp()
        {
            return View(new AppCreateDto());
        }

        [HttpPost]
        public async Task<IActionResult> CreateApp(AppCreateDto model)
        {
            var result = await _panelAppService.CreateApp(model);
            AddMessage(result);
            return RedirectToAction(nameof(AppList));
        }

    }
}
