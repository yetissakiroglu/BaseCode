using Economy.Panel.UI.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Economy.PanelUI.Controllers;

[Authorize]
public class HomeController : BaseController
{
    public IActionResult Index()
    {
        var target = RoleLandingUrl(CurrentUserRoles);
        return Redirect(target);
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


}
