using Economy.Application.ApplicationUI.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace Economy.Panel.UI.Areas.Api
{
    [Area("Api")]
    [ApiController]
    // Versiyonlu, sade bir rota: /api/ui/v1/rooms
    [Route("api/ui/v1/[controller]")]
    //[ResponseCache(Duration = 60, Location = ResponseCacheLocation.Any, NoStore = false)]
    public class MenuController : ControllerBase
    {
        [HttpGet("menus")]
        [ProducesResponseType(typeof(IEnumerable<MenuViewModel>), StatusCodes.Status200OK)]
        public IActionResult Menus()
        {
            return Ok();
        }
    }
}
