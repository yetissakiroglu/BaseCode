using Economy.Domain.BaseEntities;

namespace Economy.Domain.Entites.EntityAppSettings
{
    public class AppSettingLogo : BaseEntity<int>
    {
        public string LogoPath { get; set; } = string.Empty; 
        public string? MobileLogoPath { get; set; }
        public string? FaviconPath { get; set; }
    }
}
