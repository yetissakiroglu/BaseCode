namespace Economy.Application.TenantUI.Dtos.AppSettingDtos
{
    public class AppSettingTranslationDto
    {
        public int? Id { get; set; } // Çeviri ID'si    
        public int AppLanguageId { get; set; }
        public string AppLanguageCode { get; set; } = default!;
        public string AppLanguageIcon { get; set; } = default!;
        public string Title { get; set; }
        public string Description { get; set; }
        public string MetaTitle { get; set; }
        public string MetaSlogan { get; set; }
        public string MetaDescription { get; set; }
    }
}
