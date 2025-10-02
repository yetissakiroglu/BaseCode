using Economy.Domain.BaseEntities;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Economy.Domain.Entites.TenantEntity.EntityAppPages
{
    // Medya çevirileri (alt/caption gibi dil bazlı)
    [Index(nameof(ContentMediaId), nameof(LanguageId), IsUnique = true)]
    public class ContentMediaTranslation : BaseEntity<int>
    {
        public int ContentMediaId { get; set; }
        public int LanguageId { get; set; }

        [MaxLength(200)]
        public string? Alt { get; set; }

        [MaxLength(300)]
        public string? Caption { get; set; }

        public bool IsActive { get; set; } = true;

        // Navigations (opsiyonel)
        // public ContentMedia? ContentMedia { get; set; }
        // public AppLanguage? Language { get; set; }
    }
}
