using Economy.Application.Dtos.AppPageDtos;
using Economy.Application.Queries.AppPages;
using Economy.Core.Tools;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace AppWeb.Controllers
{
    public class PageController : BaseController
    {
        private readonly IMediator _mediator;

        public PageController(IMediator mediator) => _mediator = mediator;

        public async Task<IActionResult> Index(string url, string lang)
        {
            var result = new ResponseModel<AppPageDto>();
            var model = await _mediator.Send(new GetAppPageByLanguageCodeByUrlQuery(url, lang));
            return View(model.Data);
        }
    }
}
