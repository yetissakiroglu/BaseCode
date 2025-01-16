using Economy.Domain.Entites.EntityPages;

namespace Economy.Application.Dtos.PageDtos
{
    public record AppImageDto
    {
        public int Id { get; set; }
        public int AppImageGroupId { get; set; } // Odanın ID'si
        public string ImageUrl { get; set; }
        public bool IsCover { get; set; } // Kapak resim mi değil mi?

        // Entity'den DTO'ya dönüştürme metodu
        public static AppImageDto FromEntity(AppImage entity, string languageCode)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));

            //var translation = entity.AppPageTranslations.FirstOrDefault(t => t.LanguageCode == languageCode);

            //if (translation == null)
            //{
            //    translation = new AppPageTranslation();

            //    //return AppPageDto();
            //    //throw new InvalidOperationException("No translation found for the specified language or default language.");
            //}

            return new AppImageDto
            {
                Id = entity.Id,
                AppImageGroupId = entity.AppImageGroupId,
                ImageUrl = entity.ImageUrl,
                IsCover = entity.IsCover

            };
        }
    }
}
