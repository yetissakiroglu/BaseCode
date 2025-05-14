namespace Economy.Panel.Application.Dtos.AppLanguageDtos
{
    public class AppLanguageDto
    {
        public int Id { get; set; }
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
    }
}
