using Economy.Domain.BaseEntities;
using Economy.Domain.Entites.EntityAppLanguage;

namespace Economy.Domain.Entites.EntityAppSettings
{
    public class AppSettingTranslation : BaseEntity<int>
    {
        public int AppSettingId { get; set; }
        public int AppLanguageId { get; set; }

        // Çeviri Alanları
        public string TranslatedSiteTitle { get; set; } = string.Empty;
        public string TranslatedSiteDescription { get; set; } = string.Empty;
        public string TranslatedMetaTitle { get; set; } = string.Empty;
        public string TranslatedMetaDescription { get; set; } = string.Empty;

        public virtual AppSetting AppSetting { get; set; } = null!;
        public virtual AppLanguage AppLanguage { get; set; } = null!;
    }
}
