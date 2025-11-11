using Economy.Domain.Entites.AdminEntity.EntityApp;
using Economy.Persistence.Contexts;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Economy.Panel.UI.Areas.Tenant.Controllers
{
    [Area("Tenant")]
    [Authorize(Roles = "Tenant Admin")]
    public class SelectController : Controller
    {
        private readonly DefaultDbContext _db;
        public SelectController(DefaultDbContext db) => _db = db;

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            var list = await _db.Set<AppManager>()
                .Where(m => m.UserId == userId)
                .Select(m => new Vm { AppId = m.AppId, Name = m.App!.HotelName, Domain = m.App.Domain })
                .ToListAsync();

            return View(list);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(int appId)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            var authorized = await _db.Set<AppManager>()
                .AnyAsync(m => m.UserId == userId && m.AppId == appId);

            if (!authorized)
            {
                ModelState.AddModelError("", "Bu oteli yönetme yetkiniz yok.");
                return await Index();
            }

            // Güncel principal
            var authResult = await HttpContext.AuthenticateAsync(IdentityConstants.ApplicationScheme);
            var principal = authResult?.Principal ?? HttpContext.User;
            var identity = (ClaimsIdentity)principal.Identity!;

            var ex = identity.FindFirst("appId");
            if (ex != null) identity.RemoveClaim(ex);
            identity.AddClaim(new Claim("appId", appId.ToString()));

            await HttpContext.SignInAsync(
                IdentityConstants.ApplicationScheme,
                new ClaimsPrincipal(identity),
                new AuthenticationProperties { IsPersistent = true }
            );

            return Redirect("/tenant");
        }

        public sealed class Vm
        {
            public int AppId { get; set; }
            public string Name { get; set; } = "";
            public string? Domain { get; set; }
        }
    }
}
