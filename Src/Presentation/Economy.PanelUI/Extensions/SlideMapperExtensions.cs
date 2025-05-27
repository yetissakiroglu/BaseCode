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
    public static class SlideMapperExtensions
    {
        public static List<AppSlideListViewModel> MapToListViewModel(this IEnumerable<AppSlideDto> slides,IEnumerable<AppLanguageDto> languages)
        {
            return slides.Select(slide => new AppSlideListViewModel
            {
                Id = slide.Id,
                Sequence = slide.Sequence,
                WebImageFile = slide.WebImageFile,
                MobileImageFile = slide.MobileImageFile,
                Translations = languages.Select(lang => MapToTranslationViewModel(lang, slide.Translations)).ToList()
            }).ToList();
        }

        public static AppSlideCreateEditViewModel MapToEditViewModel(this AppSlideDto dto,IEnumerable<AppLanguageDto> languages)
        {
            return new AppSlideCreateEditViewModel
            {
                Id = dto.Id,
                Sequence = dto.Sequence,
                WebImageFile = dto.WebImageFile,
                MobileImageFile = dto.MobileImageFile,
                Translations = languages.Select(lang => MapToCreateEditTranslationViewModel(lang, dto.Translations)).ToList()
            };
        }

        public static AppSlideCreateEditDto MapToDto(this AppSlideCreateEditViewModel viewModel)
        {
            return new AppSlideCreateEditDto
            {
                Id = viewModel.Id,
                Sequence = viewModel.Sequence,
                WebImageFile = viewModel.WebImageFile,
                MobileImageFile = viewModel.MobileImageFile,
                ThumbnailBase64 = viewModel.ThumbnailBase64,
                ThumbnailMobilBase64 =viewModel.ThumbnailMobilBase64,
                Translations = viewModel.Translations.Select(MapToCreateEditDto).ToList()
            };
        }

        public static AppSlideCreateEditViewModel ToEmptyCreateEditViewModel(this IEnumerable<AppLanguageDto> languages)
        {
            return new AppSlideCreateEditViewModel
            {
                Translations = languages.Select(lang => new AppSlideTranslationCreateEditViewModel
                {
                    AppLanguageId = lang.Id,
                    LanguageName = lang.Name,
                    LanguageIcon = lang.Icon,
                    LanguageIsDefault = lang.IsDefault
                }).ToList()
            };
        }

        public static AppSlideViewModel ToViewModel(this AppSlideDto slide)
        {
            return new AppSlideViewModel
            {
                Id = slide.Id,
                Sequence = slide.Sequence,
                WebImageFile = slide.WebImageFile,
                MobilImageFile = slide.MobileImageFile,
                Translations = slide.Translations.Select(t => new AppSlideTranslationViewModel
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

        #region Private Helpers

        private static AppSlideTranslationViewModel MapToTranslationViewModel(AppLanguageDto lang, IEnumerable<AppSlideTranslationDto> translations)
        {
            var t = translations.FirstOrDefault(x => x.AppLanguageId == lang.Id);
            return new AppSlideTranslationViewModel
            {
                Id = t?.Id ?? 0,
                AppSlideId = t?.AppSlideId ?? 0,
                AppLanguageId = lang.Id,
                LanguageName = lang.Name,
                LanguageIcon = lang.Icon,
                Title = t?.Title ?? string.Empty,
                Content = t?.Content ?? string.Empty,
                ButtonText = t?.ButtonText ?? string.Empty,
                ButtonUrl = t?.ButtonUrl ?? string.Empty,
                ButtonIcon = t?.ButtonIcon ?? string.Empty,
                IsExternal = t?.IsExternal ?? false,
                LanguageIsDefault = lang.IsDefault
            };
        }

        private static AppSlideTranslationCreateEditViewModel MapToCreateEditTranslationViewModel(AppLanguageDto lang, IEnumerable<AppSlideTranslationDto> translations)
        {
            var t = translations.FirstOrDefault(x => x.AppLanguageId == lang.Id);
            return new AppSlideTranslationCreateEditViewModel
            {
                Id = t?.Id ?? 0,
                AppSlideId = t?.AppSlideId ?? 0,
                AppLanguageId = lang.Id,
                LanguageName = lang.Name,
                LanguageIcon = lang.Icon,
                Title = t?.Title ?? string.Empty,
                Content = t?.Content ?? string.Empty,
                ButtonText = t?.ButtonText ?? string.Empty,
                ButtonUrl = t?.ButtonUrl ?? string.Empty,
                ButtonIcon = t?.ButtonIcon ?? string.Empty,
                IsExternal = t?.IsExternal ?? false,
                LanguageIsDefault = lang.IsDefault
            };
        }

        private static AppSlideTranslationCreateEditDto MapToCreateEditDto(AppSlideTranslationCreateEditViewModel vm)
        {
            return new AppSlideTranslationCreateEditDto
            {
                Id = vm.Id,
                AppSlideId = vm.AppSlideId,
                AppLanguageId = vm.AppLanguageId,
                Title = vm.Title,
                Content = vm.Content,
                ButtonText = vm.ButtonText,
                ButtonUrl = vm.ButtonUrl,
                ButtonIcon = vm.ButtonIcon,
                IsExternal = vm.IsExternal
            };
        }

        #endregion
    }


}
