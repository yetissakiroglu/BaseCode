using Economy.UI.Models;

namespace Economy.Application.ApplicationUI.Interfaces
{
    public interface ISiteConfigAccessor
    {
        (SiteSettingDto? Setting, SiteTechnicalDto? Technical) GetAsync(string lang);
    }
}
