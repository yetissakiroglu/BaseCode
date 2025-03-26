namespace Economy.Application.Dtos.AppPageDtos
{
    public class AppPageSectionDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Content { get; set; }
        public int Sequence { get; set; }
        public bool IsVisible { get; set; }
        public int AppPageId { get; set; }
        public int AppSectionId { get; set; }
        public AppSectionDto AppSection { get; set; } = new();

    }
}
