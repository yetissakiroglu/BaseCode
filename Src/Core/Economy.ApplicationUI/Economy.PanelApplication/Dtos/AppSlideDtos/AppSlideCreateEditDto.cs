using Economy.Panel.Application.Dtos.AppSlideDtos.SlideTranslationDtos;
using Microsoft.AspNetCore.Http;

namespace Economy.Panel.Application.Dtos.AppSlideDtos
{
    public class AppSlideCreateEditDto
    {
        public int Id { get; set; }
        public int Sequence { get; set; }
        public string? ThumbnailBase64 { get; set; }
        public string? ThumbnailMobilBase64 { get; set; }
        public IFormFile WebCoverImageFile { get; set; }
        public IFormFile MobileCoverImageFile { get; set; }

        public List<AppSlideLanguageCreateEditDto> Translations { get; set; }
    }
}
