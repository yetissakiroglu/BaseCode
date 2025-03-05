using Economy.Domain.Entites.EntityAppLanguage;
using Economy.Persistence.Contexts;

namespace Economy.Persistence.Seeds
{
    public class AppLanguageSeeder
    {
        private readonly AppDbContext _context;

        public AppLanguageSeeder(AppDbContext context)
        {
            _context = context;
        }

        public async Task SeedAsync()
        {
            if (!_context.AppLanguages.Any())
            {
                var appLanguages = GetPreconfiguredAppLanguages();
                await _context.AppLanguages.AddRangeAsync(appLanguages);
                await _context.SaveChangesAsync();
            }
        }

        private IEnumerable<AppLanguage> GetPreconfiguredAppLanguages()
        {
            return new List<AppLanguage>
            {
                new AppLanguage
                {
                    Code = "tr",
                    Name = "Türkçe",
                    IsRTL = false,
                    Icon = "🇹🇷",
                    IsActive = true,
                    IsDefault = true,
                    IsDeleted = false
                },
                new AppLanguage
                {
                    Code = "en",
                    Name = "English",
                    IsRTL = false,
                    Icon = "🇬🇧",
                    IsActive = true,
                    IsDefault = false,
                    IsDeleted = false
                },
                new AppLanguage
                {
                    Code = "ar",
                    Name = "العربية",
                    IsRTL = true,
                    Icon = "🇸🇦",
                    IsActive = true,
                    IsDefault = false,
                    IsDeleted = false
                }
                // Diğer diller eklenebilir
            };
        }

    }

}
