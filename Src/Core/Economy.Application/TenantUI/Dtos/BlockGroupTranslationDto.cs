namespace Economy.Application.TenantUI.Dtos
{
    public class BlockGroupTranslationDto
    {
        public int? Id { get; set; }
        public int LanguageId { get; set; }
        public string LanguageCode { get; set; } = default!;
        public string? LanguageIcon { get; set; } = default!;
        public string Title { get; set; }
        public string? Description { get; set; }
    }
}
