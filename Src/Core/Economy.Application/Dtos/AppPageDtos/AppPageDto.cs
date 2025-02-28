using Economy.Domain.Models;

namespace Economy.Application.Dtos.AppPageDtos
{

    public class AppPageDto
    {
        public string Title { get; set; }
        public string? ShortDescription { get; set; }
        public string Slug { get; set; }
        public string? Content { get; set; }
        public string? MetaTitle { get; set; }
        public string? MetaDescription { get; set; }
        public bool IsHomePage { get; set; }
        public List<BreadcrumbDto> Breadcrumbs { get; set; } = new();
        public List<AppPageSectionDto> AppPageSections { get; set; } = new();
    }




}
