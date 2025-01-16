using Economy.Domain.BaseEntities;

namespace Economy.Domain.Entites.EntityAppLanguage
{
    public class AppLanguage:BaseEntity<int>
    {
        public string Name { get; set; }
        public string LanguageCode { get; set; }
        public string CultureInfo { get; set; }
        public bool IsEnabled { get; set; }
        public string? ResourceFileName { get; set; }
        public bool IsRTL { get; set; }
        public bool IsDefault { get; set; }
        public string? IconCode { get; set; }
    }
}
