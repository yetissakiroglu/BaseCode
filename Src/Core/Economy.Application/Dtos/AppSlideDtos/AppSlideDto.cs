namespace Economy.Application.Dtos.AppSlideDtos
{
    public class AppSlideDto
    {
        public int Id { get; set; }
        public int Sequence { get; set; }
        public List<AppSlideTranslationDto> Translations { get; set; } = new();
    }
}
