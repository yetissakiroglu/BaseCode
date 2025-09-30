using Economy.Application.TenantUI.Dtos.AppSettingLogoDtos;
using Economy.Application.TenantUI.Interfaces;
using Economy.Panel.UI.Areas.Tenant.Models.AppSettingLogoViewModels;
using Economy.Panel.UI.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Economy.Panel.UI.Areas.Tenant.Controllers
{
    [Area("Tenant")]
    [Authorize]
    public class LogosController : BaseController
    {
        private readonly IPanelAppSettingLogoService _panelAppSettingLogoService;
        public LogosController(IPanelAppSettingLogoService panelAppSettingLogoService)
        {
            _panelAppSettingLogoService = panelAppSettingLogoService;
        }

        public IActionResult Index()
        {
            var result = _panelAppSettingLogoService.GetAppSettingLogo(false);
            var resultDto = new AppSettingLogoCreateEditViewModel()
            {
                FaviconPath = result.Data.FaviconPath,
                LogoPath = result.Data.LogoPath,
                MobileLogoPath = result.Data.MobileLogoPath,
                ShareImagePath = result.Data.ShareImagePath,
                Id = result.Data.Id
            };
            return View(resultDto);
        }
        public IActionResult CreateEditLogo(AppSettingLogoCreateEditViewModel model)
        {
            var result = _panelAppSettingLogoService.CreateEditAppSettingLogo(new AppSettingLogoCreateEditDto
            {
                Id = model.Id,
                LogoPath = model.LogoPath,
                MobileLogoPath = model.MobileLogoPath,
                FaviconPath = model.FaviconPath,
                FaviconBase64 = model.CroppedFaviconBase64,
                LogoBase64 = model.CroppedLogoBase64,
                MobileLogoBase64 = model.CroppedMobileLogoBase64,
                ShareImageBase64 =model.CroppedShareImageBase64,
                ShareImagePath = model.ShareImagePath
            });

            AddValidationErrorsToModelState(result.ValidationErrors);
            AddMessage(result);
            return RedirectToAction("Index");
        }

     }
}
