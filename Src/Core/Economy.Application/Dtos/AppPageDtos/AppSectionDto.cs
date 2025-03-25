using Economy.Domain.Enums;

namespace Economy.Application.Dtos.AppPageDtos
{
    public class AppSectionDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Content { get; set; }
        public SectionType SectionType { get; set; }
    }
}
