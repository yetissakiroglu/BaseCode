namespace Economy.Application.TenantUI.Dtos.AppPageDtos
{
    public class PageListDto
    {
        public int Id { get; set; }
        public string? ParentTitle { get; set; }
        public string? Title { get; set; }
        public required string Slug { get; set; }
        public bool IsHomepage { get; set; }
        public bool IsActive { get; set; }
        public DateTime? PublishAtUtc { get; set; }
        public int SortOrder { get; set; }
    }
}
