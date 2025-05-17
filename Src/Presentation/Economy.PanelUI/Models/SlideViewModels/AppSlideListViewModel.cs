using Economy.Panel.UI.Models.SettingViewModels;

namespace Economy.Panel.UI.Models.SlideViewModels
{
    public class AppSlideListViewModel
    {
        public int Id { get; set; }
        public int Sequence { get; set; }
        public string? ThumbnailBase64 { get; set; }
        public string? ThumbnailMobilBase64 { get; set; }

        public List<AppSLideLanguageViewModel> Translations { get; set; } = new();

    }
}
