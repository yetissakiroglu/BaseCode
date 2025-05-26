namespace Economy.Panel.UI.Models.SlideViewModels.AppSlideLanguageViewModels
{
    public class AppSlideTranslationCreateEditViewModel
    {
        public int Id { get; set; }
        public int AppSlideId { get; set; }
        public string Title { get; set; }
        public string? Content { get; set; }
        public bool IsExternal { get; set; }
        public string? ButtonText { get; set; }
        public string? ButtonUrl { get; set; }
        public string? ButtonIcon { get; set; }

        //Dil
        public int AppLanguageId { get; set; }
        public string? LanguageName { get; set; }
        public string? LanguageIcon { get; set; }
        public bool LanguageIsDefault { get; set; }

    }
}
