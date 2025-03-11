using AppWeb.Providers;
using Economy.Application.Queries.AppMenus;
using MediatR;
using Microsoft.AspNetCore.Mvc;

public class MenuViewComponent : ViewComponent
{
    private readonly IMediator _mediator;
    private readonly ILogger<MenuViewComponent> _logger;
    private readonly LanguageProvider _languageProvider;

    public MenuViewComponent(ILogger<MenuViewComponent> logger, IMediator mediator, LanguageProvider languageProvider)
    {
        _logger = logger;
        _mediator = mediator;
        _languageProvider = languageProvider;
    }

    public async Task<IViewComponentResult> InvokeAsync()
    {
        var lang = _languageProvider.GetCurrentLanguage();

        var menus = await _mediator.Send(new GetAllAppMenuByParentMenuIdQuery(null,lang));
        return View(menus.Data);  // View'ı döndür
    }

  
}
