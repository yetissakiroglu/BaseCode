using Microsoft.AspNetCore.Mvc;
using MyHotelSite.Repositories;
using MyHotelSite.Services;

namespace Economy.Web.Demo1UI.Controllers
{
    [Route("{lang:length(2)}/{slug?}")]
    public class PagesController : Controller
    {

        private readonly ISiteConfigAccessor _cfg;
        private readonly IRoomRepository _rooms;
        private readonly ICampaignRepository _campaigns;
        private readonly IGalleryRepository _gallery;
        private readonly ISeoHelper _seo;

        public PagesController(ISiteConfigAccessor cfg, IRoomRepository rooms, ICampaignRepository campaigns, IGalleryRepository gallery, ISeoHelper seo)
        { _cfg = cfg; _rooms = rooms; _campaigns = campaigns; _gallery = gallery; _seo = seo; }



        // Ör: /tr/rooms  veya /tr/aile-odasi
        [HttpGet]
        public async Task<IActionResult> Index(string lang, string? slug = "home", CancellationToken ct = default)
        {
            var vm = await _cfg.GetPageAsync(lang, slug!, ct);
            if (vm is null) return NotFound();

            // Görünüm yönlendirmesi
            if (vm.Type == "list")
                return View("List", vm);

            return View("Detail", vm);
        }

        // Hreflang canonical link’leri (layout’ta çağırmak için)
        [NonAction]
        public static string BuildLangUrl(string lang, string slug) => $"/{lang}/{slug}";
    }
}
