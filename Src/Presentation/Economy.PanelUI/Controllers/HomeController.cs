using Economy.PanelUI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Economy.PanelUI.Controllers;

[Authorize]  // Bu, sadece giriþ yapmýþ kullanýcýlarýn eriþmesini saðlar
public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {

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
        //    //Bu örnek hash deðeri "Admin123*" parolasý için geçerlidir.
        //};

        //var password = "Admin123*"; // Kullanýcýnýn girdiði þifre
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
