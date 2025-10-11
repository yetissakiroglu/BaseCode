using Economy.Application.AdminUI.Dtos.AppGeneralSettingDtos;

namespace Economy.Application.Providers
{
    public interface IAppSettingsProvider
    {
        Task<AppGeneralSettingDto?> GetGeneralAsync(bool useCache = true);
        void InvalidateGeneralCache();
    }
}
