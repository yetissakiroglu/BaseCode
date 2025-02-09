using Economy.Domain.Enums;

namespace Economy.Application.Dtos.AppPageDtos
{
    public class AppPageSectionDto
    {
        public required string Name { get; set; }
        public string? Content { get; set; }
        public bool IsVisible { get; set; }
        public SectionType SectionType { get; set; }
        public int AppPageId { get; set; }
        public int AppSectionId { get; set; }
    }
   
}
