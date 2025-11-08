using HotelMultiTenant.Multitenancy;
using HotelMultiTenant.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;

namespace HotelMultiTenant.Controllers
{
    [OutputCache(PolicyName = "PerHost")]
    public class HomeController(IContentService content) : Controller
    {
        private int GetTenantId() => HttpContext.GetTenant()?.Current?.Id ?? 0;

        public async Task<IActionResult> Index()
            => View(await content.GetHomeAsync(GetTenantId()));

        public async Task<IActionResult> About()
            => View(await content.GetAboutAsync(GetTenantId()));

        public async Task<IActionResult> Rooms()
            => View(await content.GetRoomsAsync(GetTenantId()));

        public async Task<IActionResult> Services()
            => View(await content.GetServicesAsync(GetTenantId()));

        public async Task<IActionResult> Contact()
            => View(await content.GetContactAsync(GetTenantId()));
    }

}
