namespace Economy.Panel.UI.Models.SettingViewModels
{
    public class SettingViewModel
    {
        public int Id { get; set; }
        public List<SettingLanguageViewModel> Translations { get; set; } = new();
    }
}
