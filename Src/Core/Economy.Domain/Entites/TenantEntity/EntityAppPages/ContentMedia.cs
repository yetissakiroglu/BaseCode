using Economy.Core.Enums;
using Economy.Domain.BaseEntities;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Economy.Domain.Entites.TenantEntity.EntityAppPages
{
    // İçeriğe bağlı medya (galeri/kapak)
    public class ContentMedia : BaseEntity<int>
    {
        public int OwnerId { get; set; }

        [Required, MaxLength(500)]
        public string Url { get; set; } = "";

        public int SortOrder { get; set; } = 0;
        public bool IsActive { get; set; } = true;

        // Navigations
        public ICollection<ContentMediaTranslation> Translations { get; set; } = new List<ContentMediaTranslation>();
    }
}
