using Economy.Application.AdminUI.Dtos.AppGeneralSettingDtos;
using Economy.Application.AdminUI.Interfaces;
using Economy.Application.Providers;
using Economy.Core.Core;
using Economy.Panel.UI.Controllers;
using Economy.Panel.UI.Models.GeneralSettingsPageViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Org.BouncyCastle.Ocsp;

namespace Economy.Panel.UI.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class SystemSettingsController : BaseController
    {
        private readonly IPanelAppGeneralSettingService _service;
        private readonly IWebHostEnvironment _env;
        private readonly IAppSettingsProvider _appSettingsProvider;
        private readonly IImageStorage _storage;

        public SystemSettingsController(IPanelAppGeneralSettingService service, IWebHostEnvironment env, IAppSettingsProvider appSettingsProvider, IImageStorage storage)
        {
            _service = service;
            _env = env;
            _appSettingsProvider = appSettingsProvider;
            _storage = storage;
        }

        [HttpGet]
        public async Task<IActionResult> General()
        {
            var result = await _service.GetGeneralSettingAsync();
            if (!result.HasData)
            {
                AddMessage(result);
                return View(result);
            }
            var vm = new GeneralSettingsPageViewModel
            {
                Id = result.Data.Id,
                SiteName = result.Data.SiteName,
                Domain = result.Data.Domain,
                Theme = result.Data.Theme,
                LogoUrl = result.Data.LogoUrl,
                MetaTitleSuffix = result.Data.MetaTitleSuffix,
                DefaultMetaDescription = result.Data.DefaultMetaDescription
            };
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> General(GeneralSettingsPageViewModel vm, CancellationToken ct)
        {
            if (!ModelState.IsValid) return View(vm);


            // Logo dosyası geldiyse kaydet ve LogoUrl set et
            if (vm.LogoFile is not null && vm.LogoFile.Length > 0)
            {
                var img = await _storage.UploadAsync("systemlogo", file: vm.LogoFile, aspect: null, ct);
                vm.LogoUrl = _storage.ToPublicUrl(img);
            }

            if (vm.Id.HasValue && vm.Id.Value > 0)
            {
                // UPDATE
                var editDto = new AppGeneralSettingEditDto
                {
                    Id = vm.Id.Value,
                    SiteName = vm.SiteName?.Trim() ?? "",
                    Domain = vm.Domain?.Trim(),
                    Theme = string.IsNullOrWhiteSpace(vm.Theme) ? "light" : vm.Theme.Trim(),
                    LogoUrl = vm.LogoUrl?.Trim(),
                    MetaTitleSuffix = vm.MetaTitleSuffix?.Trim(),
                    DefaultMetaDescription = vm.DefaultMetaDescription?.Trim()
                };

                var update = await _service.UpdateGeneralSettingAsync(editDto);
                if (!update.IsSuccess)
                {
                    ModelState.AddModelError(string.Empty, update.Message ?? "Güncelleme sırasında bir hata oluştu.");
                    return View(vm);
                }

                _appSettingsProvider.InvalidateGeneralCache();

                TempData["Success"] = "Genel ayarlar güncellendi.";
                return RedirectToAction(nameof(General));
            }
            else
            {
                // CREATE
                var createDto = new AppGeneralSettingCreateDto
                {
                    SiteName = vm.SiteName?.Trim() ?? "",
                    Domain = vm.Domain?.Trim(),
                    Theme = string.IsNullOrWhiteSpace(vm.Theme) ? "light" : vm.Theme.Trim(),
                    LogoUrl = vm.LogoUrl?.Trim(),
                    MetaTitleSuffix = vm.MetaTitleSuffix?.Trim(),
                    DefaultMetaDescription = vm.DefaultMetaDescription?.Trim()
                };

                var create = await _service.CreateGeneralSettingAsync(createDto);
                if (!create.IsSuccess)
                {
                    ModelState.AddModelError(string.Empty, create.Message ?? "Kayıt sırasında bir hata oluştu.");
                    return View(vm);
                }

                TempData["Success"] = "Genel ayarlar kaydedildi.";
                return RedirectToAction(nameof(General));
            }
        }

    }

}
