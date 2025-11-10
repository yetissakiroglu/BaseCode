using Economy.Application.ApplicationUI.Dtos;
using Economy.Application.ApplicationUI.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Economy.Panel.UI.Areas.Api
{
    [Area("Api")]
    [ApiController]
    [Route("api/[controller]")]
    //[ResponseCache(Duration = 60, Location = ResponseCacheLocation.Any, NoStore = false)]
    public class MenuController(IApplicationMenuService applicationMenuService) : ControllerBase
    {
        private readonly IApplicationMenuService _applicationMenuService = applicationMenuService;

        [HttpGet("menus")]
        [ProducesResponseType(typeof(IEnumerable<MenuNodeDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> MenusAsync(CancellationToken ct)
        {
            var menus = await _applicationMenuService.GetMenuAsync("tr", ct);
            return Ok(menus);
        }
    }
}
