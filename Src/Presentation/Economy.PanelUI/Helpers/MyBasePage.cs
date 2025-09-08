using Microsoft.AspNetCore.Mvc.Razor;
using System.Security.Claims;

namespace Economy.Panel.UI.Helpers
{
    public abstract class MyBaseViewPage<TModel> : RazorPage<TModel>
    {
        public bool IsSuperAdmin() => User?.IsInRole("Super Admin") ?? false;
        public bool IsTenantAdmin() => User?.IsInRole("Tenant Admin") ?? false;
        public string CurrentUserEmail => User?.FindFirstValue(ClaimTypes.Email) ?? "Anonim";
        public string CurrentUserId => User?.FindFirstValue(ClaimTypes.NameIdentifier) ?? "";
        public string CurrentUserName => User?.FindFirstValue(ClaimTypes.Name) ?? "";
        public string CurrentFullName => (User?.FindFirst("FirstName")?.Value ?? "") + " " + (User?.FindFirst("LastName")?.Value ?? "");
        public bool IsAuthenticated => User?.Identity?.IsAuthenticated ?? false;
    }
}
