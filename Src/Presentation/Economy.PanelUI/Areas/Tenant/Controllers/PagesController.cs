using Economy.Panel.Application.Interfaces;
using Economy.Panel.UI.Controllers;
using Economy.Panel.UI.Extensions;
using Economy.Panel.UI.Models.ContentViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Economy.Panel.UI.Areas.Tenant.Controllers
{
    [Area("Tenant")]
    [Authorize]
    public class PagesController : BaseController
    {
        private readonly IPanelAppContentService _panelAppContentService;
        private readonly IPanelAppLanguageService _panelAppLanguageService;

        public PagesController(IPanelAppContentService panelAppContentService, IPanelAppLanguageService panelAppLanguageService)
        {
            _panelAppContentService = panelAppContentService;
            _panelAppLanguageService = panelAppLanguageService;
        }

        [HttpGet]
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
        [HttpGet]
        public IActionResult Create()
        {
            var allLanguages = _panelAppLanguageService.GetAllLanguage(false, true);
            if (!allLanguages.HasData)
            {
                AddMessage(allLanguages);
                return RedirectToAction(nameof(Index));
            }
            var viewModel = new AppContentCreateEditViewModel();
            viewModel.ToEmptyCreateEditViewModel(allLanguages.Data);
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(AppContentCreateEditViewModel viewModel)
        {
            var dto = viewModel.MapToDto();
            var result = _panelAppContentService.CreateContent(dto);

            AddValidationErrorsToModelState(result.ValidationErrors);
            AddMessage(result);

            if (!result.IsSuccess)
            {
                return View(viewModel);
            }

            return RedirectToAction("Index");
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

            var result = _panelAppContentService.GetContent(Id, false);
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
        public IActionResult Edit(AppContentCreateEditViewModel viewModel)
        {
            var dto = viewModel.MapToDto();
            var result = _panelAppContentService.EditContent(dto);

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
