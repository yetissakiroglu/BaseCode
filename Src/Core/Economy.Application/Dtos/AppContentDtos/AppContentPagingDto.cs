using Economy.Domain.Enums;

namespace Economy.Panel.Application.Dtos.AppContentDtos
{
    public class AppContentPagingDto
    {
        public int Id { get; set; }
        public string? WebThumbnailUrl { get; set; }
        public string? MobilThumbnailUrl { get; set; }
        public ContentType ContentType { get; set; } = ContentType.Odalar; // Onay durumu 
        // İlişkiler
        public int? AppCategoryId { get; set; } // Kategori ID'si
        public int AppLanguageId { get; set; }
        // 
        public string Title { get; set; }
        public string? ShortDescription { get; set; }
        public string? Content { get; set; }
        public bool IsExternal { get; set; }
        public string Url { get; set; }

    }
}
