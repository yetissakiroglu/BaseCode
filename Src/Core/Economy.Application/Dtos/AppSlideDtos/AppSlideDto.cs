using Economy.Domain.Entites.EntitySlides;

namespace Economy.Application.Dtos.SlideDtos
{
    public record AppSlideDto
    {

        public int Id { get; init; }  // Nullable yerine zorunlu hale getirildi
        public string Title { get; init; }
        public string Content { get; init; }
        public string LanguageCode { get; init; }
        public bool IsExternal { get; init; } // IsDefault alanı eklendi
        public string URL { get; set; } // Harici bağlantı için URL
        public string ImageUrl { get; set; }
        public int Sequence { get; set; }
        public int AppSectionId { get; set; }


        // Entity'den DTO'ya dönüştürme metodu
        public static AppSlideDto FromEntity(AppSlide entity, string languageCode)
        {
            var translation = entity.AppSlideTranslations.FirstOrDefault(t => t.LanguageCode == languageCode);

            if (translation == null)
            {
                throw new InvalidOperationException("No translation found for the specified language or default language.");
            }

            return new AppSlideDto
            {
                Id = entity.Id,
                Title = translation.Title,
                Content = translation.Content,
                LanguageCode = translation.LanguageCode,
                IsExternal = translation.IsExternal,
                URL = translation.URL,
                ImageUrl = translation.ImageUrl,
                Sequence = entity.Sequence,
                AppSectionId = entity.AppSectionId,
            };
        }

        // ListModel Özellikleri
        public static List<AppSlideDto> List(List<AppSlide> entities, string languageCode)
        {
            return entities.Select(entity => FromEntity(entity, languageCode)).ToList();
        }

        // CreateModel Özellikleri
        public static AppSlide CreateEntity(string title, string content, string languageCode, bool isExternal,string url,string imageUrl,int sequence, int appSectionId)
        {
            var translation = new AppSlideTranslation
            {
                Title = title,
                Content = content,
                LanguageCode = languageCode,
                IsExternal = isExternal, // Dışarıdan gelen IsDefault değerini kullanıyoruz
                URL = url,
                ImageUrl = imageUrl,
            };
            return new AppSlide
            {
                AppSectionId = appSectionId,
                Sequence = sequence,
                AppSlideTranslations = new List<AppSlideTranslation> { translation }
            };
        }

        // EditModel Özellikleri
        public static AppSlide EditEntity(int id, string title, string content, string languageCode, bool isExternal, string url, string imageUrl, int sequence, int appSectionId)
        {
            var translation = new AppSlideTranslation
            {
                AppSlideId = id, // İlgili Slide'e ait
                Title = title,
                Content = content,
                LanguageCode = languageCode,
                IsExternal = isExternal, // Dışarıdan gelen IsDefault değerini kullanıyoruz
                URL = url,
                ImageUrl = imageUrl
            };

            return new AppSlide
            {
                Id = id,
                Sequence = sequence,
                AppSectionId = appSectionId,
                AppSlideTranslations = new List<AppSlideTranslation> { translation }
            };
        }
    }

}
