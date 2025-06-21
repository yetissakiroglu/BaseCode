using Economy.Base.Application.Dtos.BaseModels;

namespace Economy.Application.Validations.AppUserValidator
{
    using FluentValidation;

    public class AppUserCreateDtoValidator : AbstractValidator<AppUserCreateDto>
    {
        public AppUserCreateDtoValidator()
        {
            RuleFor(x => x.FirstName)
                .NotEmpty().WithMessage("Adı alanı boş olamaz.")
                .MaximumLength(50).WithMessage("Adı en fazla 50 karakter olabilir.");

            RuleFor(x => x.LastName)
                .NotEmpty().WithMessage("Soyadı alanı boş olamaz.")
                .MaximumLength(50).WithMessage("Soyadı en fazla 50 karakter olabilir.");

            RuleFor(x => x.TenantId)
                .NotEmpty().WithMessage("Bağlı Uygulama seçilmelidir.")
                .GreaterThan(0).WithMessage("Geçerli bir uygulama seçilmelidir.");

            RuleFor(x => x.UserName)
                .NotEmpty().WithMessage("Kullanıcı Adı alanı boş olamaz.")
                .MinimumLength(3).WithMessage("Kullanıcı Adı en az 3 karakter olmalıdır.")
                .MaximumLength(30).WithMessage("Kullanıcı Adı en fazla 30 karakter olabilir.");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("E-posta alanı boş olamaz.")
                .EmailAddress().WithMessage("Geçerli bir e-posta adresi giriniz.");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Şifre alanı boş olamaz.")
                .MinimumLength(6).WithMessage("Şifre en az 6 karakter olmalıdır.")
                .Matches(@"[A-Z]+").WithMessage("Şifre en az bir büyük harf içermelidir.")
                .Matches(@"[a-z]+").WithMessage("Şifre en az bir küçük harf içermelidir.")
                .Matches(@"[0-9]+").WithMessage("Şifre en az bir rakam içermelidir.")
                .Matches(@"[\@\!\?\*\.]+").WithMessage("Şifre en az bir özel karakter (@!?*.) içermelidir.");

            RuleFor(x => x.PhoneNumber)
                .MaximumLength(20).WithMessage("Telefon numarası en fazla 20 karakter olabilir.")
                .Matches(@"^\+?\d{10,20}$").When(x => !string.IsNullOrWhiteSpace(x.PhoneNumber))
                .WithMessage("Geçerli bir telefon numarası giriniz.");
        }
    }


}
