using Economy.Panel.UI.Models.SlideViewModels.AppSlideLanguageViewModels;

namespace Economy.Panel.UI.Models.SlideViewModels
{
    public class AppSlideCreateEditViewModel
    {
        public int Id { get; set; }
        public int Sequence { get; set; }
        public string? ThumbnailBase64 { get; set; }
        public string? ThumbnailMobilBase64 { get; set; }

        public List<AppSlideLanguageCreateEditViewModel> Translations { get; set; } = new();
    }
}
