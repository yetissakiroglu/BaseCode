using Economy.Core.Interfaces.Economy.Panel.Persistence.Services;
using Economy.Panel.Application.Interfaces;
using Economy.Panel.Persistence.Services;
using Economy.Panel.UI.Extensions;
using Economy.Panel.UI.Models.CategoryViewModels;
using Economy.Panel.UI.Models.ContentViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Economy.Panel.UI.Controllers
{
    public class PageController : BaseController
    {
        private readonly IPanelAppContentService _panelAppContentService;
        private readonly IPanelAppLanguageService _panelAppLanguageService;

        public PageController(IPanelAppContentService panelAppContentService, IPanelAppLanguageService panelAppLanguageService)
        {
            _panelAppContentService = panelAppContentService;
            _panelAppLanguageService = panelAppLanguageService;
        }

        public IActionResult Index()
        {
            var allLanguages = _panelAppLanguageService.GetAllLanguage(false, true);
            if (!allLanguages.HasData)
            {
                AddMessage(allLanguages);
                return View(new List<AppContentListViewModel>());
            }

            var result = _panelAppContentService.GetAllContent(false);
            if (!result.HasData)
            {
                AddMessage(result);
                return View(result.Data);
            }
            var resultModel = result.Data.MapToListViewModel(allLanguages.Data);

            return View(resultModel);
  
        }
    }
}
