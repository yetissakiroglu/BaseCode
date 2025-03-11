using AppWeb.Models;
using AppWeb.Providers;
using Economy.Application.Queries.AppMenus;
using Economy.Application.Queries.AppPages;
using Economy.Application.Queries.AppSettings;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace AppWeb.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private readonly IMediator _mediator;
        private readonly LanguageProvider _languageProvider;

        public HomeController(ILogger<HomeController> logger, IMediator mediator, LanguageProvider languageProvider)
        {
            _logger = logger;
            _mediator = mediator;
            _languageProvider = languageProvider;
        }
         
        public async Task<IActionResult> Index(string lang)
        {
            if (!string.IsNullOrEmpty(lang))
            {
              var defaultPage = await _mediator.Send(new GetAppPageByLanguageCodeQuery(lang));
            }

            var lang1 = _languageProvider.GetCurrentLanguage();
            var defaultPageNew = await _mediator.Send(new GetAppPageByLanguageCodeQuery(lang1));
            

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
