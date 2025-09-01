using Economy.Application.Dtos.AppAccountDtos;
using Economy.Application.Interfaces;
using Economy.Base.Application.Dtos.BaseModels;
using Economy.Core.Dtos;
using Economy.Core.Tools;
using Economy.Core.Tools.Result;
using Economy.Domain.Entites.Identities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Economy.Persistence.Services
{
    public sealed class PanelAppAccountService : IPanelAppAccountService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IAuditLogWriter _audit;

        public PanelAppAccountService(
            UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager,
            IAuditLogWriter audit
          )
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _audit = audit;
        }

        public async Task<ServiceResult<LoginResultDto>> LoginAsync(
            SignIn model,
            string? returnUrl,
            HttpContext httpContext,
            ModelStateDictionary modelState)
        {
            // Basit model kontrolleri
            if (model is null)
            {
                modelState.AddModelError(string.Empty, "Model boş gönderildi.");
                return ServiceResult<LoginResultDto>.Failure("Model boş gönderildi.", statusCode: 400);
            }
            if (string.IsNullOrWhiteSpace(model.Email) || string.IsNullOrWhiteSpace(model.Password))
            {
                modelState.AddModelError(string.Empty, "E-posta ve parola zorunludur.");
                return ServiceResult<LoginResultDto>.Failure("E-posta ve parola zorunludur.", statusCode: 400);
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
                await SafeAuditLogAsync(user, model.Email, true, "Login succeeded", httpContext, 200);

                return ServiceResult<LoginResultDto>.Success(
                    new LoginResultDto
                    {
                        Outcome = SignInOutcome.Succeeded,
                        User = user is null ? null : new AppUserDto
                        {
                            Id = user.Id,
                            Email = user.Email!,
                            UserName = user.UserName!
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
                modelState.AddModelError(string.Empty, "İki aşamalı doğrulama gerekli.");
                await SafeAuditLogAsync(user, model.Email, false, "Requires two-factor authentication", httpContext, 401);
                return ServiceResult<LoginResultDto>.Failure(
                    data: new LoginResultDto
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
                modelState.AddModelError(string.Empty, "Hesabınız geçici olarak kilitlendi.");
                await SafeAuditLogAsync(user, model.Email, false, "User locked out", httpContext, 423);

                return ServiceResult<LoginResultDto>.Failure(
                    data: new LoginResultDto { Outcome = SignInOutcome.LockedOut },
                    "Hesabınız geçici olarak kilitlendi.",
                    statusCode: 423
                );
            }

            // ❌ Geçersiz kimlik bilgileri
            modelState.AddModelError(string.Empty, "Kullanıcı adı veya parola hatalı.");
            await SafeAuditLogAsync(user, model.Email, false, "Invalid credentials", httpContext, 401);

            return ServiceResult<LoginResultDto>.Failure(
                data: new LoginResultDto { Outcome = SignInOutcome.InvalidCredentials },
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



        // ----------------- Yardımcı metotlar -----------------

        private async Task SafeAuditLogAsync(AppUser? user, string email, bool success, string message, HttpContext httpContext, int statusCode)
        {
            try
            {
                await _audit.LogLoginAsync(user, email, success, message, httpContext, statusCode);
            }
            catch (Exception ex)
            {
                await _audit.LogLoginAsync(user, email, false, message, httpContext, statusCode);
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
