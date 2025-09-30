using Economy.Domain.BaseEntities;

namespace Economy.Domain.Entites.TenantEntity.EntityAppSettings
{
    public class AppSetting : BaseEntity<int>
    {
        public virtual ICollection<AppSettingTranslation> Translations { get; set; } = new List<AppSettingTranslation>();

    }
}
