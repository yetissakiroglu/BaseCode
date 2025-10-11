using Economy.Application.AdminUI.Dtos.AppSuperAdminUserDtos;
using FluentValidation;

namespace Economy.Application.AdminUI.Validations.AppSuperAdminValidator
{
    public class SuperAdminEditDtoValidator : AbstractValidator<AppSuperAdminUserEditDto>
    {
        public SuperAdminEditDtoValidator()
        {
            RuleFor(x => x.FirstName)
                .NotEmpty().WithMessage("Adı alanı boş olamaz.")
                .MaximumLength(50).WithMessage("Adı en fazla 50 karakter olabilir.");

            RuleFor(x => x.LastName)
                .NotEmpty().WithMessage("Soyadı alanı boş olamaz.")
                .MaximumLength(50).WithMessage("Soyadı en fazla 50 karakter olabilir.");

            RuleFor(x => x.UserName)
                .NotEmpty().WithMessage("Kullanıcı Adı alanı boş olamaz.")
                .MinimumLength(3).WithMessage("Kullanıcı Adı en az 3 karakter olmalıdır.")
                .MaximumLength(30).WithMessage("Kullanıcı Adı en fazla 30 karakter olabilir.");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("E-posta alanı boş olamaz.")
                .EmailAddress().WithMessage("Geçerli bir e-posta adresi giriniz.");

            RuleFor(x => x.PhoneNumber)
                .MaximumLength(20).WithMessage("Telefon numarası en fazla 20 karakter olabilir.")
                .Matches(@"^\+?\d{10,20}$").When(x => !string.IsNullOrWhiteSpace(x.PhoneNumber))
                .WithMessage("Geçerli bir telefon numarası giriniz.");
        }
    }

  
}
