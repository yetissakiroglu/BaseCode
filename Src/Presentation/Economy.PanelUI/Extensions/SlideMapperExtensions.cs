using Economy.Domain.Entites.EntitySlides;
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
    public static class SlideMapper
    {
        public static List<AppSlideListViewModel> MapToListViewModel(this IEnumerable<AppSlideDto> slides, IEnumerable<AppLanguageDto> languages)
        {
            return slides.Select(slide => new AppSlideListViewModel
            {
                Id = slide.Id,
                Sequence = slide.Sequence,
                ThumbnailBase64 = slide.WebImageFile,
                ThumbnailMobilBase64 = slide.MobileImageFile,
                Translations = languages.Select(lang => MapTranslation(lang, slide.Translations)).ToList()
            }).ToList();
        }
        public static AppSlideCreateEditViewModel MapToEditViewModel(this AppSlideDto dto, IEnumerable<AppLanguageDto> languages)
        {
            return new AppSlideCreateEditViewModel
            {
                Id = dto.Id,
                Sequence = dto.Sequence,
                ThumbnailBase64 = dto.WebImageFile,
                ThumbnailMobilBase64 = dto.MobileImageFile,
                Translations = languages.Select(lang => MapTranslationList(lang, dto.Translations)).ToList()
            };
        }
        public static AppSlideCreateEditDto MapToDto(this AppSlideCreateEditViewModel viewModel)
        {
            return new AppSlideCreateEditDto
            {
                Id = viewModel.Id,
                Sequence = viewModel.Sequence,
                WebImageFile = viewModel.ThumbnailBase64,
                MobileImageFile = viewModel.ThumbnailMobilBase64,
                Translations = viewModel.Translations.Select(MapTranslationVmToDto).ToList()
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

        // ------------------------
        // Özel Yardımcı Metotlar
        // ------------------------

        private static AppSlideTranslationViewModel MapTranslation(AppLanguageDto lang, IEnumerable<AppSlideTranslationDto> translations)
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
        private static AppSlideTranslationCreateEditViewModel MapTranslationList(AppLanguageDto lang, IEnumerable<AppSlideTranslationDto> translations)
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
        private static AppSlideTranslationCreateEditDto MapTranslationVmToDto(AppSlideTranslationCreateEditViewModel vm)
        {
            return new AppSlideTranslationCreateEditDto
            {
                Id = vm.Id,
                AppLanguageId = vm.AppLanguageId,
                AppSlideId = vm.AppSlideId,
                Title = vm.Title,
                Content = vm.Content,
                ButtonText = vm.ButtonText,
                ButtonUrl = vm.ButtonUrl,
                ButtonIcon = vm.ButtonIcon,
                IsExternal = vm.IsExternal
            };
        }

    }

}
