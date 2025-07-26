using Microsoft.AspNetCore.Mvc.Razor;
using System.Security.Claims;

namespace Economy.Panel.UI.Helpers
{
    public abstract class MyBaseViewPage<TModel> : RazorPage<TModel>
    {
        public bool IsSuperAdmin() => User?.IsInRole("Süper Admin") ?? false;
        public bool IsTenantAdmin() => User?.IsInRole("Tenant Admin") ?? false;
        public bool IsContentManager() => User?.IsInRole("Content Manager") ?? false;
        public bool IsAdmin() => User?.IsInRole("Admin") ?? false;
        public bool IsOtelEditor() => User?.IsInRole("Otel Editör") ?? false;
        public string CurrentUserEmail => User?.FindFirstValue(ClaimTypes.Email) ?? "Anonim";
        public string CurrentUserId => User?.FindFirstValue(ClaimTypes.NameIdentifier) ?? "";
        public string CurrentUserName => User?.FindFirstValue(ClaimTypes.Name) ?? "";
        public string CurrentFullName => (User?.FindFirst("FirstName")?.Value ?? "") + " " + (User?.FindFirst("LastName")?.Value ?? "");
        public bool IsAuthenticated => User?.Identity?.IsAuthenticated ?? false;
    }
}
