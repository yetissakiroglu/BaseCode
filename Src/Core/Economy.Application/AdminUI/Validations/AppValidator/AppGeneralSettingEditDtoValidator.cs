using Economy.Application.AdminUI.Dtos.AppGeneralSettingDtos;
using Economy.Application.AdminUI.Validations.ValidationHelpers;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Economy.Application.AdminUI.Validations.AppValidator
{
    public class AppGeneralSettingEditDtoValidator : AbstractValidator<AppGeneralSettingEditDto>
    {
        public AppGeneralSettingEditDtoValidator()
        {
          
            RuleFor(x => x.Id).GreaterThan(0).WithMessage("Geçersiz kayıt Id.");

            RuleFor(x => x.SiteName)
                .NotEmpty().WithMessage("Site adı zorunludur.")
                .Length(2, 200).WithMessage("Site adı 2-200 karakter olmalıdır.");

            When(x => !string.IsNullOrWhiteSpace(x.Domain), () =>
            {
                RuleFor(x => x.Domain!)
                    .Must(GeneralSettingsValidationHelpers.IsValidHost)
                    .WithMessage("Geçerli bir alan adı giriniz (ör: ornek.com).")
                    .MaximumLength(200);
            });

            RuleFor(x => x.Theme)
                .NotEmpty().WithMessage("Tema zorunludur.")
                .Must(GeneralSettingsValidationHelpers.IsAllowedTheme)
                .WithMessage("Tema 'light' veya 'dark' olmalıdır.");

            When(x => !string.IsNullOrWhiteSpace(x.LogoUrl), () =>
            {
                RuleFor(x => x.LogoUrl!)
                    .Must(GeneralSettingsValidationHelpers.IsUrlOrPath)
                    .WithMessage("Logo URL '/...' ile başlayan yol veya tam http/https URL olmalıdır.")
                    .MaximumLength(300);
            });

            RuleFor(x => x.MetaTitleSuffix).MaximumLength(120);
            RuleFor(x => x.DefaultMetaDescription).MaximumLength(300);
        
        }
    }
}
