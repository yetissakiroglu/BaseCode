using AppWeb.Models;
using Economy.Application.Queries.AppMenus;
using Economy.Application.Queries.AppSettings;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Globalization;

namespace AppWeb.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private readonly IMediator _mediator;

        public HomeController(ILogger<HomeController> logger, IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }

        public async Task<IActionResult> Index()
        {  
            // CultureInfo.CurrentUICulture.Name ile geçerli dil kodunu alýyoruz

            var lang = CultureInfo.CurrentUICulture.Name;
            var menus = await _mediator.Send(new GetAllAppMenuByParentMenuIdQuery(null));

            
            var setting = await _mediator.Send(new GetAppSettingQuery());

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
