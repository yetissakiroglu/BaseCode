namespace Economy.Application.TenantUI.Dtos.AppSettingDtos
{
    public class AppSettingTranslationDto
    {
        public int? Id { get; set; } // Çeviri ID'si    
        public int? AppSettingId { get; set; }
        public int? AppLanguageId { get; set; }
        public string SiteTitle { get; set; }
        public string Description { get; set; }
        public string MetaTitle { get; set; }
        public string MetaDescription { get; set; }
    }
}
