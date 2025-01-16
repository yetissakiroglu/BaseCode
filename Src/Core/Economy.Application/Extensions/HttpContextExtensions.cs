using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace Economy.Application.Extensions
{
    public static class HttpContextExtensions
    {
        public static string GetUserId(this IHttpContextAccessor httpContextAccessor)
        {
            return httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? string.Empty;
        }

        public static string GetActionFromQuery(this HttpRequest request)
        {
            var action = request.Query["action"].ToString().ToLower();
            return string.IsNullOrEmpty(action) ? "create" : action;
        }
    }
}
