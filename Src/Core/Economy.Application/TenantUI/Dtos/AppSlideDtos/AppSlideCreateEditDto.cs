using Economy.Application.TenantUI.Dtos.AppSlideDtos.SlideTranslationDtos;

namespace Economy.Application.TenantUI.Dtos.AppSlideDtos
{
    public class AppSlideCreateEditDto
    {
        public int Id { get; set; }
        public int Sequence { get; set; }
        public string? ThumbnailBase64 { get; set; }
        public string? ThumbnailMobilBase64 { get; set; }
        public string? WebImageFile { get; set; }
        public string? MobileImageFile { get; set; }

        public List<AppSlideTranslationCreateEditDto> Translations { get; set; }
    }
}
