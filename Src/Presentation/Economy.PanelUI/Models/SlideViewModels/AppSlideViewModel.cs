using Economy.Panel.UI.Models.SlideViewModels.AppSlideLanguageViewModels;

namespace Economy.Panel.UI.Models.SlideViewModels
{
    public class AppSlideViewModel
    {
        public int Id { get; set; }
        public int Sequence { get; set; }
        public string? WebImageFile { get; set; }
        public string? MobilImageFile { get; set; }
        public List<AppSlideTranslationViewModel> Translations { get; set; } = new();
    }
}
