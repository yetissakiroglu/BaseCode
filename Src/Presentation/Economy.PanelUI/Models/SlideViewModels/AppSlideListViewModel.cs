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

        [DisplayName("Küçük Görsel")]
        public string? WebImageFile { get; set; }

        [DisplayName("Mobil Küçük Görsel")]
        public string? MobileImageFile { get; set; }

        [DisplayName("Dil Çevirileri")]
        public List<AppSlideTranslationViewModel> Translations { get; set; } = new();
    }
}
