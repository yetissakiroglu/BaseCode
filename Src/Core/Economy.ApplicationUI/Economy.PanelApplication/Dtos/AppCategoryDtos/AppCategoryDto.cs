using Economy.Domain.Enums;
using Economy.Panel.Application.Dtos.AppCategoryDtos.CategoryTranslationDtos;

namespace Economy.Panel.Application.Dtos.AppCategoryDtos
{
    public class AppCategoryDto
    {
        public int Id { get; set; }
        public ContentType ContentType { get; set; }
        public int? ParentCategoryId { get; set; }

        public List<AppCategoryTranslationDto> Translations { get; set; } = new();
        public List<AppCategoryDto> SubCategories { get; set; } = new();
    }
}
