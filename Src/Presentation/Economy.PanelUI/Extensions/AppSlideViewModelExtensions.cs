using Economy.Panel.Application.Dtos.AppLanguageDtos;
using Economy.Panel.Application.Dtos.AppSlideDtos;
using Economy.Panel.Application.Dtos.AppSlideDtos.SlideTranslationDtos;
using Economy.Panel.UI.Models.SlideViewModels;
using Economy.Panel.UI.Models.SlideViewModels.AppSlideLanguageViewModels;

namespace Economy.Panel.UI.Extensions
{
    /// <summary>
    /// Extension methods for mapping and creating ViewModel and DTO objects related to AppSlide.
    /// </summary>
    public static class AppSlideMappingExtensions
    {
        /// <summary>
        /// Creates an empty AppSlideCreateEditViewModel with translations initialized for all languages.
        /// </summary>
        /// <param name="languages">List of available languages.</param>
        /// <returns>Initialized AppSlideCreateEditViewModel with empty translations.</returns>
        public static AppSlideCreateEditViewModel ToEmptyCreateEditViewModel(this IEnumerable<AppLanguageDto> languages)
        {
            return new AppSlideCreateEditViewModel
            {
                Translations = languages.Select(lang => new AppSlideLanguageCreateEditViewModel
                {
                    AppLanguageId = lang.Id,
                    Name = lang.Name,
                    Icon = lang.Icon
                }).ToList()
            };
        }

        /// <summary>
        /// Maps AppSlideDto to AppSlideCreateEditViewModel including all languages and their translations.
        /// </summary>
        /// <param name="languages">List of all supported languages.</param>
        /// <param name="slide">Slide data transfer object.</param>
        /// <returns>Mapped AppSlideCreateEditViewModel.</returns>
        public static AppSlideCreateEditViewModel ToCreateEditViewModel(this IEnumerable<AppLanguageDto> languages, AppSlideDto slide)
        {
            var translations = languages.Select(lang =>
            {
                var translation = slide.Translations.FirstOrDefault(t => t.AppLanguageId == lang.Id);

                return new AppSlideLanguageCreateEditViewModel
                {
                    Id = translation?.Id ?? 0,
                    Icon = lang.Icon,
                    Name = lang.Name,
                    AppSlideId = slide.Id,
                    AppLanguageId = lang.Id,
                    Title = translation?.Title ?? string.Empty,
                    Content = translation?.Content ?? string.Empty,
                    IsExternal = translation?.IsExternal ?? false,
                    ButtonText = translation?.ButtonText ?? string.Empty,
                    ButtonUrl = translation?.ButtonUrl ?? string.Empty,
                    ButtonIcon = translation?.ButtonIcon ?? string.Empty
                };
            }).ToList();

            return new AppSlideCreateEditViewModel
            {
                Id = slide.Id,
                Sequence = slide.Sequence,
                ThumbnailBase64 = slide.ThumbnailBase64,
                ThumbnailMobilBase64 = slide.ThumbnailMobilBase64,
                Translations = translations
            };
        }

        /// <summary>
        /// Maps a collection of AppSlideDto to a list of AppSlideListViewModel including all language translations.
        /// </summary>
        /// <param name="slides">Slides DTO list.</param>
        /// <param name="languages">All supported languages.</param>
        /// <returns>List of AppSlideListViewModel.</returns>
        public static List<AppSlideListViewModel> ToListViewModel(this IEnumerable<AppSlideDto> slides, IEnumerable<AppLanguageDto> languages)
        {
            return slides.Select(slide => new AppSlideListViewModel
            {
                Id = slide.Id,
                Sequence = slide.Sequence,
                ThumbnailBase64 = slide.ThumbnailBase64,
                ThumbnailMobilBase64 = slide.ThumbnailMobilBase64,
                Translations = languages.Select(lang =>
                {
                    var translation = slide.Translations.FirstOrDefault(t => t.AppLanguageId == lang.Id);
                    return new AppSlideLanguageViewModel
                    {
                        Id = translation?.Id ?? 0,
                        AppSlideId = translation?.AppSlideId ?? slide.Id,
                        AppLanguageId = lang.Id,
                        Icon = lang.Icon,
                        Name = lang.Name,
                        Title = translation?.Title ?? string.Empty,
                        Content = translation?.Content,
                        IsExternal = translation?.IsExternal ?? false,
                        ButtonText = translation?.ButtonText,
                        ButtonUrl = translation?.ButtonUrl,
                        ButtonIcon = translation?.ButtonIcon
                    };
                }).ToList()
            }).ToList();
        }

        /// <summary>
        /// Maps AppSlideDto to AppSlideViewModel.
        /// </summary>
        /// <param name="slide">Slide DTO.</param>
        /// <returns>AppSlideViewModel.</returns>
        public static AppSlideViewModel ToViewModel(this AppSlideDto slide)
        {
            return new AppSlideViewModel
            {
                Id = slide.Id,
                Sequence = slide.Sequence,
                ThumbnailBase64 = slide.ThumbnailBase64,
                ThumbnailMobilBase64 = slide.ThumbnailMobilBase64,
                Translations = slide.Translations.Select(t => new AppSlideLanguageViewModel
                {
                    Id = t.Id,
                    AppSlideId = t.AppSlideId,
                    AppLanguageId = t.AppLanguageId,
                    Title = t.Title,
                    Content = t.Content,
                    IsExternal = t.IsExternal,
                    ButtonText = t.ButtonText,
                    ButtonUrl = t.ButtonUrl,
                    ButtonIcon = t.ButtonIcon
                }).ToList()
            };
        }

        /// <summary>
        /// Maps AppSlideCreateEditViewModel to AppSlideCreateEditDto.
        /// </summary>
        /// <param name="model">View model to map.</param>
        /// <returns>Mapped DTO.</returns>
        public static AppSlideCreateEditDto ToDto(this AppSlideCreateEditViewModel model)
        {
            return new AppSlideCreateEditDto
            {
                Id = model.Id,
                Sequence = model.Sequence,
                ThumbnailBase64 = model.ThumbnailBase64,
                ThumbnailMobilBase64 = model.ThumbnailMobilBase64,
                Translations = model.Translations.Select(t => new AppSlideLanguageCreateEditDto
                {
                    Id = t.Id,
                    AppSlideId = t.AppSlideId,
                    AppLanguageId = t.AppLanguageId,
                    Title = t.Title,
                    Content = t.Content,
                    IsExternal = t.IsExternal,
                    ButtonText = t.ButtonText,
                    ButtonUrl = t.ButtonUrl,
                    ButtonIcon = t.ButtonIcon
                }).ToList()
            };
        }
    }

}
