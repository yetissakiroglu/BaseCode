using Economy.Application.Dtos.DashboardSummaryDtos;
using Economy.Application.Interfaces;
using Economy.PanelUI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Economy.Panel.UI.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    //[Route("[area]/[controller]")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IPanelDashboardService _svc;

        public HomeController(ILogger<HomeController> logger, IPanelDashboardService svc)
        {
            _logger = logger;
            _svc = svc;
        }

        public async Task<IActionResult> Index()
        {
            var res = await _svc.GetSummaryAsync();
            if (!res.IsSuccess || res.Data == null)
            {
                TempData["Error"] = res.Message ?? "Dashboard verileri alınamadı.";
                return View(new DashboardSummaryDto());
            }
            ViewData["Title"] = "Dashboard";
            return View(res.Data);

            //var user = new AppUser
            //{
            //    Id = 1,
            //    UserName = "Hotel1",
            //    NormalizedUserName = "Hotel1",
            //    Email = "Hotel1@example.com",
            //    NormalizedEmail = "HOTEL1@EXAMPLE.COM",
            //    EmailConfirmed = true,
            //    FirstName = "Hotel1",
            //    LastName = "Yöneticisi",
            //    IsDefaultAdmin = true,
            //    SecurityStamp = "11111111-aaaa-bbbb-cccc-222222222222",
            //    ConcurrencyStamp = "33333333-dddd-eeee-ffff-444444444444",
            //    PasswordHash = "AQAAAAIAAYagAAAAENTd6wlppRLil0VbnPSjSF66HtD4Ckjs1Uraqpgi3/41X9LTDtE+ANyVCJQLfpjVyw==",
            //    TenantId = 1,
            //    //Bu örnek hash değeri "Admin123*" parolası için geçerlidir.
            //};

            //var password = "Admin123*"; // Kullanıcının girdiği şifre
            //var hasher = new PasswordHasher<AppUser>();
            //var result = hasher.VerifyHashedPassword(user, user.PasswordHash, password);

            return View();
        }
        public IActionResult Editor()
        {
            return View();
        }
        public IActionResult Select()
        {
            return View();
        }
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
