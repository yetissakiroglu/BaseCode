using Economy.Application.Dtos.AppTechnicalSettingDtos;
using Economy.Application.Interfaces;
using Economy.Panel.Application.Interfaces;
using Economy.Panel.UI.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Economy.Panel.UI.Areas.Tenant.Controllers
{
    [Area("Tenant")]
    [Authorize]
    public class TechnicalSettingsController : BaseController
    {
        private readonly IPanelAppTechnicalSettingService _panelAppTechnicalSettingService;

        public TechnicalSettingsController(IPanelAppTechnicalSettingService panelAppTechnicalSettingService)
        {
            _panelAppTechnicalSettingService = panelAppTechnicalSettingService;
        }

        public IActionResult Index()
        {
            var res = _panelAppTechnicalSettingService.GetAppTechnicalSetting(true);
            return View(res.Data ?? new AppTechnicalSettingDto());
        }

        [HttpPost("/Tenant/TechnicalSettings")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> TechnicalSettings(AppTechnicalSettingDto model)
        {
            if (!ModelState.IsValid)
                return View(model);

            //var res = await _panelAppTechnicalSettingService.SaveAppTechnicalSetting(model);
            //if (!res.IsSuccess)
            //{
            //    if (res.ValidationErrors != null)
            //        AddValidationErrorsToModelState(res.ValidationErrors); // senin helper’ın
            //    TempData["Error"] = res.Message ?? "Kayıt sırasında hata oluştu.";
            //    return View(model);
            //}

            TempData["Success"] = "Ayarlar kaydedildi.";
            return RedirectToAction(nameof(TechnicalSettings));
        }
    }
}
