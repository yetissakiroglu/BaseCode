using Economy.Domain.Enums;

namespace Economy.Panel.Application.Dtos.AppCategoryDtos
{
    public class AppCategoryPagingDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Url { get; set; } = string.Empty;

        public ContentType ContentType { get; set; }
        public int? ParentCategoryId { get; set; }
        public string? ParentCategoryTitle { get; set; }

        public int AppLanguageId { get; set; }
    }
}
