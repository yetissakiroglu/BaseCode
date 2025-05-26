using System.ComponentModel;

namespace Economy.Panel.UI.Models.SlideViewModels.AppSlideLanguageViewModels
{
    public class AppSlideTranslationViewModel
    {
        [DisplayName("ID")]
        public int Id { get; set; }
        [DisplayName("Dil ID")]
        public int AppLanguageId { get; set; }

        [DisplayName("Slide ID")]
        public int AppSlideId { get; set; }

        [DisplayName("Başlık")]
        public string Title { get; set; }

        [DisplayName("İçerik")]
        public string? Content { get; set; }

        [DisplayName("Harici Bağlantı")]
        public bool IsExternal { get; set; }

        [DisplayName("Buton Metni")]
        public string? ButtonText { get; set; }

        [DisplayName("Buton URL")]
        public string? ButtonUrl { get; set; }

        [DisplayName("Buton İkonu")]
        public string? ButtonIcon { get; set; }

        [DisplayName("Dil Adı")]
        public string? LanguageName { get; set; }

        [DisplayName("Dil İkonu")]
        public string? LanguageIcon { get; set; }
        public bool LanguageIsDefault { get; set; }

    }
}
