using Economy.Panel.Application.Dtos.AppSettingDtos;
using Economy.Panel.Application.Interfaces;
using Economy.Panel.UI.Models.SettingViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Economy.Panel.UI.Controllers
{
    public class SettingController : BaseController
    {
        private readonly IPanelAppSettingService _panelAppSettingService;
        private readonly IPanelAppLanguageService _panelAppLanguageService;

        public SettingController(IPanelAppSettingService panelAppSettingService, IPanelAppLanguageService panelAppLanguageService)
        {
            _panelAppSettingService = panelAppSettingService;
            _panelAppLanguageService = panelAppLanguageService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var allLanguages = _panelAppLanguageService.GetAllLanguage(false, true);
      
            var appSetting = _panelAppSettingService.GetAppSetting(false);
            var translations = allLanguages.Data.Select(lang =>
            {
                var existing = appSetting.Data?.Translations?.FirstOrDefault(p => p.AppLanguageId == lang.Id);
                return new SettingLanguageViewModel
                {
                    Id = existing?.Id,
                    Code = lang.Code,
                    Icon = lang.Icon,
                    IsRTL = lang.IsRTL,
                    Name = lang.Name,
                    Description = existing?.Description,
                    MetaDescription = existing?.MetaDescription,
                    AppLanguageId = lang.Id,
                    AppSettingId = existing?.AppSettingId,
                    MetaTitle = existing?.MetaTitle,
                    SiteTitle = existing?.SiteTitle,
                };
            }).ToList();


            var vm = new SettingViewModel
            {
                Id = appSetting.Data.Id,
                Translations = translations
            };
            return View(vm);
        }

        [HttpPost]
        public IActionResult SaveSetting(SettingViewModel viewModel)
        {
            var createEditModel = new AppSettingCreateEditDto();
            createEditModel.Id = viewModel.Id;

            foreach (var translation in viewModel.Translations)
            {
                var existingLang = viewModel.Translations.FirstOrDefault(p => p.AppLanguageId == translation.Id);

                if (existingLang != null)
                {
                    createEditModel.Translations.Add(new AppSettingTranslationDto()
                    {
                        Description = translation.Description,
                        MetaDescription = translation.MetaDescription,
                        AppLanguageId = translation.AppLanguageId,
                        AppSettingId = translation.AppSettingId,
                        Id = translation.Id,
                        MetaTitle = translation.MetaTitle,
                        SiteTitle = translation.SiteTitle,

                    });
                }
                else
                {
                    createEditModel.Translations.Add(new AppSettingTranslationDto()
                    {
                        Description = translation.Description,
                        MetaDescription = translation.MetaDescription,
                        AppLanguageId = translation.AppLanguageId,
                        AppSettingId = translation.AppSettingId,
                        Id = translation.Id,
                        MetaTitle = translation.MetaTitle,
                        SiteTitle = translation.SiteTitle,

                    });
                }

            }

            var saveModel = _panelAppSettingService.SaveAppSetting(createEditModel);
            AddMessage(saveModel);
            return RedirectToAction(nameof(Index));


        }


    }
}

