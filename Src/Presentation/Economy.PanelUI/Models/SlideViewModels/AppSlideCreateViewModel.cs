namespace Economy.Panel.UI.Models.SlideViewModels
{
    public class AppSlideCreateViewModel
    {
        //public int Id { get; set; }
        public int Sequence { get; set; }
        public string? ThumbnailBase64 { get; set; }
        public string? ThumbnailMobilBase64 { get; set; }

        public List<AppSlideLanguageCreateViewModel> Translations { get; set; } = new();
    }
}
