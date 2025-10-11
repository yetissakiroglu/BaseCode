using Economy.Application.AdminUI.Dtos.AppAccountDtos;
using Economy.Core.Tools;
using Economy.Core.Tools.Result;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Economy.Application.AdminUI.Interfaces
{
    public interface IPanelAppAccountService
    {
        Task<ServiceResult<AppLoginResultDto>> LoginAsync(AppSignInDto model, string? returnUrl, HttpContext httpContext);
        Task<ServiceResult<NoContent>> LogoutAsync(HttpContext httpContext);

    }
}
