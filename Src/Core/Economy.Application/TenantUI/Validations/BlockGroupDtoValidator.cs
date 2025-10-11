using Economy.Application.TenantUI.Dtos;
using FluentValidation;

namespace Economy.Application.TenantUI.Validations
{
    public class BlockGroupDtoValidator : AbstractValidator<BlockGroupDto>
    {
        public BlockGroupDtoValidator()
        {
            RuleFor(x => x.Code).NotEmpty().MaximumLength(150).Matches("^[a-z0-9-]+$");
            RuleFor(x => x.Title).NotEmpty().MaximumLength(200);
        }
    }
}
