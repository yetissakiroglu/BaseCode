using Economy.Domain.BaseEntities;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Economy.Domain.Entites.TenantEntity.EntityAppPages
{
    // İçerik çevirileri (dile bağlı alanlar)
    [Index(nameof(ContentItemId), nameof(LanguageId), IsUnique = true)] // aynı içerik + aynı dil = tek kayıt
    [Index(nameof(LanguageId), nameof(Slug))]                           // slug aramaları
    public class ContentItemTranslation : BaseEntity<int>
    {
        // FK’ler
        public int ContentItemId { get; set; }
        public int LanguageId { get; set; }

        // URL için dil bazlı slug (Page/Block)
        [MaxLength(200)]
        public string? Slug { get; set; }

        // Metin alanları
        [MaxLength(200)]
        public string? Title { get; set; }

        [MaxLength(500)]
        public string? Summary { get; set; }

        public string? Body { get; set; }

    
        [MaxLength(100)]
        public string? ButtonText { get; set; }

        [MaxLength(500)]
        public string? ButtonUrl { get; set; }

        [MaxLength(200)]
        public string? MetaTitle { get; set; }

        [MaxLength(300)]
        public string? MetaDescription { get; set; }

     
   
      
        // Navigations (opsiyonel, Fluent'te bağlanır)
        // public ContentItem? ContentItem { get; set; }
        // public AppLanguage? Language { get; set; }
    }

}
