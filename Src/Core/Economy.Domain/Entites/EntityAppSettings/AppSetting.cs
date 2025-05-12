using Economy.Domain.BaseEntities;

namespace Economy.Domain.Entites.EntityAppSettings
{
    public class AppSetting : BaseEntity<int>
    {
        public virtual ICollection<AppSettingTranslation> Translations { get; set; } = new List<AppSettingTranslation>();

    }
}
