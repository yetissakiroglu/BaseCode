using Economy.Panel.Application.Dtos.AppSlideDtos.SlideTranslationDtos;

namespace Economy.Panel.Application.Dtos.AppSlideDtos
{
    public class AppSlideCreateEditDto
    {
        public int Id { get; set; }
        public int Sequence { get; set; }
        public string? ThumbnailBase64 { get; set; }
        public string? ThumbnailMobilBase64 { get; set; }
        public List<AppSlideLanguageCreateEditDto> Translations { get; set; }
    }
}
