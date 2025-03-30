using Economy.Application.Interfaces;
using Economy.Persistence.Contexts;
using Microsoft.Extensions.Caching.Memory;

namespace Economy.Persistence.Services
{
    public class CacheService : ICacheService
    {
        private readonly IMemoryCache _memoryCache;
        private readonly AppDbContext _context;

        public CacheService(IMemoryCache memoryCache, AppDbContext context)
        {
            _memoryCache = memoryCache;
            _context = context;
        }

        public T? Get<T>(string key)
        {
            if (!IsCacheEnabled())
                return default;

            return _memoryCache.TryGetValue(key, out T value) ? value : default;
        }

        public void Set<T>(string key, T value, TimeSpan duration)
        {
            if (!IsCacheEnabled())
                return;

            _memoryCache.Set(key, value, duration);
        }

        public void Remove(string key)
        {
            _memoryCache.Remove(key);
        }

        public bool IsCacheEnabled()
        {
            // Her zaman veritabanından oku
            var setting = _context.AppTechnicalSettings.FirstOrDefault(x => x.Id == 1);
            return setting?.EnableCache ?? false;
        }
    }
}
