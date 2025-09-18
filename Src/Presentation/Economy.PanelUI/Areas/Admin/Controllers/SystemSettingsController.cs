using Economy.Application.Dtos.AppGeneralSettingDtos;
using Economy.Application.Dtos.AppSecuritySettingDtos;
using Economy.Application.Interfaces;
using Economy.Application.Providers;
using Economy.Panel.UI.Controllers;
using Economy.Panel.UI.Models.GeneralSettingsPageViewModels;
using Economy.Panel.UI.Models.SecuritySettingsPageViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Economy.Panel.UI.Areas.Admin.Controllers
{

    [Area("Admin")]
    [Authorize]
    //[Route("[area]/[controller]")]
    public class SystemSettingsController : BaseController
    {
        private readonly IPanelAppGeneralSettingService _service;
        private readonly IWebHostEnvironment _env;
        private readonly IPanelAppSecuritySettingService _secService;
        private readonly IAppSettingsProvider _appSettingsProvider;
        public SystemSettingsController(IPanelAppGeneralSettingService service, IWebHostEnvironment env, IPanelAppSecuritySettingService secService, IAppSettingsProvider appSettingsProvider)
        {
            _service = service;
            _env = env;
            _secService = secService;
            _appSettingsProvider = appSettingsProvider;
        }

        [HttpGet]
        public async Task<IActionResult> General()
        {
            // Tek kayıt mantığı: varsa ilk kaydı al, yoksa boş form göster
            var listResult = await _service.GetGeneralSettingListAsync();
            if (!listResult.IsSuccess)
            {
                TempData["Error"] = listResult.Message ?? "Ayarlar yüklenemedi.";
                return View(new GeneralSettingsPageViewModel());
            }

            var first = listResult.Data?.FirstOrDefault();
            if (first == null) return View(new GeneralSettingsPageViewModel());

            var vm = new GeneralSettingsPageViewModel
            {
                Id = first.Id,
                SiteName = first.SiteName,
                Domain = first.Domain,
                Theme = first.Theme,
                LogoUrl = first.LogoUrl,
                MetaTitleSuffix = first.MetaTitleSuffix,
                DefaultMetaDescription = first.DefaultMetaDescription
            };
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> General(GeneralSettingsPageViewModel vm)
        {
            if (!ModelState.IsValid) return View(vm);

            // Logo dosyası geldiyse kaydet ve LogoUrl set et
            if (vm.LogoFile is not null && vm.LogoFile.Length > 0)
            {
                var ext = Path.GetExtension(vm.LogoFile.FileName);
                var fileName = $"logo_{DateTime.UtcNow:yyyyMMddHHmmssfff}{ext}";
                var saveDir = Path.Combine(_env.WebRootPath, "uploads", "logos");
                Directory.CreateDirectory(saveDir);
                var fullPath = Path.Combine(saveDir, fileName);
                using (var fs = System.IO.File.Create(fullPath))
                    await vm.LogoFile.CopyToAsync(fs);
                vm.LogoUrl = $"/uploads/logos/{fileName}";
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



        #region Security Settings
        [HttpGet]
        public async Task<IActionResult> Security()
        {
            // Tek kayıt yaklaşımı: varsa ilkini göster
            var list = await _secService.ListAsync();
            if (!list.IsSuccess) { TempData["Error"] = list.Message; return View(new SecuritySettingsPageViewModel()); }

            var first = list.Data?.FirstOrDefault();
            if (first == null) return View(new SecuritySettingsPageViewModel());

            return View(new SecuritySettingsPageViewModel
            {
                Id = first.Id,
                PasswordRequiredLength = first.PasswordRequiredLength,
                PasswordRequireDigit = first.PasswordRequireDigit,
                PasswordRequireLowercase = first.PasswordRequireLowercase,
                PasswordRequireUppercase = first.PasswordRequireUppercase,
                PasswordRequireNonAlphanumeric = first.PasswordRequireNonAlphanumeric,
                LockoutTimeSpanMinutes = first.LockoutTimeSpanMinutes,
                LockoutMaxFailedAccessAttempts = first.LockoutMaxFailedAccessAttempts,
                LockoutAllowedForNewUsers = first.LockoutAllowedForNewUsers,
                SignInRequireConfirmedEmail = first.SignInRequireConfirmedEmail,
                SignInRequireConfirmedPhoneNumber = first.SignInRequireConfirmedPhoneNumber,
                TwoFactorRequired = first.TwoFactorRequired
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Security(SecuritySettingsPageViewModel vm)
        {
            if (!ModelState.IsValid) return View(vm);

            if (vm.Id.HasValue && vm.Id.Value > 0)
            {
                var dto = new AppSecuritySettingEditDto
                {
                    Id = vm.Id.Value,
                    PasswordRequiredLength = vm.PasswordRequiredLength,
                    PasswordRequireDigit = vm.PasswordRequireDigit,
                    PasswordRequireLowercase = vm.PasswordRequireLowercase,
                    PasswordRequireUppercase = vm.PasswordRequireUppercase,
                    PasswordRequireNonAlphanumeric = vm.PasswordRequireNonAlphanumeric,
                    LockoutTimeSpanMinutes = vm.LockoutTimeSpanMinutes,
                    LockoutMaxFailedAccessAttempts = vm.LockoutMaxFailedAccessAttempts,
                    LockoutAllowedForNewUsers = vm.LockoutAllowedForNewUsers,
                    SignInRequireConfirmedEmail = vm.SignInRequireConfirmedEmail,
                    SignInRequireConfirmedPhoneNumber = vm.SignInRequireConfirmedPhoneNumber,
                    TwoFactorRequired = vm.TwoFactorRequired
                };

                var up = await _secService.UpdateAsync(dto);
                if (!up.IsSuccess) { ModelState.AddModelError("", up.Message ?? "Güncelleme hatası."); return View(vm); }

                TempData["Success"] = "Güvenlik ayarları güncellendi.";
                return RedirectToAction(nameof(Security));
            }
            else
            {
                var dto = new AppSecuritySettingCreateDto
                {
                    PasswordRequiredLength = vm.PasswordRequiredLength,
                    PasswordRequireDigit = vm.PasswordRequireDigit,
                    PasswordRequireLowercase = vm.PasswordRequireLowercase,
                    PasswordRequireUppercase = vm.PasswordRequireUppercase,
                    PasswordRequireNonAlphanumeric = vm.PasswordRequireNonAlphanumeric,
                    LockoutTimeSpanMinutes = vm.LockoutTimeSpanMinutes,
                    LockoutMaxFailedAccessAttempts = vm.LockoutMaxFailedAccessAttempts,
                    LockoutAllowedForNewUsers = vm.LockoutAllowedForNewUsers,
                    SignInRequireConfirmedEmail = vm.SignInRequireConfirmedEmail,
                    SignInRequireConfirmedPhoneNumber = vm.SignInRequireConfirmedPhoneNumber,
                    TwoFactorRequired = vm.TwoFactorRequired
                };

                var cr = await _secService.CreateAsync(dto);
                if (!cr.IsSuccess) { ModelState.AddModelError("", cr.Message ?? "Kayıt hatası."); return View(vm); }

                TempData["Success"] = "Güvenlik ayarları kaydedildi.";
                return RedirectToAction(nameof(Security));
            }
        }
        #endregion
    }

}
