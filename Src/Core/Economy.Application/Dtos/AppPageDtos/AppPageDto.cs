namespace Economy.Application.Dtos.AppPageDtos
{
    public class AppPageDto
    {
        public int Id { get; set; }
        public bool IsHomePage { get; set; }
        public List<AppPageTranslationDto> Translations { get; set; } = new();
        public List<AppPageSectionDto> AppPageSections { get; set; } = new();


    }
}
