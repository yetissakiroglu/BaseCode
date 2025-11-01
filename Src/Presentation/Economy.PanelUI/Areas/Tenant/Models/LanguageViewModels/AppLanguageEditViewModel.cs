using System.ComponentModel;

namespace Economy.Panel.UI.Areas.Tenant.Models.LanguageViewModels
{
    public class AppLanguageEditViewModel
    {
        [DisplayName("ID")]
        public int Id { get; set; }

        [DisplayName("Dil Kodu")]
        public string Code { get; set; }

        [DisplayName("Dil Adı")]
        public string Name { get; set; }

        [DisplayName("Sağdan Sola Yazım (RTL)")]
        public bool IsRTL { get; set; }

        [DisplayName("Simge (Icon)")]
        public string Icon { get; set; }

        [DisplayName("Aktif mi?")]
        public bool IsActive { get; set; }

        [DisplayName("Varsayılan Dil mi?")]
        public bool IsDefault { get; set; }
    }
}
