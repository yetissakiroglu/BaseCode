using Economy.Application.Dtos.AppSecuritySettingDtos;
using FluentValidation;

namespace Economy.Application.Validations.AppSecuritySettingValidator
{
    public class AppSecuritySettingCreateDtoValidator : AbstractValidator<AppSecuritySettingCreateDto>
    {

        public AppSecuritySettingCreateDtoValidator()
        {
            RuleFor(x => x.PasswordRequiredLength).InclusiveBetween(4, 128);
            RuleFor(x => x.LockoutTimeSpanMinutes).InclusiveBetween(1, 1440);
            RuleFor(x => x.LockoutMaxFailedAccessAttempts).InclusiveBetween(1, 20);
            // Diğer bool’lara ekstra kural gerekmiyor
        }
    }

}
