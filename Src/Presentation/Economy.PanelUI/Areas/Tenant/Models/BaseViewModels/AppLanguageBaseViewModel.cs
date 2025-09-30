namespace Economy.Panel.UI.Areas.Tenant.Models.BaseViewModels
{
    public abstract class AppLanguageBaseViewModel
    {
        public int? AppLanguageId { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public bool IsRTL { get; set; }
        public string Icon { get; set; }
    }
}
