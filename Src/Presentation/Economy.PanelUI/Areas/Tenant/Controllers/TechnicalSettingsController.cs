using AutoMapper;
using Economy.Application.TenantUI.AppTechnicalSettingDtos;
using Economy.Application.TenantUI.Interfaces;
using Economy.Core.Extensions;
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
            var resModel = _mapper.Map<AppTechnicalSettingCreateEditDto>(res.Data);
            return View(resModel ?? new AppTechnicalSettingCreateEditDto());
        }

        [HttpPost("/Tenant/TechnicalSettings")]
        [ValidateAntiForgeryToken]
        public IActionResult TechnicalSettings(AppTechnicalSettingCreateEditDto model)
        {
            //model.MaintenanceAllowedIpList = MaintenanceAllowedIpListRaw.ToIpList();
       
            if (!ModelState.IsValid)
                return View(model);

            var res = _panelAppTechnicalSettingService.SaveAppTechnicalSetting(model);

            AddValidationErrorsToModelState(res.ValidationErrors);
            AddMessage(res);

            if (!res.IsSuccess)
            {
                return View(model);
            }

            return RedirectToAction(nameof(TechnicalSettings));
        }
    }
}
