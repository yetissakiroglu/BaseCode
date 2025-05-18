namespace Economy.Panel.UI.Models.SlideViewModels
{
    public class AppSlideLanguageEditViewModel
    {
        public int Id { get; set; }
        public int AppSlideId { get; set; }
        public string Title { get; set; }
        public string? Content { get; set; }
        public bool IsExternal { get; set; }
        public string? ButtonText { get; set; }
        public string? ButtonUrl { get; set; }
        public string? ButtonIcon { get; set; }

        //Dil
        public int AppLanguageId { get; set; }
        public string? Code { get; set; }
        public string? Name { get; set; }
        public bool? IsRTL { get; set; }
        public string? Icon { get; set; }
    }
}
