using Economy.Application.Commands.AppMenus;
using FluentValidation;

namespace Economy.Application.Validation
{
    public class UpdateAppMenuCommandValidator : AbstractValidator<UpdateAppMenuCommand>
    {
        public UpdateAppMenuCommandValidator()
        {
            // Command alanı zorunlu
            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("The 'Title' field is required in the request body.");

            // Slug alanı zorunlu
            RuleFor(x => x.Slug)
                .NotEmpty().WithMessage("The 'Slug' field is required in the request body.");

            // parentMenuId, geçerli bir integer olmalı
            RuleFor(x => x.ParentMenuId)
                .Must(BeValidInteger).WithMessage("Invalid value for 'parentMenuId'. Please ensure it is a valid integer.");
        }

        private bool BeValidInteger(int? value)
        {
            return value.HasValue && value.Value >= 0; // Geçerli bir tamsayı kontrolü
        }
    }
}
