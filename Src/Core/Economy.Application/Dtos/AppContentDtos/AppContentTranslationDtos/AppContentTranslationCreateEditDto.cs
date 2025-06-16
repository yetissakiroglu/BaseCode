namespace Economy.Panel.Application.Dtos.AppContentDtos.AppContentTranslationDtos
{
    public class AppContentTranslationCreateEditDto
    {
        public int Id { get; set; }
        public int AppContentId { get; set; }
        public string Title { get; set; }
        public string? ShortDescription { get; set; }
        public string? Content { get; set; }
        public bool IsExternal { get; set; }
        public string Url { get; set; }

        // SEO alanları
        public string? MetaTitle { get; set; }
        public string? MetaDescription { get; set; }
        public int AppLanguageId { get; set; }

    }
}
