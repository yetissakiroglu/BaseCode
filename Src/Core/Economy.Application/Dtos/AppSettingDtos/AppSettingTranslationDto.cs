namespace Economy.Application.Dtos.AppSettingDtos
{
    public class AppSettingTranslationDto
    {
        public int AppLanguageId { get; set; }
        public string TranslatedSiteTitle { get; set; }
        public string TranslatedSiteDescription { get; set; }
        public string TranslatedMetaTitle { get; set; }
        public string TranslatedMetaDescription { get; set; }
    }
}
