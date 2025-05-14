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
        public IActionResult Edit(AppLanguageEditViewModel viewModel)
        {

            return View();
        }

        [HttpGet]
        public IActionResult Create()
        {


            return View();
        }

        [HttpPost]
        public IActionResult Create(AppLanguageCreateViewModel viewModel)
        {


            return View();
        }

        [HttpGet]
        public IActionResult Details(int id)
        {
            var result = _panelAppLanguageService.GetLanguage(id, false);
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
        public IActionResult Delete(int id)
        {

            return View();
        }
        



    }
}
