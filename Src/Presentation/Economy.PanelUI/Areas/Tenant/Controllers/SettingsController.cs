using Economy.Application.TenantUI.Dtos.AppSettingDtos;
using Economy.Application.TenantUI.Interfaces;
using Economy.Panel.UI.Controllers;
using Economy.Panel.UI.Models.SettingViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Economy.Panel.UI.Areas.Tenant.Controllers
{
    [Area("Tenant")]
    [Authorize]
    public class SettingsController : BaseController
    {
        private readonly IPanelAppSettingService _panelAppSettingService;
        private readonly IPanelAppLanguageService _panelAppLanguageService;
        public SettingsController(IPanelAppSettingService panelAppSettingService, IPanelAppLanguageService panelAppLanguageService)
        {
            _panelAppSettingService = panelAppSettingService;
            _panelAppLanguageService = panelAppLanguageService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var allLanguages = _panelAppLanguageService.GetAllLanguage(false, true);
            if(!allLanguages.HasData)
            {
                AddMessage(allLanguages);
                return View(new AppSettingViewModel());
            }

            var appSetting = _panelAppSettingService.GetAppSetting(false);

            // DİL VARSA: her dil için mevcut çeviriyi (varsa) eşleştir
            var translations = allLanguages.Data.Select(lang =>
            {
                var existing = appSetting?.Data?.Translations?
                    .FirstOrDefault(p => p.AppLanguageId == lang.Id);

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
                    // Çeviride yoksa bile AppSetting Id’sini ver; o da yoksa 0
                    AppSettingId = existing?.AppSettingId ?? (appSetting?.Data?.Id ?? 0),
                    MetaTitle = existing?.MetaTitle,
                    SiteTitle = existing?.SiteTitle
                };
            }).ToList();

            var vm = new AppSettingViewModel
            {
                Id = appSetting?.Data?.Id ?? 0,
                Translations = translations
            };
            return View(vm);
        }

        [HttpPost]
        public IActionResult SaveSetting(AppSettingViewModel viewModel)
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

