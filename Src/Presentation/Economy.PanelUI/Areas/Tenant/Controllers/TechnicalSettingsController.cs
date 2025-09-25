using AutoMapper;
using Economy.Application.Dtos.AppTechnicalSettingDtos;
using Economy.Application.Interfaces;
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
        private readonly IMapper _mapper;

        public TechnicalSettingsController(IPanelAppTechnicalSettingService panelAppTechnicalSettingService, IMapper mapper)
        {
            _panelAppTechnicalSettingService = panelAppTechnicalSettingService;
            _mapper = mapper;
        }

        public IActionResult Index()
        {
            var res = _panelAppTechnicalSettingService.GetAppTechnicalSetting(false);

            var entity = _mapper.Map<AppTechnicalSettingCreateEditDto>(res.Data);

            return View(entity ?? new AppTechnicalSettingCreateEditDto());
        }

        [HttpPost("/Tenant/TechnicalSettings")]
        [ValidateAntiForgeryToken]
        public IActionResult TechnicalSettings(AppTechnicalSettingCreateEditDto model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var res = _panelAppTechnicalSettingService.SaveAppTechnicalSetting(model);

            AddValidationErrorsToModelState(res.ValidationErrors);
            AddMessage(res);

            if (!res.IsSuccess)
            {
                TempData["Error"] = res.Message ?? "Kayıt sırasında hata oluştu.";
                return View(model);
            }

            TempData["Success"] = "Ayarlar kaydedildi.";
            return RedirectToAction(nameof(TechnicalSettings));
        }
    }
}
