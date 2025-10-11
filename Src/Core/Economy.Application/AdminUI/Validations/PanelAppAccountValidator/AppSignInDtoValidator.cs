using Economy.Application.AdminUI.Dtos.AppAccountDtos;
using FluentValidation;

namespace Economy.Application.AdminUI.Validations.PanelAppAccountValidator
{
    public class AppSignInDtoValidator : AbstractValidator<AppSignInDto>
    {
        public AppSignInDtoValidator()
        {

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("E-posta alanı boş olamaz.");
            //.EmailAddress().WithMessage("Geçerli bir e-posta adresi giriniz.");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Şifre alanı boş olamaz.")
                .MinimumLength(6).WithMessage("Şifre en az 6 karakter olmalıdır.");
                //.Matches(@"[A-Z]+").WithMessage("Şifre en az bir büyük harf içermelidir.")
                //.Matches(@"[a-z]+").WithMessage("Şifre en az bir küçük harf içermelidir.")
                //.Matches(@"[0-9]+").WithMessage("Şifre en az bir rakam içermelidir.")
                //.Matches(@"[\@\!\?\*\.]+").WithMessage("Şifre en az bir özel karakter (@!?*.) içermelidir.");
        }
    }
}
