using Economy.Panel.Application.Dtos.AppLanguageDtos;
using FluentValidation;

namespace Economy.Panel.Application.Validations.AppLanguageValidator
{
    public class AppLanguageEditDtoValidator : AbstractValidator<AppLanguageEditDto>
    {
        public AppLanguageEditDtoValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("Geçersiz dil Id'si.");

            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Dil adı boş olamaz.")
                .MaximumLength(100).WithMessage("Dil adı en fazla 100 karakter olabilir.");

            RuleFor(x => x.Code)
                .NotEmpty().WithMessage("Dil kodu boş olamaz.")
                .Length(2).WithMessage("Dil kodu 2 karakter olmalıdır.");

            RuleFor(x => x.Icon)
                .MaximumLength(200).WithMessage("Icon alanı en fazla 200 karakter olabilir.");

            RuleFor(x => x.IsDefault)
                .NotNull().WithMessage("Varsayılan dil bilgisi belirtilmelidir.");

            RuleFor(x => x.IsActive)
                .NotNull().WithMessage("Aktiflik durumu belirtilmelidir.");

            RuleFor(x => x.IsRTL)
                .NotNull().WithMessage("RTL (sağdan sola) durumu belirtilmelidir.");
        }
    }
}
