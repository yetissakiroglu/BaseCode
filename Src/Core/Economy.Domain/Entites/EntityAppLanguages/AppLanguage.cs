using Economy.Domain.BaseEntities;
using Economy.Domain.Entites.EntityAppSettings;

namespace Economy.Domain.Entites.EntityAppLanguage
{
    public class AppLanguage:BaseEntity<int>
    {
        public string Code { get; set; } // Dil kodu (örneğin: "tr", "en", "ar")
        public string Name { get; set; } // Dilin adı (örneğin: "Türkçe", "English", "العربية")

        // Sağdan sola yazım için bir alan (Opsiyonel, örneğin Arapça için)
        public bool IsRTL { get; set; } // Sağdan sola yazımı destekleyen diller

        // Opsiyonel: Dilin gösterimi için simge (örneğin: "tr" için "🇹🇷")
        public string Icon { get; set; }

        // Bir dilin aktif olup olmadığını belirlemek için
        public bool IsActive { get; set; }

        // Dilin varsayılan olup olmadığını belirtmek için
        public bool IsDefault { get; set; }

        // Bir dildeki çevirileri tutmak için ilişki
        public virtual ICollection<AppSettingTranslation> AppSettingTranslations { get; set; } = new List<AppSettingTranslation>();

    }
}
