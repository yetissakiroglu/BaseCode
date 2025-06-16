namespace Economy.Panel.Application.Dtos.AppSettingLogoDtos
{
    public class AppSettingLogoDto
    {
        public int Id { get; set; }
        public string LogoPath { get; set; } = string.Empty;
        public string? MobileLogoPath { get; set; }
        public string? FaviconPath { get; set; }
    }
}
