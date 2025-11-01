using Economy.Application.TenantUI.Dtos.AppMenuDtos;
using FluentValidation;

namespace Economy.Application.TenantUI.Validations.AppMenuIValidator
{
  

    public class AppMenuItemValidator : AbstractValidator<MenuItemDto>
    {
        public AppMenuItemValidator()
        {
            RuleFor(x => x.Location).NotEmpty().MaximumLength(50);
            RuleFor(x => x.SortOrder).GreaterThanOrEqualTo(0);

            // XOR kuralı: IsExternal true ise PageId olmamalı; IsExternal false ise PageId zorunlu
            When(x => x.IsExternal, () =>
            {
                RuleFor(x => x.PageId).Must(p => p == null)
                    .WithMessage("IsExternal=true iken PageId boş olmalı.");
            });
            When(x => !x.IsExternal, () =>
            {
                RuleFor(x => x.PageId).NotNull().WithMessage("IsExternal=false iken PageId zorunlu.");
            });

            RuleFor(x => x.Translations)
                .NotNull().WithMessage("En az bir çeviri gerekli.")
                .Must(trs => trs.Count > 0).WithMessage("En az bir çeviri girmelisiniz.");

           
        }
    }
}
