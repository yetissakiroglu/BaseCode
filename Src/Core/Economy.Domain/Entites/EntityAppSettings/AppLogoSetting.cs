using Economy.Domain.BaseEntities;

namespace Economy.Domain.Entites.EntityAppSettings
{
    public class AppLogoSetting : BaseEntity<int>
    {
        public string WebLogoPath { get; set; }
        public string MobileLogoPath { get; set; }
    }
}
