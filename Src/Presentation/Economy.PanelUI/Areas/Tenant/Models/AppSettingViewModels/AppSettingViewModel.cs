using Economy.Panel.UI.Areas.Tenant.Models.BaseViewModels;

namespace Economy.Panel.UI.Areas.Tenant.Models.AppSettingViewModels
{
    public class AppSettingViewModel
    {
        public int Id { get; set; }
        public List<SettingLanguageViewModel> Translations { get; set; } = new();
    }
    public class SettingLanguageViewModel : AppLanguageBaseViewModel
    {
        public int? Id { get; set; } // Çeviri ID'si    
        public int? AppSettingId { get; set; }
        public string? SiteTitle { get; set; }
        public string? Description { get; set; }
        public string? MetaTitle { get; set; }
        public string? MetaDescription { get; set; }
    }



}
