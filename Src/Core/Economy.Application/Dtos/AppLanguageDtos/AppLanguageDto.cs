using Economy.Domain.Entites.EntityAppLanguage;

namespace Economy.Application.Dtos.WebSettingDtos
{
    public record AppLanguageDto
    {
        public int Id { get; init; }
        public string Name { get; set; }
        public string LanguageCode { get; set; }
        public string CultureInfo { get; set; }
        public bool IsEnabled { get; set; }
        public string? ResourceFileName { get; set; }
        public bool IsRTL { get; set; }
        public bool IsDefault { get; set; }
        public string? IconCode { get; set; }

        // Entity'den DTO'ya dönüştürme metodu
        public static AppLanguageDto FromEntity(AppLanguage entity)
        {
            return new AppLanguageDto
            {
                Id = entity.Id,
                Name = entity.Name,
                LanguageCode = entity.LanguageCode,
                CultureInfo = entity.CultureInfo,
                IsEnabled = entity.IsEnabled,
                ResourceFileName = entity.ResourceFileName,
                IsRTL = entity.IsRTL,
                IsDefault = entity.IsDefault,
                IconCode = entity.IconCode
            };
        }

        // ListModel Özellikleri
        public static List<AppLanguageDto> List(List<AppLanguage> entities)
        {
            return entities.Select(entity => FromEntity(entity)).ToList();
        }

        // CreateModel Özellikleri
        public static AppLanguage CreateEntity(string name, string languageCode, string cultureInfo, bool isEnabled, bool isRTL, bool isDefault, string? resourceFileName, string? iconCode)
        {
            return new AppLanguage
            {
                Name = name,
                LanguageCode = languageCode,
                CultureInfo = cultureInfo,
                IsEnabled = isEnabled,
                IsRTL = isRTL,
                IsDefault = isDefault,
                ResourceFileName = resourceFileName,
                IconCode = iconCode
            };
        }

        // EditModel Özellikleri
        public static AppLanguage EditEntity(int id, string name, string languageCode, string cultureInfo, bool isEnabled, bool isRTL, bool isDefault, string? resourceFileName, string? iconCode)
        {
            return new AppLanguage
            {
                Id = id,
                Name = name,
                LanguageCode = languageCode,
                CultureInfo = cultureInfo,
                IsEnabled = isEnabled,
                IsRTL = isRTL,
                IsDefault = isDefault,
                ResourceFileName = resourceFileName,
                IconCode = iconCode
            };
        }
    }
}
