using Economy.Application.AdminUI.Dtos.AppDtos;
using FluentValidation;

namespace Economy.Application.AdminUI.Validations.AppValidator
{
    public class AppCreateEditDtoValidator : AbstractValidator<AppCreateEditDto>
    {
        public AppCreateEditDtoValidator()
        {
            RuleFor(x => x.HotelName)
            .NotEmpty().WithMessage("Otel adı boş olamaz.")
            .MaximumLength(200).WithMessage("Otel adı en fazla 200 karakter olmalıdır.");

            RuleFor(x => x.ServerName)
                .NotEmpty().WithMessage("Sunucu adı boş olamaz.")
                .MaximumLength(200).WithMessage("Sunucu adı en fazla 200 karakter olmalıdır.");

            RuleFor(x => x.DatabaseName)
                .NotEmpty().WithMessage("Veritabanı adı boş olamaz.")
                .MaximumLength(100).WithMessage("Veritabanı adı en fazla 100 karakter olmalıdır.");

            //RuleFor(x => x.UserName)
            //    .NotEmpty().WithMessage("Kullanıcı adı boş olamaz.")
            //    .MaximumLength(100).WithMessage("Kullanıcı adı en fazla 100 karakter olmalıdır.");

            //RuleFor(x => x.IsPassword)
            //    .NotNull().WithMessage("Şifre var mı alanı boş olamaz.");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Şifre boş olamaz.")
                .MaximumLength(100).WithMessage("Şifre en fazla 100 karakter olmalıdır.");

            RuleFor(x => x.Domain)
                .MaximumLength(250).WithMessage("Domain en fazla 250 karakter olmalıdır.")
                .When(x => !string.IsNullOrWhiteSpace(x.Domain)); // boş değilse kontrol et

        }
    }

   
}
