using Economy.Application.ApplicationUI.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Economy.Panel.UI.Areas.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class MenuConfigController : ControllerBase
    {
        private readonly IMenuAccessor _siteConfigAccessor;
        public MenuConfigController(IMenuAccessor siteConfigAccessor)
        {
            _siteConfigAccessor = siteConfigAccessor;

        }

        [HttpGet]
        public async Task<IActionResult> GetAsync()
        {
            var lang = HttpContext.Request.Headers["Accept-Language"].ToString();
            var menuView = await _siteConfigAccessor.GetAsync(lang);
            return Ok(menuView); // JSON kesin görünür
        }
    }
}
