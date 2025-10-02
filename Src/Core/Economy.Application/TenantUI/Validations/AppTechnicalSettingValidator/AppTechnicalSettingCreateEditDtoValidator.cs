using Economy.Application.TenantUI.Dtos.AppTechnicalSettingDtos;
using FluentValidation;

namespace Economy.Application.TenantUI.Validations.AppTechnicalSettingValidator
{
 
    public sealed class AppTechnicalSettingCreateEditDtoValidator : AbstractValidator<AppTechnicalSettingCreateEditDto>
    {
        public AppTechnicalSettingCreateEditDtoValidator()
        {
            RuleFor(x => x.DomainName)
              .NotEmpty().WithMessage("Domain adı boş olamaz.")
              .MaximumLength(200);

            //RuleFor(x => x.StaticFileUrl)
            //    .MaximumLength(500)
            //    .Matches(@"^https?://.*").When(x => !string.IsNullOrWhiteSpace(x.StaticFileUrl))
            //    .WithMessage("StaticFileUrl http/https ile başlamalıdır.");

            //RuleFor(x => x.GoogleAnalyticsCode).MaximumLength(1000);
            //RuleFor(x => x.FacebookPixelCode).MaximumLength(2000);

            //// Basit XSS guard (güvenlik için ayrıca server-side sanitize edeceğiz)
            //RuleFor(x => x.CustomCss)
            //    .Must(NoScriptTag).WithMessage("CustomCss içinde script etiketi kullanmayın.");
            //RuleFor(x => x.CustomJs)
            //    .Must(NoScriptTag).WithMessage("CustomJs içinde <script> etiketi kullanmayın.");

            //// Placement'lara uygun içerik
            //RuleFor(x => x.CustomHeaderScripts)
            //    .Must((dto, val) => string.IsNullOrWhiteSpace(val) ||
            //                        dto.AllowedCustomScriptPlacements.HasFlag(CustomScriptPlacement.Header))
            //    .WithMessage("Header scriptleri için Header izni (AllowedCustomScriptPlacements) gereklidir.");

            //RuleFor(x => x.CustomFooterScripts)
            //    .Must((dto, val) => string.IsNullOrWhiteSpace(val) ||
            //                        dto.AllowedCustomScriptPlacements.HasFlag(CustomScriptPlacement.Footer))
            //    .WithMessage("Footer scriptleri için Footer izni (AllowedCustomScriptPlacements) gereklidir.");
        }

        private bool NoScriptTag(string? value)
            => string.IsNullOrWhiteSpace(value) || !value.Contains("<script", StringComparison.OrdinalIgnoreCase);

    }
}


