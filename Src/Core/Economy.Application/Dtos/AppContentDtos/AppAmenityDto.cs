using Economy.Domain.Entites.EntityPages;

namespace Economy.Application.Dtos.PageDtos
{
    public record AppAmenityDto
    {
        public int Id { get; set; }
        public int AppAmenityGroupId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        // Entity'den DTO'ya dönüştürme metodu
        public static AppAmenityDto FromEntity(AppAmenity entity, string languageCode)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));

            //var translation = entity.AppPageTranslations.FirstOrDefault(t => t.LanguageCode == languageCode);

            //if (translation == null)
            //{
            //    translation = new AppPageTranslation();

            //    //return AppPageDto();
            //    //throw new InvalidOperationException("No translation found for the specified language or default language.");
            //}

            return new AppAmenityDto
            {
                Id = entity.Id,
                Description = entity.Description,
                AppAmenityGroupId = entity.AppAmenityGroupId,
                Name = entity.Name
            };
        }


    }
}
