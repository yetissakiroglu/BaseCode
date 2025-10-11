using Economy.Core.Enums;
using Economy.Core.Tools.Result;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Text;

namespace Economy.Panel.UI.Controllers
{
    public class BaseController : Controller
    {
        protected void AddMessage<T>(ServiceResult<T> response)
        {
            if (response == null)
                return;

            var messages = new List<string>();

            if (!string.IsNullOrWhiteSpace(response.Message))
                messages.Add(response.Message);

            if (response.Errors?.Any() == true)
                messages.AddRange(response.Errors);

            if (!messages.Any())
                return;

            var messageBuilder = new StringBuilder();
            foreach (var msg in messages)
                messageBuilder.AppendLine(msg);

            TempData["MessageNotification"] = messageBuilder.ToString().TrimEnd();
            TempData["TypeNotification"] = response.StatusCode switch
            {
                >= 200 and < 300 => response.HasData ? NotificationType.Success : NotificationType.Warning,
                >= 400 and < 500 => NotificationType.Warning,
                >= 500 => NotificationType.Danger,
                _ => NotificationType.Information
            };

            TempData["TitleNotification"] = response.StatusCode switch
            {
                >= 200 and < 300 => response.HasData ? "Başarılı" : "Uyarı",
                >= 400 and < 500 => "Uyarı",
                >= 500 => "Hata",
                _ => "Bildirim"
            };
        }
        protected void AddValidationErrorsToModelState(IReadOnlyDictionary<string, string[]>? validationErrors)
        {
            if (validationErrors == null) return;

            foreach (var (key, messages) in validationErrors)
            {
                foreach (var message in messages)
                {
                    if (key == "DuplicateUserName")
                    {
                        ModelState.AddModelError("UserName", message);
                    }

                    if (key == "DuplicateEmail")
                    {
                        ModelState.AddModelError("Email", message);
                    }
                    else
                    {
                        ModelState.AddModelError(key, message);
                    }
                }
            }
        }
        protected int CurrentUserId
        {
            get
            {
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
                if (userIdClaim == null)
                    return 0;

                return int.TryParse(userIdClaim.Value, out int id) ? id : 0;
            }
        }
        protected string CurrentUserName
        {
            get => User.FindFirst(ClaimTypes.Name)?.Value ?? string.Empty;
        }
        protected string CurrentUserEmail
        {
            get => User.FindFirst(ClaimTypes.Email)?.Value ?? string.Empty;
        }
        protected string CurrentUserFullName
        {
            get
            {
                var firstName = User.FindFirst("FirstName")?.Value ?? "";
                var lastName = User.FindFirst("LastName")?.Value ?? "";
                return $"{firstName} {lastName}".Trim();
            }
        }
        protected List<string> CurrentUserRoles
        {
            get => User.Claims
                        .Where(c => c.Type == ClaimTypes.Role)
                        .Select(c => c.Value)
                        .ToList();
        }
        protected static string RoleLandingUrl(IList<string> roles)
        {
            if (roles.Contains("Super Admin")) return "/admin/dashboard";
            if (roles.Contains("Tenant Admin")) return "/tenant/home";
            return "/"; // default
        }
    }
}

