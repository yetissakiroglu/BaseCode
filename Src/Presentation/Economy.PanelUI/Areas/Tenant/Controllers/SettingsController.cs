using Economy.Application.TenantUI.Dtos.AppPageDtos;
using Economy.Application.TenantUI.Dtos.AppSettingDtos;
using Economy.Application.TenantUI.Interfaces;
using Economy.Panel.UI.Areas.Tenant.Models;
using Economy.Panel.UI.Controllers;
using Economy.Panel.UI.Models.SettingViewModels;
using Economy.Persistence.Tenant.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

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
        public async Task<IActionResult> Index(CancellationToken ct)
        {
            var appSettingResult = await _panelAppSettingService.GetAppSettingAsync(false, ct);
            var vm = new AppSettingCreateEditDto();
            if (!appSettingResult.HasData)
            {
                await _panelAppSettingService.FillLanguagesAsync(vm, ct);
                return View(vm);
            }
            vm.Id = appSettingResult.Data.Id;
            await _panelAppSettingService.FillLanguagesAsync(vm, ct);
            foreach (var t in vm.Translations)
            {
                var hit = appSettingResult.Data.Translations.FirstOrDefault(x => x.AppLanguageId == t.AppLanguageId);
                if (hit is null) continue;
                t.Id = hit.Id;
                t.MetaTitle = hit.MetaTitle;
                t.MetaDescription = hit.MetaDescription;
                t.Description = hit.Description;
                t.Title = hit.Title;
                t.MetaSlogan = hit.MetaSlogan;
            }
            return View(vm);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> SaveSetting(AppSettingCreateEditDto viewModel, CancellationToken ct)
        {
            var result = await _panelAppSettingService.GetAppSettingAsync(false, ct);
            if (!result.HasData)
            {
                var createModel = await _panelAppSettingService.Create(viewModel, ct);
                AddValidationErrorsToModelState(result.ValidationErrors);
                AddMessage(result);
            }
            else
            {
                var editModel = await _panelAppSettingService.Edit(result.Data.Id, viewModel, ct);
                AddValidationErrorsToModelState(result.ValidationErrors);
                AddMessage(result);
            }
            return RedirectToAction(nameof(Index));
        }
    }
}

