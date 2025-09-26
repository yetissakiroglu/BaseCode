using Economy.Application.ApplicationUI.Dtos;

namespace Economy.Application.ApplicationUI.Interfaces
{
    public interface ISiteConfigAccessor
    {
        Task<(SiteSettingDto? Setting, SiteTechnicalDto? Technical)> GetAsync(int appId);
    }
}
