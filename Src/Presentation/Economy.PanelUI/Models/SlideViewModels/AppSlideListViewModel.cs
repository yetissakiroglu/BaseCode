using Economy.Panel.UI.Models.SlideViewModels.AppSlideLanguageViewModels;
using System.ComponentModel;

namespace Economy.Panel.UI.Models.SlideViewModels
{
    public class AppSlideListViewModel
    {
        [DisplayName("ID")]
        public int Id { get; set; }

        [DisplayName("Sıra")]
        public int Sequence { get; set; }

        [DisplayName("Küçük Görsel (Base64)")]
        public string? ThumbnailBase64 { get; set; }

        [DisplayName("Mobil Küçük Görsel (Base64)")]
        public string? ThumbnailMobilBase64 { get; set; }

        [DisplayName("Dil Çevirileri")]
        public List<AppSlideLanguageViewModel> Translations { get; set; } = new();
    }
}
