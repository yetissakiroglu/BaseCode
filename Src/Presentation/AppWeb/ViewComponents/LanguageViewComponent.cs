using Economy.Application.Queries.AppLanguages;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace AppWeb.ViewComponents
{
    public class LanguageViewComponent : ViewComponent
    {
        private readonly IMediator _mediator;
        private readonly ILogger<LanguageViewComponent> _logger;

        public LanguageViewComponent(ILogger<LanguageViewComponent> logger, IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var languages = await _mediator.Send(new GetAllAppLanguageQuery(true));
            return View(languages.Data);  // View'ı döndür
        }


    }
   
}
