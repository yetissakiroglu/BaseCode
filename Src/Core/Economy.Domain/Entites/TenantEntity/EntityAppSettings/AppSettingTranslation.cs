using Economy.Domain.BaseEntities;
using Economy.Domain.Entites.TenantEntity.EntityAppLanguages;
using System.ComponentModel.DataAnnotations;

namespace Economy.Domain.Entites.TenantEntity.EntityAppSettings
{
    public class AppSettingTranslation : BaseEntity<int>
    {
        // Yabancı anahtarlar
        public int AppSettingId { get; set; }
        public AppSetting AppSetting { get; set; }
        public int AppLanguageId { get; set; }
        public AppLanguage AppLanguage { get; set; }   // 🔹 navigasyon
        // Çeviri alanları
        [MaxLength(200)]
        public string? Title { get; set; }

        [MaxLength(300)]
        public string? Description { get; set; }

        [MaxLength(200)]
        public string? MetaTitle { get; set; }
        [MaxLength(500)]
        public string? MetaSlogan { get; set; }

        [MaxLength(300)]
        public string? MetaDescription { get; set; }
    }
}
