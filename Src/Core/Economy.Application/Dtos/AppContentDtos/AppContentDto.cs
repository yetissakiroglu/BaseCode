using Economy.Application.Dtos.AppContentDtos;
using Economy.Domain.Entites.EntityPages;

namespace Economy.Application.Dtos.PageDtos
{
    public record AppContentDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string? ShortDescription { get; set; }
        public string Slug { get; set; }
        public string? Content { get; set; }
        public string? MetaTitle { get; set; }
        public string? MetaDescription { get; set; }
        public string? MetaKeywords { get; set; }
        public int? CategoryId { get; set; }
        public string? CategoryTitle { get; set; }
        public List<AppContent_ImportantNoteDto> ImportantNotes { get; set; }


        public static AppContentDto FromEntity(AppContent entity)
        {
            if (entity == null)
            {
               return new AppContentDto();
            };


            var result = new AppContentDto
            {
                Id = entity.Id,
                Slug = entity.Slug,
                Title = entity.Title,
                Content = entity.Content,
                CategoryId = entity.AppCategoryId,
                CategoryTitle = entity.AppCategory.Name,
                MetaDescription = entity.MetaDescription,
                ShortDescription = entity.ShortDescription,
                MetaKeywords = entity.MetaKeywords,
                MetaTitle = entity.MetaTitle,
                ImportantNotes = new List<AppContent_ImportantNoteDto>()
            };

            result.ImportantNotes = AppContent_ImportantNoteDto.List(entity.AppContent_ImportantNotes.ToList());

            return result;
        }
        // ListModel Özellikleri
        public static List<AppContentDto> List(List<AppContent> entities)
        {
            return entities.Select(entity => FromEntity(entity)).ToList();
        }



    }
}
