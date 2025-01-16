using Economy.Domain.Entites.EntityPages;

namespace Economy.Application.Dtos.PageDtos
{
    public class AppAmenityGroupDto
    {
        public int Id { get; set; }
        public int AppContentId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public List<AppAmenityDto> AppAmenities { get; set; }

        public static AppAmenityGroupDto FromEntity(AppAmenityGroup entity, string languageCode)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));

            //var translation = entity.AppPageTranslations.FirstOrDefault(t => t.LanguageCode == languageCode);

            //if (translation == null)
            //{
            //    translation = new AppPageTranslation();

            //    //return AppPageDto();
            //    //throw new InvalidOperationException("No translation found for the specified language or default language.");
            //}

            return new AppAmenityGroupDto
            {
                Id = entity.Id,
                Name = entity.Name,
                Description = entity.Description,
                AppContentId = entity.AppContentId,
                AppAmenities = entity.AppAmenities?.Select(section => AppAmenityDto.FromEntity(section, languageCode)).ToList() ?? new()

            };
        }


    }
}
