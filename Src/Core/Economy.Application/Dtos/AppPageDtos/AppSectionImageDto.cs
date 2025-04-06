namespace Economy.Application.Dtos.AppPageDtos
{
    public class AppSectionImageDto
    {
        public int AppSectionId { get; set; }
        public int Sequence { get; set; }
        public string Name { get; set; }
        public string? Content { get; set; }
        public string? Thumbnail { get; set; }
    }
}
