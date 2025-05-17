using Economy.Panel.Application.Interfaces;
using Economy.Panel.UI.Models.SlideViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Economy.Panel.UI.Controllers
{
    public class SlideController : BaseController
    {
        private readonly IPanelAppSlideService _panelAppSlideService;

        public SlideController(IPanelAppSlideService panelAppSlideService)
        {
            _panelAppSlideService = panelAppSlideService;
        }

        public IActionResult Index()
        {
            var result = _panelAppSlideService.GetAllSlide(false);

            var resultModel = result.Data.Select()


        }
    }
}
