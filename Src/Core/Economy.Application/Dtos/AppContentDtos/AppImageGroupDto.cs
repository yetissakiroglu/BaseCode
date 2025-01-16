using Economy.Domain.Entites.EntityPages;

namespace Economy.Application.Dtos.PageDtos
{
    public record AppImageGroupDto
    {
        public int Id { get; set; }
        public int AppContentId { get; set; } // Odanın ID'si
        public string Name { get; set; }
        public string Description { get; set; }
        public List<AppImageDto> AppImages { get; set; }
        // Entity'den DTO'ya dönüştürme metodu
        public static AppImageGroupDto FromEntity(AppImageGroup entity, string languageCode)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));

            //var translation = entity.AppPageTranslations.FirstOrDefault(t => t.LanguageCode == languageCode);

            //if (translation == null)
            //{
            //    translation = new AppPageTranslation();

            //    //return AppPageDto();
            //    //throw new InvalidOperationException("No translation found for the specified language or default language.");
            //}

            return new AppImageGroupDto
            {
                Id = entity.Id,
                Name = entity.Name,
                Description = entity.Description,
                AppContentId = entity.AppContentId,
                AppImages = entity.AppImages?.Select(section => AppImageDto.FromEntity(section, languageCode)).ToList() ?? new()

            };
        }
    }
}

