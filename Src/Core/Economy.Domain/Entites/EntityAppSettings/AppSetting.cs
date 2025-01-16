using Economy.Domain.BaseEntities;

namespace Economy.Domain.Entites.EntityAppSettings
{
    public class AppSetting : BaseEntity<int>
    {
        public string TimeZone { get; set; }

        // Zorunlu olanlar
        public string SiteName { get; set; } // Sitenin adı (zorunlu)

        // Zorunlu olmayanlar
        public string? MetaTitle { get; set; } // Ana meta başlık (isteğe bağlı)
        public string? MetaDescription { get; set; } // Meta açıklama (isteğe bağlı)

        // Zorunlu olanlar
        public string? SiteLogo { get; set; } // Logo dosya yolu (zorunlu)

        // Zorunlu olmayanlar
        public string? SiteFavicon { get; set; } // Favicon dosya yolu (isteğe bağlı)


        // Bakım Modu (isteğe bağlı)
        public bool IsMaintenanceMode { get; set; } // Bakım modu açık mı? (varsayılan: false)
        public string? MaintenanceMessage { get; set; } // Bakım mesajı (isteğe bağlı)

    }
}
