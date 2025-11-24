using Economy.Domain.BaseEntities;
using Economy.Domain.Entites.TenantEntity.EntityAppLanguages;

namespace Economy.Domain.Entites.TenantEntity.EntityAppPages
{
    public class DefRoomAttributeTranslation : BaseEntity<int>
    {
        public int DefRoomAttributeId { get; set; }
        public DefRoomAttribute DefRoomAttribute { get; set; } = null!;

        public int AppLanguageId { get; set; }
        public AppLanguage AppLanguage { get; set; } = null!;

        public string Name { get; set; } = "";          // "Oda Tipi" / "Room Type"
        public string? Description { get; set; }        // İsteğe bağlı açıklama
    }
}
