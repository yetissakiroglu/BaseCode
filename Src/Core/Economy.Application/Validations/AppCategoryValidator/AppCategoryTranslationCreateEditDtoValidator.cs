using Economy.Panel.Application.Dtos.AppCategoryDtos.CategoryTranslationDtos;
using FluentValidation;

namespace Economy.Panel.Application.Validations.AppCategoryValidator
{
    public class AppCategoryTranslationCreateEditDtoValidator : AbstractValidator<AppCategoryTranslationCreateEditDto>
    {
        public AppCategoryTranslationCreateEditDtoValidator()
        {
            RuleFor(x => x.AppLanguageId)
                .GreaterThan(0)
                .WithMessage("Dil seçimi zorunludur.");

            RuleFor(x => x.Title)
                .NotEmpty()
                .WithMessage("Başlık alanı zorunludur.")
                .MaximumLength(255)
                .WithMessage("Başlık en fazla 255 karakter olabilir.");

            RuleFor(x => x.Url)
                .NotEmpty()
                .WithMessage("URL alanı zorunludur.")
                .MaximumLength(255)
                .WithMessage("URL en fazla 255 karakter olabilir.");

            RuleFor(x => x.MetaTitle)
                .MaximumLength(255)
                .WithMessage("Meta başlık en fazla 255 karakter olabilir.");

            RuleFor(x => x.MetaDescription)
                .MaximumLength(500)
                .WithMessage("Meta açıklama en fazla 500 karakter olabilir.");
        }
    }
}
