namespace Economy.Panel.UI.Models.SettingViewModels
{
    public class SettingLanguageViewModel
    {
        public int? Id { get; set; } // Çeviri ID'si    
        public int? AppSettingId { get; set; }
        public string SiteTitle { get; set; }
        public string Description { get; set; }
        public string MetaTitle { get; set; }
        public string MetaDescription { get; set; }

        public int? AppLanguageId { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public bool IsRTL { get; set; }
        public string Icon { get; set; }

    }
}
