using HotelMultiTenant.Services;
using Microsoft.AspNetCore.Mvc;

namespace HotelMultiTenant.Controllers
{
    //[OutputCache(PolicyName = "PerHost")]
    public class HomeController(IContentService content) : Controller
    {
        private int GetTenantId() => 1;

        public async Task<IActionResult> Index()
            => View(await content.GetHomeAsync(GetTenantId()));

    }

}
