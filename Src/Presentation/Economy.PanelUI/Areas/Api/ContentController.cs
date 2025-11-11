using Economy.Application.ApplicationUI.Dtos;
using Economy.Application.ApplicationUI.Interfaces;
using Economy.UI.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace Economy.Panel.UI.Areas.Api
{
    [Area("Api")]
    [ApiController]
    [Route("api/[controller]")]
    //[ResponseCache(Duration = 60, Location = ResponseCacheLocation.Any, NoStore = false)]
    public class ContentController(IApplicationMenuService applicationMenuService) : ControllerBase
    {
        private readonly IApplicationMenuService _applicationMenuService = applicationMenuService;

        [HttpGet("tanent")]
        [ProducesResponseType(typeof(IEnumerable<SiteTechnicalDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetTanentAsync(CancellationToken ct)
        {
            var menus = await _applicationMenuService.GetSiteTechnicalAsync(ct);
            return Ok(menus);
        }


        [HttpGet("menus")]
        [ProducesResponseType(typeof(IEnumerable<MenuNodeDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> MenusAsync(CancellationToken ct)
        {
            var menus = await _applicationMenuService.GetMenuAsync("tr", ct);
            return Ok(menus);
        }


        [HttpGet("sitemeta")]
        [ProducesResponseType(typeof(IEnumerable<SiteMetaDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> SiteMetaAsync(CancellationToken ct)
        {
            var menus = await _applicationMenuService.GetSiteMetaAsync("tr", ct);
            return Ok(menus);
        }
    }
}
