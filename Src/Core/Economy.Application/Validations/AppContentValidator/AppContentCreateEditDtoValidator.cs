using Economy.Panel.Application.Dtos.AppContentDtos;
using FluentValidation;

namespace Economy.Panel.Application.Validations.AppContentValidator
{
    public class AppContentCreateEditDtoValidator : AbstractValidator<AppContentCreateEditDto>
    {
        public AppContentCreateEditDtoValidator()
        {
            RuleFor(x => x.ContentType)
                .IsInEnum()
                .WithMessage("Geçerli bir içerik türü seçiniz.");

            RuleForEach(x => x.Translations)
                .SetValidator(new AppContentTranslationCreateEditDtoValidator());

            RuleFor(x => x.Translations)
                .NotEmpty()
                .WithMessage("En az bir dil çevirisi girilmelidir.");
        }
    }

   
}
