using Economy.Panel.Application.Dtos.AppSlideDtos.SlideTranslationDtos;

namespace Economy.Panel.Application.Dtos.AppSlideDtos
{
    public class AppSlideListDto
    {
        public int Id { get; set; }
        public int Sequence { get; set; }
        public string? ThumbnailBase64 { get; set; }
        public string? ThumbnailMobilBase64 { get; set; }
        public List<SlideTranslationDto> Translations { get; set; }
    }
}
