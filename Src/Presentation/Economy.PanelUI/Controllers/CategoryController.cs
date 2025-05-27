using Economy.Panel.Application.Interfaces;
using Economy.Panel.Persistence.Services;
using Economy.Panel.UI.Extensions;
using Economy.Panel.UI.Models.CategoryViewModels;
using Economy.Panel.UI.Models.SlideViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Economy.Panel.UI.Controllers
{
    public class CategoryController : BaseController
    {
        private readonly IPanelAppCategoryService _panelAppCategoryService;
        private readonly IPanelAppLanguageService _panelAppLanguageService;

        public CategoryController(IPanelAppCategoryService panelAppCategoryService, IPanelAppLanguageService panelAppLanguageService)
        {
            _panelAppCategoryService = panelAppCategoryService;
            _panelAppLanguageService = panelAppLanguageService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var allLanguages = _panelAppLanguageService.GetAllLanguage(false, true);
            if (!allLanguages.HasData)
            {
                AddMessage(allLanguages);
                return View(new List<AppCategoryListViewModel>());
            }

            var result = _panelAppCategoryService.GetAllCategories(false);
            if (!result.HasData)
            {
                AddMessage(result);
                return View(result.Data);
            }
            var resultModel = result.Data.MapToListViewModel(allLanguages.Data);

            return View(resultModel);
        }
        [HttpGet]
        public IActionResult Edit(int Id)
        {
            var allLanguages = _panelAppLanguageService.GetAllLanguage(false, true);
            if (!allLanguages.HasData)
            {
                AddMessage(allLanguages);
                return RedirectToAction(nameof(Index));
            }

            var result = _panelAppCategoryService.GetCategory(Id, false);
            if (!result.HasData)
            {
                AddMessage(result);
                return RedirectToAction(nameof(Index));
            }

            var resultModel = result.Data.MapToEditViewModel(allLanguages.Data);

            return View(resultModel);
        }

    }
}
