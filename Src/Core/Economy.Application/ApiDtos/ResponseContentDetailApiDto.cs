using Economy.Domain.Entites.EntityAppContents;
using Economy.Domain.Entites.EntityPages;

namespace Economy.Application.ApiDtos
{
    public record ResponseContentDetailApiDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Url { get; set; }
        public string CategoryTitle { get; set; }
        public string CategoryUrl { get; set; }
        public string? ShortDescription { get; set; }
        public string? Content { get; set; }
        public object Breadcrumbs { get; set; }
        public object ImportantNotes { get; set; }
        public object Documents { get; set; }
        public static ResponseContentDetailApiDto FromEntity(AppContent model)
        {
            return new ResponseContentDetailApiDto
            {
                Id = model.Id,
                Title = model.Title,
                Url = model.GetUrl(),
                CategoryTitle = model.AppCategory.Name,
                CategoryUrl = model.AppCategory.GetUrlPath(),
                ShortDescription = model.ShortDescription,
                Content = model.Content,
                Breadcrumbs = model.GetBreadcrumbs(),
                ImportantNotes = ResponseImportantNotesApiDto.FromEntities([.. model.AppContent_ImportantNotes]),
                Documents =ResponseDocumentApiDto.FromEntities([.. model.AppContent_Documents])
            };
        }
        public static List<ResponseContentDetailApiDto> FromEntities(List<AppContent> modelList)
        {
            return modelList.Select(model => FromEntity(model)).ToList();
        }

    }

    public record ResponseImportantNotesApiDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string? Content { get; set; }
        public int Sequence { get; set; }

        public static ResponseImportantNotesApiDto FromEntity(AppContent_ImportantNote model)
        {
            return new ResponseImportantNotesApiDto
            {
                Id = model.Id,
                Title = model.Title,
                Content = model.Content,
                Sequence = model.Sequence
            };
        }
        public static List<ResponseImportantNotesApiDto> FromEntities(List<AppContent_ImportantNote> modelList)
        {
            return modelList.Select(model => FromEntity(model)).ToList();
        }

    }

    public record ResponseDocumentApiDto
    {
        public int Id { get; set; }
        public string DocumentId { get; set; }
        public string Title { get; set; }

        public static ResponseDocumentApiDto FromEntity(AppContent_Document model)
        {
            return new ResponseDocumentApiDto
            {
                Id = model.Id,
                Title = model.Title,
                DocumentId = model.DocumentId,
            };
        }
        public static List<ResponseDocumentApiDto> FromEntities(List<AppContent_Document> modelList)
        {
            return modelList.Select(model => FromEntity(model)).ToList();
        }

    }
}
