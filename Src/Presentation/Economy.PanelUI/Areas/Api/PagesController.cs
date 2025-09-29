
using Economy.Application.ApplicationUI.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Economy.Panel.UI.Areas.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class PagesController : ControllerBase
    {
        private readonly IPageAccessor _pages;
        public PagesController(IPageAccessor pages) => _pages = pages;

        [HttpGet("{slug}")]
        public async Task<IActionResult> GetAsync(string slug, CancellationToken ct)
        {
            var lang = HttpContext.Request.Headers["Accept-Language"].ToString();
            var vm = await _pages.GetAsync(lang, slug, ct);
            if (vm is null) return NotFound();
            return Ok(vm);
        }
        [HttpGet("Homepage")]
        public async Task<IActionResult> GetHomepageAsync(CancellationToken ct)
        {
            var lang = HttpContext.Request.Headers["Accept-Language"].ToString();
            var vm = await _pages.GetAsync(lang, true, ct);
            if (vm is null) return NotFound();
            return Ok(vm);
        }
    }

}
