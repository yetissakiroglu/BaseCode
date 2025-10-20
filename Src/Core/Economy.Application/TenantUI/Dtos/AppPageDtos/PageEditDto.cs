using Economy.Core.Dtos.Custom;

namespace Economy.Application.TenantUI.Dtos.AppPageDtos
{
    public class PageEditDto
    {
        public int? Id { get; set; }
        public int? AppPageId { get; set; }
        public bool IsActive { get; set; } = true;
        public bool IsHomepage { get; set; } = false;
        public DateTime? PublishAtUtc { get; set; }
        public int SortOrder { get; set; }
        public short Type { get; set; } = 1;
        public string? CoverImageUrl { get; set; }
        public string? OgImageUrl { get; set; }
        public List<PageTranslationDto> Translations { get; set; } = new();
        public List<GalleryGroupVm> Galleries { get; set; } = new();
        public List<ImageFieldVm> Singles { get; set; } = new();
    }

    public class PageTranslationDto
    {
        public int? Id { get; set; }
        public int AppLanguageId { get; set; }
        public string AppLanguageCode { get; set; } = default!;
        public string? AppLanguageIcon { get; set; } = default!;
        public string Slug { get; set; }
        public string? Title { get; set; }
        public string? Summary { get; set; }
        public string? Body { get; set; }
        public string? MetaTitle { get; set; }
        public string? MetaDescription { get; set; }
    }

}
