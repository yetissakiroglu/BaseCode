using Economy.Domain.Entites.EntityAppContents;

namespace Economy.Application.Dtos.AppContentDtos
{
    public record AppContent_ImportantNoteDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string? Content { get; set; }
        public int Sequence { get; set; }


        public static AppContent_ImportantNoteDto FromEntity(AppContent_ImportantNote entity)
        {
            if (entity == null)
            {
                return new AppContent_ImportantNoteDto();
            };


            return new AppContent_ImportantNoteDto
            {
                Id = entity.Id,
                Title = entity.Title,
                Content = entity.Content,
                Sequence = entity.Sequence
            };



        }
        // ListModel Özellikleri
        public static List<AppContent_ImportantNoteDto> List(List<AppContent_ImportantNote> entities)
        {
            return entities.Select(entity => FromEntity(entity)).ToList();
        }


    }
}
