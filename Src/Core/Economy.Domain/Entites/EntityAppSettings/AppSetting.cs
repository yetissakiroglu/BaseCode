﻿using Economy.Domain.BaseEntities;

namespace Economy.Domain.Entites.EntityAppSettings
{
    public class AppSetting : BaseEntity<int>
    {
        // Genel Ayarlar
        public string SiteTitle { get; set; }
        public string SiteDescription { get; set; }

        // Marka ve Tasarım
        public string LogoUrl { get; set; }
        public string PrimaryColor { get; set; }

        // SEO Ayarları
        public string MetaTitle { get; set; }
        public string MetaDescription { get; set; }

        public bool IsMaintenanceMode { get; set; }
        public string MaintenanceMessage { get; set; }

        public virtual ICollection<AppSettingTranslation> Translations { get; set; } = new List<AppSettingTranslation>();


    }
}
