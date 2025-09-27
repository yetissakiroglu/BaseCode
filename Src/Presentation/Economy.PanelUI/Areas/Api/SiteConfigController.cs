using Economy.Application.ApplicationUI.Interfaces;
using Economy.Persistence.PersistenceUI.Services;
using Microsoft.AspNetCore.Http;
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
        public IActionResult Get()
        {
            var lang = HttpContext.Request.Headers["Accept-Language"].ToString();
            var (setting, technical) = _siteConfigAccessor.GetAsync("tr");
            return Ok(new { Setting = setting, Technical = technical }); // JSON kesin görünür
        }
    }
}
