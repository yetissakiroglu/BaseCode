using Economy.Application.Dtos.AppAccountDtos;
using Economy.Core.Dtos;
using Economy.Core.Tools;
using Economy.Core.Tools.Result;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Economy.Application.Interfaces
{
    public interface IPanelAppAccountService
    {
        Task<ServiceResult<LoginResultDto>> LoginAsync(SignIn model, string? returnUrl, HttpContext httpContext, ModelStateDictionary modelState);
        Task<ServiceResult<NoContent>> LogoutAsync(HttpContext httpContext);

    }
}
