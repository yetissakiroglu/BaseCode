using Economy.Application.TenantUI.Dtos.AppLanguageDtos;
using FluentValidation;

namespace Economy.Application.TenantUI.Validations.AppLanguageValidator
{
    public class AppLanguageCreateEditDtoValidator : AbstractValidator<AppLanguageCreateEditDto>
    {
        public AppLanguageCreateEditDtoValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Dil adı boş olamaz.")
                .MaximumLength(100).WithMessage("Dil adı en fazla 100 karakter olabilir.");

            RuleFor(x => x.Code)
                .NotEmpty().WithMessage("Dil kodu boş olamaz.")
                .MaximumLength(10).WithMessage("Dil kodu en fazla 10 karakter olabilir.");

            RuleFor(x => x.Icon)
                .NotEmpty().WithMessage("Dil ikon boş olamaz.")
                .MaximumLength(250).WithMessage("İkon yolu en fazla 250 karakter olabilir.");

            RuleFor(x => x.IsDefault)
                .NotNull().WithMessage("Varsayılan dil bilgisi belirtilmelidir.");

            RuleFor(x => x.IsActive)
                .NotNull().WithMessage("Aktiflik durumu belirtilmelidir.");

            RuleFor(x => x.IsRTL)
                .NotNull().WithMessage("RTL (sağdan sola) durumu belirtilmelidir.");

        }
    }
}
