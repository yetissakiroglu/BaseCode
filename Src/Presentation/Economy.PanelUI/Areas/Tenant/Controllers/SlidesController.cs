using Economy.Panel.Application.Interfaces;
using Economy.Panel.UI.Controllers;
using Economy.Panel.UI.Extensions;
using Economy.Panel.UI.Models.SlideViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Economy.Panel.UI.Areas.Tenant.Controllers
{
    [Area("Tenant")]
    [Authorize]
    public class SlidesController : BaseController
    {
        private readonly IPanelAppSlideService _panelAppSlideService;
        private readonly IPanelAppLanguageService _panelAppLanguageService;

        public SlidesController(IPanelAppSlideService panelAppSlideService, IPanelAppLanguageService panelAppLanguageService)
        {
            _panelAppSlideService = panelAppSlideService;
            _panelAppLanguageService = panelAppLanguageService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var allLanguages = _panelAppLanguageService.GetAllLanguage(false, true);
            if (!allLanguages.HasData)
            {
                AddMessage(allLanguages);
                return View(new List<AppSlideListViewModel>());
            }

            var result = _panelAppSlideService.GetAllSlide(false);
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

            var result = _panelAppSlideService.GetSlide(Id, false);
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
        public IActionResult Edit(AppSlideCreateEditViewModel viewModel)
        {
            var dto = viewModel.MapToDto();
            var result = _panelAppSlideService.EditSlide(dto);

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
          
            var viewModel = allLanguages.Data.ToEmptyCreateEditViewModel();

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(AppSlideCreateEditViewModel viewModel)
        {
            var dto = viewModel.MapToDto();
            var result = _panelAppSlideService.CreateSlide(dto);

            AddValidationErrorsToModelState(result.ValidationErrors);
            AddMessage(result);

            if (!result.IsSuccess)
            {
                return View(viewModel);
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
            var result = _panelAppSlideService.DeleteSlide(id);
            if(result.IsSuccess)
            {
                result.RedirectUrl = "/Slide";
            }

            return Json(result);
        }

        [HttpGet]
        public IActionResult Details(int id)
        {
            var allLanguages = _panelAppLanguageService.GetAllLanguage(false, true);
            if (!allLanguages.HasData)
            {
                AddMessage(allLanguages);
                return RedirectToAction(nameof(Index));
            }

            var result = _panelAppSlideService.GetSlide(id, false);
            if (!result.HasData)
            {
                AddMessage(result);
                return RedirectToAction(nameof(Index));
            }

            var resultModel = result.Data.ToViewModel();

            return View(resultModel);
        }
    }

}
