using Economy.Application.Dtos.DashboardSummaryDtos;
using Economy.Application.Interfaces;
using Economy.PanelUI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Economy.PanelUI.Controllers;

[Authorize]  // Bu, sadece giriþ yapmýþ kullanýcýlarýn eriþmesini saðlar
public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IPanelDashboardService _svc;

    public HomeController(ILogger<HomeController> logger, IPanelDashboardService svc)
    {
        _logger = logger;
        _svc = svc;
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
