using AppWeb.Models;
using AppWeb.Providers;
using Economy.Application.Dtos.AppPageDtos;
using Economy.Application.Queries.AppPages;
using Economy.Core.Tools;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace AppWeb.Controllers
{
    public class HomeController : Controller
    {
        private readonly IMediator _mediator;
        private readonly LanguageProvider _languageProvider;

        public HomeController(IMediator mediator, LanguageProvider languageProvider)
        {
            _mediator = mediator;
            _languageProvider = languageProvider;
        }

        public async Task<IActionResult> Index(string lang)
        {
            var result = new ResponseModel<AppPageDto>();

            if (!string.IsNullOrEmpty(lang))
            {
                result = await _mediator.Send(new GetAppPageDefaultByLanguageCodeQuery(lang));
                result.Data.LanguageCode = lang;
                return View(result.Data);
            }

            var langDefault = _languageProvider.GetCurrentLanguage();
            result = await _mediator.Send(new GetAppPageDefaultByLanguageCodeQuery(langDefault));
            //result.Data.LanguageCode = langDefault;
            return View(result.Data);
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
