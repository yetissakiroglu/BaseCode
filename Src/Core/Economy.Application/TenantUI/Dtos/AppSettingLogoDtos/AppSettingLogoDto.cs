namespace Economy.Application.TenantUI.Dtos.AppSettingLogoDtos
{
    public class AppSettingLogoDto
    {
        public int Id { get; set; }
        public string LogoPath { get; set; } = string.Empty;
        public string? MobileLogoPath { get; set; }
        public string? FaviconPath { get; set; }
        public string? ShareImagePath { get; set; }

    }
}
