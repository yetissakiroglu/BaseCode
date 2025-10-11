using Economy.Application.AdminUI.Dtos.AppAccountDtos;
using Economy.Application.AdminUI.Interfaces;
using Economy.Application.Extensions;
using Economy.Core.Enums;
using Economy.Core.Tools;
using Economy.Core.Tools.Result;
using Economy.Domain.Entites.AdminEntity.EntityAppUsers;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace Economy.Persistence.Admin.Services
{
    public sealed class PanelAppAccountService : IPanelAppAccountService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IValidator<AppSignInDto> _validator;

        public PanelAppAccountService(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, IValidator<AppSignInDto> validator)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _validator = validator;
        }

        public async Task<ServiceResult<AppLoginResultDto>> LoginAsync(AppSignInDto model, string? returnUrl, HttpContext httpContext)
        {

            var validation = _validator.Validate(model);
            if (!validation.IsValid)
            {
                return ServiceResult<AppLoginResultDto>.Failure(
                    "Doğrulama hatası",
                    validationErrors: validation.ToValidationDictionary()
                );
            }

            // Kullanıcıyı bul (audit için de gerekebilir)
            var user = await _userManager.FindByNameAsync(model.Email)
                       ?? await _userManager.FindByEmailAsync(model.Email);

            // Giriş denemesi (başarısızlıkta lockout sayacı artsın)
            var result = await _signInManager.PasswordSignInAsync(
                userName: model.Email,
                password: model.Password,
                isPersistent: model.RememberMe,
                lockoutOnFailure: true);

            // ✅ Başarılı
            if (result.Succeeded)
            {
                var roles = await _userManager.GetRolesAsync(user);

                return ServiceResult<AppLoginResultDto>.Success(
                    new AppLoginResultDto
                    {
                        Outcome = SignInOutcome.Succeeded,
                        User = user is null ? null : new AppUserResultDto
                        {
                            Id = user.Id,
                            Email = user.Email!,
                            UserName = user.UserName!,
                            RolesName = roles.Count > 0 ? string.Join(", ", roles) : "Rol Atanmamış"
                        }
                    },
                    message: "Giriş başarılı.",
                    statusCode: 200,
                    redirectUrl: string.IsNullOrWhiteSpace(returnUrl) ? "/" : returnUrl
                );
            }

            // ✅ 2FA gerekli
            if (result.RequiresTwoFactor)
            {
                return ServiceResult<AppLoginResultDto>.Failure(
                    data: new AppLoginResultDto
                    {
                        Outcome = SignInOutcome.RequiresTwoFactor,
                        User = null
                    },
                    "İki aşamalı doğrulama gerekli.",
                    statusCode: 401,
                    redirectUrl: BuildTwoFactorRedirect(returnUrl, model.RememberMe)
                );
            }

            // ✅ Kilitli hesap
            if (result.IsLockedOut)
            {
                return ServiceResult<AppLoginResultDto>.Failure(
                    data: new AppLoginResultDto { Outcome = SignInOutcome.LockedOut },
                    "Hesabınız geçici olarak kilitlendi.",
                    statusCode: 423
                );
            }

            return ServiceResult<AppLoginResultDto>.Failure(
            data: new AppLoginResultDto { Outcome = SignInOutcome.InvalidCredentials },
            "Kullanıcı adı veya parola hatalı.",
            statusCode: 401
        );
        }

        public async Task<ServiceResult<NoContent>> LogoutAsync(HttpContext httpContext)
        {
            try
            {
                // Kullanıcıyı oturumdan çıkart
                await _signInManager.SignOutAsync();
                // Servis katmanında sadece "başarılı" bilgisini dön
                return ServiceResult<NoContent>.Success(
                    data: null,
                    message: "Kullanıcı başarıyla çıkış yaptı.",
                    redirectUrl: "/"
                );
            }
            catch (Exception ex)
            {
                return ServiceResult<NoContent>.Failure(
                    message: "Çıkış yapılırken hata oluştu.",
                    errors: new[] { ex.Message }
                );
            }
        }





        private static string BuildTwoFactorRedirect(string? returnUrl, bool rememberMe)
        {
            var encodedReturnUrl = string.IsNullOrWhiteSpace(returnUrl)
                ? string.Empty
                : Uri.EscapeDataString(returnUrl);

            var qsReturn = string.IsNullOrWhiteSpace(encodedReturnUrl) ? string.Empty : $"returnUrl={encodedReturnUrl}&";
            return $"/Account/LoginWith2fa?{qsReturn}rememberMe={(rememberMe ? "true" : "false")}";
        }


    }
}
