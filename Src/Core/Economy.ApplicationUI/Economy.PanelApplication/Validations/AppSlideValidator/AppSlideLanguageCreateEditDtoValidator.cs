using Economy.Panel.Application.Dtos.AppSlideDtos.SlideTranslationDtos;
using FluentValidation;

namespace Economy.Panel.Application.Validations.AppSlideValidator
{
    public class AppSlideLanguageCreateEditDtoValidator : AbstractValidator<AppSlideLanguageCreateEditDto>
    {
        public AppSlideLanguageCreateEditDtoValidator()
        {
            RuleFor(x => x.AppLanguageId)
                .NotEmpty().WithMessage("Dil seçimi zorunludur.");

            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Başlık (Title) boş olamaz.")
                .MaximumLength(100).WithMessage("Başlık en fazla 100 karakter olabilir.");

            RuleFor(x => x.ButtonText)
                .MaximumLength(50).WithMessage("Buton metni en fazla 50 karakter olabilir.");

            RuleFor(x => x.ButtonUrl)
                .MaximumLength(250).WithMessage("Buton URL en fazla 250 karakter olabilir.");

            RuleFor(x => x.ButtonIcon)
                .MaximumLength(100).WithMessage("Buton ikon bilgisi en fazla 100 karakter olabilir.");
        }
    }
}
