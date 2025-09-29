using Economy.UI.Models;
using Economy.UI.Models.PageDtos;

namespace Economy.Web.Demo1.Services
{
    public interface ISiteConfigAccessor
    {
        Task<(SiteSettingDto? Setting, SiteTechnicalDto? Technical)> GetAsync(string lang);
        Task<PageUnifiedVm?> GetPageAsync(string lang, string slug);

    }
}
