namespace Economy.Application.TenantUI.Dtos.AppSettingLogoDtos
{
    public class AppSettingLogoCreateEditDto
    {
        public int Id { get; set; }
        public string? LogoBase64 { get; set; }
        public string? MobileLogoBase64 { get; set; }
        public string? FaviconBase64 { get; set; }
        public string? ShareImageBase64 { get; set; }
        public string? LogoPath { get; set; } 
        public string? MobileLogoPath { get; set; }
        public string? FaviconPath { get; set; }
        public string? ShareImagePath { get; set; }
        
    }
}
