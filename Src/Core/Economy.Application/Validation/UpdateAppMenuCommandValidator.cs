using Economy.Application.Commands.AppMenus;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Economy.Application.Validation
{
    public class UpdateAppMenuCommandValidator : AbstractValidator<UpdateAppMenuCommand>
    {
        public UpdateAppMenuCommandValidator()
        {
            RuleFor(x => x.Id).GreaterThan(0).WithMessage("Id must be greater than 0.");
            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Title is required.")
                .MaximumLength(100).WithMessage("Title cannot exceed 100 characters.");
            RuleFor(x => x.Slug)
                .NotEmpty().WithMessage("Slug is required.")
                .MaximumLength(100).WithMessage("Slug cannot exceed 100 characters.");
            RuleFor(x => x.ParentMenuId)
                .GreaterThanOrEqualTo(0).When(x => x.ParentMenuId.HasValue)
                .WithMessage("ParentMenuId must be greater than or equal to 0.");
        }
    }
}
