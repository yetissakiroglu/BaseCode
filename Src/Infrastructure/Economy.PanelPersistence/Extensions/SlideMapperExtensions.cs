using Economy.Domain.Entites.EntitySlides;
using Economy.Panel.Application.Dtos.AppSlideDtos;
using Economy.Panel.Application.Dtos.AppSlideDtos.SlideTranslationDtos;

namespace Economy.Panel.Persistence.Extensions
{
    /// <summary>
    /// Extension methods for mapping and creating ViewModel and DTO objects related to AppSlide.
    /// </summary>
    public static class SlideMapper
    {
    
        public static AppSlide MapToEntity(this AppSlideCreateEditDto dto)
        {
            return new AppSlide
            {
                Id = dto.Id,
                Sequence = dto.Sequence,
                ThumbnailBase64 = dto.WebImageFile,
                ThumbnailMobilBase64 = dto.MobileImageFile,
                Translations = dto.Translations.Select(MapTranslationDtoToEntity).ToList()
            };
        }
        public static AppSlideDto MapToDto(this AppSlide entity)
        {
            return new AppSlideDto
            {
                Id = entity.Id,
                Sequence = entity.Sequence,
                WebImageFile = entity.ThumbnailBase64,
                MobileImageFile = entity.ThumbnailMobilBase64,
                Translations = entity.Translations.Select(MapTranslationEntityToDto).ToList()
            };
        }
        public static AppSlideDto MapSelectToDto(AppSlide entity)
        {
            return new AppSlideDto
            {
                Id = entity.Id,
                Sequence = entity.Sequence,
                WebImageFile = entity.ThumbnailBase64,
                MobileImageFile = entity.ThumbnailMobilBase64,
                Translations = entity.Translations.Select(MapTranslationEntityToDto).ToList()
            };
        }
     
        // ------------------------
        // Özel Yardımcı Metotlar
        // ------------------------

        private static AppSlideTranslationDto MapTranslationEntityToDto(AppSlideTranslation entity)
        {
            return new AppSlideTranslationDto
            {
                Id = entity.Id,
                AppSlideId = entity.AppSlideId,
                AppLanguageId = entity.AppLanguageId,
                Title = entity.Title,
                Content = entity.Content,
                ButtonText = entity.ButtonText,
                ButtonUrl = entity.ButtonUrl,
                ButtonIcon = entity.ButtonIcon,
                IsExternal = entity.IsExternal
            };
        }
        private static AppSlideTranslation MapTranslationDtoToEntity(AppSlideTranslationCreateEditDto dto)
        {
            return new AppSlideTranslation
            {
                Id = dto.Id,
                AppSlideId = dto.AppSlideId,
                AppLanguageId = dto.AppLanguageId,
                Title = dto.Title,
                Content = dto.Content,
                ButtonText = dto.ButtonText,
                ButtonUrl = dto.ButtonUrl,
                ButtonIcon = dto.ButtonIcon,
                IsExternal = dto.IsExternal
            };
        }
      
    }

}
