using Economy.Application.TenantUI.Dtos.AppSlideDtos;
using FluentValidation;

namespace Economy.Application.TenantUI.Validations.AppSlideValidator
{
    public class AppSlideCreateEditDtoValidator : AbstractValidator<AppSlideCreateEditDto>
    {
        public AppSlideCreateEditDtoValidator()
        {
            RuleFor(x => x.Sequence)
                .NotEmpty().WithMessage("Sıra numarası boş olamaz.");

            RuleFor(x => x.Translations)
                .NotNull().WithMessage("Çeviri listesi boş olamaz.")
                .Must(x => x.Any()).WithMessage("En az bir çeviri girilmelidir.");

            RuleForEach(x => x.Translations).SetValidator(new AppSlideLanguageCreateEditDtoValidator());
        }
    }


}
