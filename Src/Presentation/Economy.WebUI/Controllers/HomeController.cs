using Economy.Web.UI.Services.Abstractions;
using Microsoft.AspNetCore.Mvc;

namespace Economy.WebUI.Controllers;

public class HomeController : Controller
{
    private readonly IContentService _content;
    public HomeController(IContentService content) => _content = content;

    public async Task<IActionResult> Index()
    {
        var c = Thread.CurrentThread.CurrentUICulture.Name;
        ViewBag.Sliders = await _content.GetHomeSliderAsync(c);
        ViewData["Title"] = "Home";
        return View();
    }
}
