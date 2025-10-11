using Economy.Application.AdminUI.Dtos.AppGeneralSettingDtos;
using Economy.Application.AdminUI.Interfaces;
using Economy.Application.Providers;
using Microsoft.Extensions.Caching.Memory;

namespace Economy.Persistence.Providers
{

    public class AppSettingsProvider : IAppSettingsProvider
    {
        private readonly IMemoryCache _cache;
        private readonly IPanelAppGeneralSettingService _generalService;
        private const string CacheKey = "GeneralSettings:Current";

        public AppSettingsProvider(IMemoryCache cache, IPanelAppGeneralSettingService generalService)
        {
            _cache = cache;
            _generalService = generalService;
        }

        public async Task<AppGeneralSettingDto?> GetGeneralAsync(bool useCache = true)
        {
            if (useCache && _cache.TryGetValue(CacheKey, out AppGeneralSettingDto dto))
                return dto;

            // Tek kayıt mantığı: ilk aktif kayıt
            var listRes = await _generalService.GetGeneralSettingListAsync();
            var first = listRes.IsSuccess ? listRes.Data?.OrderBy(x => x.Id).FirstOrDefault() : null;

            _cache.Set(CacheKey, first, TimeSpan.FromMinutes(5)); // 5 dk cache
            return first;
        }

        public void InvalidateGeneralCache() => _cache.Remove(CacheKey);
    }
}
