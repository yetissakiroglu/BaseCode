using Economy.Panel.Application.Dtos.AppSettingLogoDtos;
using Economy.Panel.Application.Interfaces;
using Economy.Panel.UI.Controllers;
using Economy.Panel.UI.Models.SettingLogoViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Economy.Panel.UI.Areas.Tenant.Controllers
{
    [Area("Tenant")]
    [Authorize]
    public class LogosController : BaseController
    {
        private readonly IPanelAppSettingLogoService _panelAppSettingLogoService;
        private readonly IWebHostEnvironment _environment;

        public LogosController(IPanelAppSettingLogoService panelAppSettingLogoService, IWebHostEnvironment environment)
        {
            _panelAppSettingLogoService = panelAppSettingLogoService;
            _environment = environment;
        }

        public IActionResult Index()
        {
            var result = _panelAppSettingLogoService.GetAppSettingLogo(false);

            var resultDto = new AppSettingLogoCreateEditViewModel()
            {
                FaviconPath = result.Data.FaviconPath,
                LogoPath = result.Data.LogoPath,
                MobileLogoPath = result.Data.MobileLogoPath,
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
                MobileLogoBase64 = model.CroppedMobileLogoBase64
            });

            AddValidationErrorsToModelState(result.ValidationErrors);
            AddMessage(result);

            return RedirectToAction("Index");
        }

        // Varsayılan görseli döndüren yardımcı fonksiyon
        //private string GetOrDefaultLogo(string type)
        //{
        //    // Gerçek uygulamada ilgili veritabanı kaydı kontrol edilir
        //    var base64 = GetBase64FromDb(type); // bu sizin veritabanı kodunuz olmalı

        //    if (string.IsNullOrWhiteSpace(base64))
        //    {
        //        var defaultImagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/no-img.jpeg");
        //        var imageBytes = System.IO.File.ReadAllBytes(defaultImagePath);
        //        return $"data:image/jpeg;base64,{Convert.ToBase64String(imageBytes)}";
        //    }

        //    return base64;
        //}
    }
}
