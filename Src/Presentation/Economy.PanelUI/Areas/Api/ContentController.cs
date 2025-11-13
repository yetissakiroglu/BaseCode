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
        [ProducesResponseType(typeof(IEnumerable<TenantDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetTanentAsync(CancellationToken ct)
        {  
            // Header'dan X-TENANT anahtarını oku
            var tenantKey = Request.Headers["X-TENANT"].FirstOrDefault();

            if (string.IsNullOrEmpty(tenantKey))
                return BadRequest("Missing X-TENANT header.");

            // Servise tenant anahtarını gönder
            var menus = await _applicationMenuService.GetTenantAsync(tenantKey, ct);

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

        // 1) Tüm yayınlanmış sayfalar
      
        // 2) Anasayfa
        [HttpGet("homepage")]
        [ProducesResponseType(typeof(PageDetailDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetHomepageAsync([FromQuery] string? lang = "tr", CancellationToken ct = default)
        {
            var vm = await _applicationMenuService.GetHomepageAsync(lang ?? "tr", ct);
            return vm is null ? NotFound() : Ok(vm);
        }

        // 3) Slug ile tek sayfa
        [HttpGet("{slug}")]
        [ProducesResponseType(typeof(PageDetailDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetBySlugAsync([FromRoute] string slug, [FromQuery] string? lang = "tr", CancellationToken ct = default)
        {
            var vm = await _applicationMenuService.GetBySlugAsync(lang ?? "tr", slug, ct);
            return vm is null ? NotFound() : Ok(vm);
        }

    }
}
