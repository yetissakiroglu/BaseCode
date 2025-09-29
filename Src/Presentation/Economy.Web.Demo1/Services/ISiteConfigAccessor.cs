using Economy.UI.Models;

namespace Economy.Web.Demo1.Services
{
    public interface ISiteConfigAccessor
    {
        Task<(SiteSettingDto? Setting, SiteTechnicalDto? Technical)> GetAsync(string lang);

    }
}
