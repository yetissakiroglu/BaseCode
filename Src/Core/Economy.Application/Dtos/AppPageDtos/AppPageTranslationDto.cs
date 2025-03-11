namespace Economy.Application.Dtos.AppPageDtos
{
    public class AppPageTranslationDto
    {
        public int AppLanguageId { get; set; }
        public string Title { get; set; } = null!;
        public string? ShortDescription { get; set; }
        public string? Content { get; set; }
        public string? MetaTitle { get; set; }
        public string? MetaDescription { get; set; }

    }
}
