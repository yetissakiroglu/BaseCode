using Economy.Panel.Application.Interfaces;
using Economy.Panel.UI.Controllers;
using Economy.Panel.UI.Extensions;
using Economy.Panel.UI.Models.CategoryViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Economy.Panel.UI.Areas.Tenant.Controllers
{
    [Area("Tenant")]
    [Authorize]
    public class CategoriesController : BaseController
    {
        private readonly IPanelAppCategoryService _panelAppCategoryService;
        private readonly IPanelAppLanguageService _panelAppLanguageService;

        public CategoriesController(IPanelAppCategoryService panelAppCategoryService, IPanelAppLanguageService panelAppLanguageService)
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(AppCategoryCreateEditViewModel viewModel)
        {
            var dto = viewModel.MapToDto();
            var result = _panelAppCategoryService.EditCategory(dto);

            AddValidationErrorsToModelState(result.ValidationErrors);
            AddMessage(result);

            if (!result.IsSuccess)
            {
                return View(viewModel);
            }

            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Create()
        {
            var allLanguages = _panelAppLanguageService.GetAllLanguage(false, true);
            if (!allLanguages.HasData)
            {
                AddMessage(allLanguages);
                return RedirectToAction(nameof(Index));
            }
            var viewModel = new AppCategoryCreateEditViewModel();
            viewModel.ToEmptyCreateEditViewModel(allLanguages.Data);
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(AppCategoryCreateEditViewModel viewModel)
        {
            var dto = viewModel.MapToDto();
            var result = _panelAppCategoryService.CreateCategory(dto);

            AddValidationErrorsToModelState(result.ValidationErrors);
            AddMessage(result);

            if (!result.IsSuccess)
            {
                return View(viewModel);
            }

            return RedirectToAction("Index");
        }


    }
}
