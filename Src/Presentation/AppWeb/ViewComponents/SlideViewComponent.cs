using AppWeb.Providers;
using Economy.Application.Queries.AppSlides;
using MediatR;
using Microsoft.AspNetCore.Mvc;

public class SlideViewComponent : ViewComponent
{
    private readonly IMediator _mediator;
    private readonly ILogger<SlideViewComponent> _logger;
    private readonly LanguageProvider _languageProvider;

    public SlideViewComponent(ILogger<SlideViewComponent> logger, IMediator mediator, LanguageProvider languageProvider)
    {
        _logger = logger;
        _mediator = mediator;
        _languageProvider = languageProvider;
    }

    public async Task<IViewComponentResult> InvokeAsync()
    {
        var lang = _languageProvider.GetCurrentLanguage();
        var slides = await _mediator.Send(new GetAllAppSlideByLanguageCodeQuery(lang));
        return View(slides.Data);  // View'ı döndür
    }

  
}
