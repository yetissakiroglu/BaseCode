namespace Economy.Panel.Application.Dtos.AppSettingLogoDtos
{
    public class AppSettingLogoCreateEditDto
    {
        public int Id { get; set; }
        public string LogoPath { get; set; } = string.Empty;
        public string? MobileLogoPath { get; set; }
        public string? FaviconPath { get; set; }
    }
}
