using Economy.Application.TenantUI.Dtos.AppSlideDtos.SlideTranslationDtos;

namespace Economy.Application.TenantUI.Dtos.AppSlideDtos
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
