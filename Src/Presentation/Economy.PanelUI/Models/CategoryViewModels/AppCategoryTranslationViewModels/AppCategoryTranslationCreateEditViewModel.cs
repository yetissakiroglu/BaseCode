namespace Economy.Panel.UI.Models.CategoryViewModels.AppCategoryTranslationViewModels
{
    public class AppCategoryTranslationCreateEditViewModel
    {
        public int Id { get; set; }
        public int AppCategoryId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Url { get; set; } = string.Empty;
        public string? ShortDescription { get; set; }
        public string? Content { get; set; }
        public string? MetaTitle { get; set; }
        public string? MetaDescription { get; set; }

        //Dil
        public int AppLanguageId { get; set; }
        public string? LanguageName { get; set; }
        public string? LanguageIcon { get; set; }
        public bool LanguageIsDefault { get; set; }

    }
}
