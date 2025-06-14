namespace Economy.Panel.UI.Models.ContentViewModels.AppContentTranslationViewModels
{
    public class AppContentTranslationViewModel
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
        //Dil
        public int AppLanguageId { get; set; }
        public string? LanguageName { get; set; }
        public string? LanguageIcon { get; set; }
        public bool LanguageIsDefault { get; set; }
    }
}
