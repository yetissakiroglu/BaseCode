using Economy.Application.ApplicationUI.Dtos;

namespace Economy.Application.ApplicationUI.Interfaces
{
    public interface ISiteConfigAccessor
    {
        (SiteSettingDto? Setting, SiteTechnicalDto? Technical) GetAsync(string lang);
    }
}
