namespace Economy.Application.Dtos.AppContentDtos
{
    public class AppContentTranslationDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string? ShortDescription { get; set; }
        public string? Content { get; set; }
        public bool IsExternal { get; set; }
        public string Url { get; set; }
        public string? MetaTitle { get; set; }
        public string? MetaDescription { get; set; }
        public int AppLanguageId { get; set; }
    }
}
