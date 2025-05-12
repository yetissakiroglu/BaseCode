using Economy.Panel.Application.Dtos.AppLanguageDtos;

namespace Economy.Panel.Application.Dtos.AppSettingDtos
{
    public class AppSettingTranslationDto
    {
        public int Id { get; set; } // Çeviri ID'si    
        public int AppSettingId { get; set; }
        public int AppLanguageId { get; set; }

        public string SiteTitle { get; set; }

        public string Description { get; set; }

        public string MetaTitle { get; set; }

        public string MetaDescription { get; set; }
        public virtual AppLanguageDto AppLanguage { get; set; }
    }
}
