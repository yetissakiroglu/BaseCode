using Economy.Panel.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Economy.Panel.UI.Controllers
{
    public class PageController : Controller
    {
        private readonly IPanelAppContentService _panelAppContentService;

        public PageController(IPanelAppContentService panelAppContentService)
        {
            _panelAppContentService = panelAppContentService;
        }

        public IActionResult Index()
        {
            var result = _panelAppContentService.GetAllContent(false);
            return View();
        }
    }
}
