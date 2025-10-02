using Economy.Application.TenantUI.Dtos.AppLanguageDtos;
using Economy.Application.TenantUI.Interfaces;
using Economy.Panel.UI.Controllers;
using Economy.Panel.UI.Models.LanguageViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Economy.Panel.UI.Areas.Tenant.Controllers
{
    [Area("Tenant")]
    [Authorize]
    public class LanguagesController : BaseController
    {
        private readonly IPanelAppLanguageService _panelAppLanguageService;

        public LanguagesController(IPanelAppLanguageService panelAppLanguageService)
        {
            _panelAppLanguageService = panelAppLanguageService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var result = _panelAppLanguageService.GetAllLanguage(isDeleted: false);

            if (!result.HasData)
            {
                AddMessage(result);
                return View(result.Data);
            }
            var resultModel = result.Data?.Select(lang => new AppLanguageListViewModel
            {
                Id = lang.Id,
                Name = lang.Name,
                Code = lang.Code,
                Icon = lang.Icon,
                IsActive = lang.IsActive,
                IsDefault = lang.IsDefault,
                IsRTL = lang.IsRTL
            }).ToList();

            return View(resultModel);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(AppLanguageCreateViewModel viewModel)
        {
            var createDto = new AppLanguageCreateEditDto
            {
                Name = viewModel.Name,
                Code = viewModel.Code,
                Icon = viewModel.Icon,
                IsActive = viewModel.IsActive,
                IsDefault = viewModel.IsDefault,
                IsRTL = viewModel.IsRTL
            };

            var result = _panelAppLanguageService.CreateLanguage(createDto);

            AddValidationErrorsToModelState(result.ValidationErrors);
            AddMessage(result);

            if (!result.IsSuccess)
            {
                return View(viewModel);
            }

            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var result = _panelAppLanguageService.GetLanguage(id, false);
            if (!result.HasData)
            {
                AddMessage(result);
                return RedirectToAction("Index");
            }

            var lang = result.Data;

            var resultModel = new AppLanguageEditViewModel
            {
                Id = lang.Id,
                Name = lang.Name,
                Code = lang.Code,
                Icon = lang.Icon,
                IsActive = lang.IsActive,
                IsDefault = lang.IsDefault,
                IsRTL = lang.IsRTL
            };

            return View(resultModel);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(AppLanguageEditViewModel viewModel)
        {
            var crudDto = new AppLanguageCreateEditDto
            {
                Id=viewModel.Id,
                Name = viewModel.Name,
                Code = viewModel.Code,
                Icon = viewModel.Icon,
                IsActive = viewModel.IsActive,
                IsDefault = viewModel.IsDefault,
                IsRTL = viewModel.IsRTL
            };

            var result = _panelAppLanguageService.EditLanguage(crudDto);

            AddValidationErrorsToModelState(result.ValidationErrors);
            AddMessage(result);

            if (!result.IsSuccess)
            {
                return View(viewModel);
            }

            return RedirectToAction("Index");
        }


        [HttpGet]
        public IActionResult Details(int id)
        {
            var result = _panelAppLanguageService.GetLanguage(id, false);
            if (!result.HasData)
            {
                AddMessage(result);
                return RedirectToAction("Index");
            }

            var lang = result.Data;

            var resultModel = new AppLanguageViewModel
            {
                Id = lang.Id,
                Name = lang.Name,
                Code = lang.Code,
                Icon = lang.Icon,
                IsActive = lang.IsActive,
                IsDefault = lang.IsDefault,
                IsRTL = lang.IsRTL
            };

            return View(resultModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
            var result = _panelAppLanguageService.DeleteLanguage(id);
            if (!result.HasData)
            {
                return Json(result);
            }

            result.RedirectUrl = "/Language";
            return Json(result);
        }



    }
}
