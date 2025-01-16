using Economy.Domain.BaseEntities;
using Economy.Domain.Entites.EntityPages;
using Economy.Domain.Entites.Identities;
using Economy.Domain.Enums;

namespace Economy.Domain.Entites.EntityAppContents
{
    public class AppComment: BaseEntity<int>
    {
        public string Content { get; set; } // Yorum içeriği
        public DateTime CreatedDate { get; set; } // Yorum oluşturulma tarihi
        public ApprovalStatus ApprovalStatus { get; set; } // Onay durumu

        // İlişkiler
        public int AppContentId { get; set; } // Yorumun ait olduğu blog yazısının ID'si
        public AppContent AppContent { get; set; } // Yorumun ait olduğu blog yazısı

        public string? AppUserId { get; set; } // Yorum bırakan kullanıcı ID'si (anonim olabilir)
        public AppUser AppUser { get; set; } // Yorum bırakan kullanıcı

        // Yorumun Yorumu - Daha sonra yapılacak
        //public int? ParentCommentId { get; set; } // Ana yorumun ID'si (alt yorumlar için)
        //public AppComment ParentComment { get; set; } // Ana yorum
        //public ICollection<AppComment>? Replies { get; set; } // Alt yorumlar (zorunlu değil)

    }
}
