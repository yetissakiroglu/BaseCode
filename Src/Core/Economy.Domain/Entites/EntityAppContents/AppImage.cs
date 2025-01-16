using Economy.Domain.BaseEntities;

namespace Economy.Domain.Entites.EntityPages
{
    public class AppImage:BaseEntity<int>
    {
        public int AppImageGroupId { get; set; } // ID'si
        public AppImageGroup AppImageGroup { get; set; } // RoomImage sınıfının Room ilişkisi
        public string? ImageUrl { get; set; }
        public bool IsCover { get; set; } // Kapak resim mi değil mi?

    }
}
