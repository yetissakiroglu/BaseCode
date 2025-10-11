using Economy.Application.ApplicationUI.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Economy.Panel.UI.Areas.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class SiteConfigController : ControllerBase
    {
        private readonly ISiteConfigAccessor _siteConfigAccessor;
        public SiteConfigController(ISiteConfigAccessor siteConfigAccessor)
        {
            _siteConfigAccessor = siteConfigAccessor;

        }

        [HttpGet]
        public async Task<IActionResult> GetAsync()
        {
            var lang = HttpContext.Request.Headers["Accept-Language"].ToString();
            var (setting, technical) = _siteConfigAccessor.GetAsync(lang);
            return Ok(new { Setting = setting, Technical = technical }); // JSON kesin görünür
        }

        [HttpGet("Slides")]
        public async Task<IActionResult> GetSlidesAsync()
        {
            var lang = HttpContext.Request.Headers["Accept-Language"].ToString();
            var slideModels = await _siteConfigAccessor.GetSlidesAsync(lang);
            return Ok(slideModels); // JSON kesin görünür
        }
       
    }
}
