namespace Economy.Application.TenantUI.Dtos.AppSlideDtos.SlideTranslationDtos
{
    public class AppSlideTranslationCreateEditDto
    {
        public int Id { get; set; }
        public int AppSlideId { get; set; }
        public int AppLanguageId { get; set; }
        public string Title { get; set; }
        public string? Content { get; set; }
        public bool IsExternal { get; set; }
        public string? ButtonText { get; set; }
        public string? ButtonUrl { get; set; }
        public string? ButtonIcon { get; set; }
    }
}
