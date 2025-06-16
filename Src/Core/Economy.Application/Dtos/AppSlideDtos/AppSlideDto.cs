using Economy.Panel.Application.Dtos.AppSlideDtos.SlideTranslationDtos;

namespace Economy.Panel.Application.Dtos.AppSlideDtos
{
    public class AppSlideDto
    {
        public int Id { get; set; }
        public int Sequence { get; set; }
        public string? WebImageFile { get; set; }
        public string? MobileImageFile { get; set; }
        public List<AppSlideTranslationDto> Translations { get; set; } = new();

    }
}
