using Economy.Domain.BaseEntities;
using System.ComponentModel.DataAnnotations;

namespace Economy.Domain.Entites.TenantEntity.EntityAppPages
{
    // İçeriğe bağlı medya (galeri/kapak)
    public class ContentMedia : BaseEntity<int>
    {
        public int ContentItemId { get; set; }

        [Required, MaxLength(500)]
        public string MediaUrl { get; set; } = "";

        public bool IsCover { get; set; } = false;
        public int SortOrder { get; set; } = 0;
        public bool IsActive { get; set; } = true;

        // Navigations
        public ICollection<ContentMediaTranslation> Translations { get; set; } = new List<ContentMediaTranslation>();
    }
}
