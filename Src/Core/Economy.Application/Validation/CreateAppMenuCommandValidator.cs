using Economy.Application.Commands.AppMenus;
using FluentValidation;

namespace Economy.Application.Validation
{
    public class CreateAppMenuCommandValidator : AbstractValidator<CreateAppMenuCommand>
    {
        public CreateAppMenuCommandValidator()
        {
            RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Title is required.")
            .MaximumLength(255).WithMessage("Title must not exceed 255 characters.");

            RuleFor(x => x.Url)
                .NotEmpty().WithMessage("Url is required.")
                .MaximumLength(255).WithMessage("Url must not exceed 255 characters.");

        }
    }
}
