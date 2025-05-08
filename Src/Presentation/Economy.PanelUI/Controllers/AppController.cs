using Economy.Panel.Application.Dtos.AppDtos;
using Economy.Panel.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

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

        [HttpGet]
        public IActionResult DetailsApp(int Id)
        {
            var result = _panelAppService.GetApp(Id, false);
            AddMessage(result);
            return View(result.Data);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteApp(int Id)
        {
            var result = _panelAppService.DeleteApp(Id);

            string json = JsonSerializer.Serialize(result, new JsonSerializerOptions { WriteIndented = true });

            return Json(result);
        }


    }
}
