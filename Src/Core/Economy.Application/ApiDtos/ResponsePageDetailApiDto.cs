using Economy.Domain.Entites.EntityAppPages;
using Economy.Domain.Enums;

namespace Economy.Application.ApiDtos
{
    public record ResponsePageDetailApiDto
    {
        public int Id { get; init; }
        public string Title { get; set; }
        public string? ShortDescription { get; set; }
        public string? Content { get; set; }
        public string Url { get; set; }
        public bool IsHomePage { get; set; }
        public string? MetaTitle { get; set; }
        public string? MetaDescription { get; set; }
        public object Breadcrumbs { get; set; }



        public List<ResponsePageSectionApiDto> AppPageSections { get; set; } = new();

        // Entity'den DTO'ya dönüştürme metodu
        public static ResponsePageDetailApiDto FromEntity(AppPage entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));

            return new ResponsePageDetailApiDto
            {
                Id = entity.Id,
                Title = entity.Title,
                ShortDescription = entity.ShortDescription,
                Content = entity.Content,
                Url = entity.GetUrl(),
                IsHomePage = entity.IsHomePage,
                Breadcrumbs = entity.GetBreadcrumbs(),
                AppPageSections = entity.AppPageSections?.Select(section => ResponsePageSectionApiDto.FromEntity(section)).ToList() ?? new()
            };
        }
    }


    public record ResponsePageSectionApiDto
    {
        public int Id { get; init; }
        public int AppPageId { get; set; }
        public int AppSectionId { get; set; }
        public ResponseSectionApiDto AppSection { get; set; } = new();
        public string Name { get; set; }
        public string Content { get; set; }
        public bool IsVisible { get; set; }
        public SectionType SectionType { get; set; }


        // Entity'den DTO'ya dönüştürme metodu
        public static ResponsePageSectionApiDto FromEntity(AppPageSection entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));

            return new ResponsePageSectionApiDto
            {
                Id = entity.Id,
                AppPageId = entity.AppPageId,
                AppSectionId = entity.AppSectionId,
                // AppSection null ise null döndürülmesi sağlanıyor
                AppSection = entity.AppSection != null ? ResponseSectionApiDto.FromEntity(entity.AppSection) : null,
                Name = entity.Name,
                Content = entity.Content,
                IsVisible = entity.IsVisible,
                SectionType = entity.SectionType,
            };
        }
    }


    public record ResponseSectionApiDto
    {
        public int Id { get; init; }
        public string Name { get; set; }
        public string? Content { get; set; }
        public SectionType SectionType { get; set; }

        // Entity'den DTO'ya dönüştürme metodu
        public static ResponseSectionApiDto FromEntity(AppSection entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));

            return new ResponseSectionApiDto
            {
                Id = entity.Id,
                Name = entity.Name,
                Content = entity.Content,
                SectionType = entity.SectionType
            };
        }
    }

}
