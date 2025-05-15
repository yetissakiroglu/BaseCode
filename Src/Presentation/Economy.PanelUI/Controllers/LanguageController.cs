using Economy.Panel.Application.Dtos.AppLanguageDtos;
using Economy.Panel.Application.Interfaces;
using Economy.Panel.UI.Models.LanguageViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Economy.Panel.UI.Controllers
{
    public class LanguageController : BaseController
    {
        private readonly IPanelAppLanguageService _panelAppLanguageService;

        public LanguageController(IPanelAppLanguageService panelAppLanguageService)
        {
            _panelAppLanguageService = panelAppLanguageService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var result = _panelAppLanguageService.GetAllLanguage(false);

            var resultModel = result.Data.Select(lang => new AppLanguageListViewModel
            {
                Id = lang.Id,
                Name = lang.Name,
                Code = lang.Code,
                Icon = lang.Icon,
                IsActive = lang.IsActive,
                IsDefault = lang.IsDefault,
                IsRTL = lang.IsRTL
            });

            return View(resultModel);
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var result = _panelAppLanguageService.GetLanguage(id, false);
            if (!result.IsSuccess || result.Data == null)
            {
                AddMessage(result); // varsa hata mesajı göster
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
            if (!ModelState.IsValid)
            {
                return View(viewModel);
            }

            var editModel = new AppLanguageEditDto
            {
                Id = viewModel.Id,
                Name = viewModel.Name,
                Code = viewModel.Code,
                Icon = viewModel.Icon,
                IsActive = viewModel.IsActive,
                IsDefault = viewModel.IsDefault,
                IsRTL = viewModel.IsRTL
            };

            var editResult = _panelAppLanguageService.EditLanguage(editModel);
            AddMessage(editResult);

            if (!editResult.IsSuccess)
            {
                return View(viewModel); // Hatalıysa form tekrar gösterilsin
            }

            return RedirectToAction("Index"); // Başarılıysa listeye dön
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
            if (!ModelState.IsValid)
            {
                return View(viewModel); // Hatalıysa tekrar form gösterilir
            }

            var createDto = new AppLanguageCreateDto
            {
                Name = viewModel.Name,
                Code = viewModel.Code,
                Icon = viewModel.Icon,
                IsActive = viewModel.IsActive,
                IsDefault = viewModel.IsDefault,
                IsRTL = viewModel.IsRTL
            };

            var result = _panelAppLanguageService.CreateLanguage(createDto);
            AddMessage(result);

            if (!result.IsSuccess)
            {
                return View(viewModel); // Hata varsa tekrar göster
            }

            return RedirectToAction("Index"); // Başarılıysa listeye dön
        }

        [HttpGet]
        public IActionResult Details(int id)
        {
            var result = _panelAppLanguageService.GetLanguage(id, false);

            if (!result.IsSuccess || result.Data == null)
            {
                AddMessage(result); // Hata mesajı göster
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
            if (result.IsSuccess)
            {
                result.Message.RedirectUrl = "/Language/" + nameof(Index);
            }
            AddMessage(result);

            return Json(result);
        }



    }
}
