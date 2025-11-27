using Economy.Domain.BaseEntities;
using Economy.Domain.Entites.TenantEntity.EntityAppLanguages;

namespace Economy.Domain.Entites.TenantEntity.EntityAppPages
{

    public class DefRoomAttributeGroupTranslation : BaseEntity<int>
    {
        public int GroupId { get; set; }
        public DefRoomAttributeGroup Group { get; set; } = null!;

        public int AppLanguageId { get; set; }
        public AppLanguage AppLanguage { get; set; } = null!;

        public string Name { get; set; } = null!;           // "Genel Özellikler"
    }

}
