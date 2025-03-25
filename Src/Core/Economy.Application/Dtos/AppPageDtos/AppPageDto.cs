namespace Economy.Application.Dtos.AppPageDtos
{
    public class AppPageDto: PageViewModel
    {
        public int Id { get; set; }
        public bool IsHomePage { get; set; }
        public string LanguageCode { get; set; } = null!;
        public List<AppPageTranslationDto> Translations { get; set; } = new();
        public List<AppPageSectionDto> AppPageSections { get; set; } = new();


    }
}
