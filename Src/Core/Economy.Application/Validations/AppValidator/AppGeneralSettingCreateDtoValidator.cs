using Economy.Application.Dtos.AppGeneralSettingDtos;
using Economy.Application.Helpers;
using FluentValidation;

namespace Economy.Application.Validations.AppValidator
{
    public class AppGeneralSettingCreateDtoValidator : AbstractValidator<AppGeneralSettingCreateDto>
    {
        public AppGeneralSettingCreateDtoValidator()
        {
            RuleFor(x => x.SiteName)
          .NotEmpty().WithMessage("Site adı zorunludur.")
          .Length(2, 200).WithMessage("Site adı 2-200 karakter olmalıdır.");

            RuleFor(x => x.Domain)
                .Cascade(CascadeMode.Stop)
                .MaximumLength(200)
                .Must(s => string.IsNullOrWhiteSpace(s) || GeneralSettingsValidationHelpers.IsValidHost(s))
                    .WithMessage("Geçerli bir alan adı giriniz (ör: ornek.com).");

            RuleFor(x => x.Theme)
                .NotEmpty().WithMessage("Tema zorunludur.")
                .Must(GeneralSettingsValidationHelpers.IsAllowedTheme)
                    .WithMessage("Tema 'light' veya 'dark' olmalıdır.");

            RuleFor(x => x.LogoUrl)
                .Cascade(CascadeMode.Stop)
                .MaximumLength(300)
                .Must(s => string.IsNullOrWhiteSpace(s) || GeneralSettingsValidationHelpers.IsUrlOrPath(s))
                    .WithMessage("Logo URL '/...' ile başlayan yol veya tam http/https URL olmalıdır.");

            RuleFor(x => x.MetaTitleSuffix)
                .MaximumLength(120);

            RuleFor(x => x.DefaultMetaDescription)
                .MaximumLength(300);



        }
    }
}
