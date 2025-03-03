using Economy.Application.Queries.AppMenus;
using MediatR;
using Microsoft.AspNetCore.Mvc;

public class MenuViewComponent : ViewComponent
{
    private readonly IMediator _mediator;
    private readonly ILogger<MenuViewComponent> _logger;

    public MenuViewComponent(ILogger<MenuViewComponent> logger, IMediator mediator)
    {
        _logger = logger;
        _mediator = mediator;
    }
 
    public async Task<IViewComponentResult> InvokeAsync()
    {
        var menus = await _mediator.Send(new GetAllAppMenuByParentMenuIdQuery(null));
        return View(menus.Data);  // View'ı döndür
    }

  
}
