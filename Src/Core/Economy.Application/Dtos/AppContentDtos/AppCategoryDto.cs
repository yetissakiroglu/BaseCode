using Economy.Domain.Enums;

namespace Economy.Application.Dtos.AppContentDtos
{
    public class AppCategoryDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public string? ShortDescription { get; set; } // Kategori açıklaması
        public string? Content { get; set; }

        public string? MetaTitle { get; set; }
        public string? MetaDescription { get; set; }

        public ContentType ContentType { get; set; }
        public PublicationStatus PublicationStatus { get; set; }
    }
}
