using Economy.Domain.Entites.EntityAppSettings;
using Economy.Persistence.Contexts;

namespace Economy.Persistence.Seeds
{
    public class AppSettingSeeder
    {
        private readonly AppDbContext _context;

        public AppSettingSeeder(AppDbContext context)
        {
            _context = context;
        }

        public async Task SeedAsync()
        {
            if (!_context.AppSettings.Any())
            {
                var appSettings = GetPreconfiguredAppSettings();
                await _context.AppSettings.AddRangeAsync(appSettings);
                await _context.SaveChangesAsync();
            }
        }

        private IEnumerable<AppSetting> GetPreconfiguredAppSettings()
        {
            return new List<AppSetting>
    {
        new AppSetting
        {
            SiteTitle = "Varsayılan Başlık",
            SiteDescription = "Varsayılan Açıklama",
            LogoUrl = "/images/default-logo.png",
            PrimaryColor = "#000000",
            MetaTitle = "Varsayılan Meta Başlığı",
            MetaDescription = "Varsayılan Meta Açıklaması",
            IsMaintenanceMode = false,
            MaintenanceMessage = "Site şu anda bakım modunda.",
            Translations = new List<AppSettingTranslation>
            {
                new AppSettingTranslation
                {
                    AppLanguageId = 1, // Türkçe
                    TranslatedSiteTitle = "Varsayılan Başlık (TR)",
                    TranslatedSiteDescription = "Varsayılan Açıklama (TR)",
                    TranslatedMetaTitle = "Varsayılan Meta Başlığı (TR)",
                    TranslatedMetaDescription = "Varsayılan Meta Açıklaması (TR)",
                },
                new AppSettingTranslation
                {
                    AppLanguageId = 2, // İngilizce
                    TranslatedSiteTitle = "Default Title (EN)",
                    TranslatedSiteDescription = "Default Description (EN)",
                    TranslatedMetaTitle = "Default Meta Title (EN)",
                    TranslatedMetaDescription = "Default Meta Description (EN)",
                }
            }
        }
    };
        }

    }

}
