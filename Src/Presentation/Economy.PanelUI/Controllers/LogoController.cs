using Economy.Panel.Application.Dtos.AppSettingLogoDtos;
using Economy.Panel.Application.Interfaces;
using Economy.Panel.UI.Models.SettingLogoViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Economy.Panel.UI.Controllers
{
    public class LogoController : BaseController
    {
        private readonly IPanelAppSettingLogoService _panelAppSettingLogoService;
        private readonly IWebHostEnvironment _environment;

        public LogoController(IPanelAppSettingLogoService panelAppSettingLogoService, IWebHostEnvironment environment)
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
            if (!ModelState.IsValid)
                return View(model);

            string wwwRootPath = _environment.WebRootPath;
            string uploadFolder = Path.Combine(wwwRootPath, "uploads");

            if (!Directory.Exists(uploadFolder))
                Directory.CreateDirectory(uploadFolder);

            string SaveBase64Image(string? base64String, string fileNamePrefix)
            {
                if (string.IsNullOrEmpty(base64String) || !base64String.StartsWith("data:image"))
                    return null;

                var base64Data = base64String.Substring(base64String.IndexOf(",") + 1);
                byte[] imageBytes = Convert.FromBase64String(base64Data);

                string fileName = $"{fileNamePrefix}_{Guid.NewGuid()}.png";
                string filePath = Path.Combine(uploadFolder, fileName);

                System.IO.File.WriteAllBytes(filePath, imageBytes);

                return $"/uploads/{fileName}";
            }

            model.LogoPath = SaveBase64Image(model.CroppedLogoBase64, "logo") ?? model.LogoPath;
            model.MobileLogoPath = SaveBase64Image(model.CroppedMobileLogoBase64, "mobilelogo") ?? model.MobileLogoPath;
            model.FaviconPath = SaveBase64Image(model.CroppedFaviconBase64, "favicon") ?? model.FaviconPath;


           var result = _panelAppSettingLogoService.CreateEditAppSettingLogo(new AppSettingLogoCreateEditDto
            {
                Id = model.Id,
                LogoPath = model.LogoPath,
                MobileLogoPath = model.MobileLogoPath,
                FaviconPath = model.FaviconPath
            });
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
