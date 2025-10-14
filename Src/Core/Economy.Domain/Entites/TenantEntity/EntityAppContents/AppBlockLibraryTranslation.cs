using Economy.Domain.BaseEntities;
using Economy.Domain.Entites.TenantEntity.EntityAppLanguages;

namespace Economy.Domain.Entites.TenantEntity.EntityAppContents
{
    public class AppBlockLibraryTranslation: BaseEntity<int>
    {
        public int Id { get; set; }
        public int AppBlockLibraryId { get; set; }
        public AppBlockLibrary AppBlockLibrary { get; set; } = null!;

        public int AppLanguageId { get; set; }
        public AppLanguage AppLanguage { get; set; } = null!;
        public string LocalizedJson { get; set; } = "{}";
    }
}
