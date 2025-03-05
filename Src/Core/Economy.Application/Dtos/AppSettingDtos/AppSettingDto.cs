namespace Economy.Application.Dtos.AppSettingDtos
{
    public class AppSettingDto
    {
        public string SiteTitle { get; set; }
        public string SiteDescription { get; set; }
        public string LogoUrl { get; set; }
        public string PrimaryColor { get; set; }
        public string MetaTitle { get; set; }
        public string MetaDescription { get; set; }
        public bool IsMaintenanceMode { get; set; }
        public string MaintenanceMessage { get; set; }

        public List<AppSettingTranslationDto> Translations { get; set; } = new();
    }
}
