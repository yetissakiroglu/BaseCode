using Economy.Application.TenantUI.Dtos;
using Economy.Core.Enums;
using FluentValidation;

namespace Economy.Application.TenantUI.Validations
{
    public class BlockItemDtoValidator : AbstractValidator<BlockItemDto>
    {
        public BlockItemDtoValidator()
        {
            RuleFor(x => x.Title).NotEmpty().MaximumLength(200);
            RuleFor(x => x.CoverImage).NotEmpty().MaximumLength(500);


            When(x => x.LinkType == LinkType.InternalPage, () =>
            {
                RuleFor(x => x.LinkedPageId).NotNull();
            });


            When(x => x.LinkType == LinkType.ExternalUrl, () =>
            {
                RuleFor(x => x.ExternalUrl).NotEmpty().Matches("^https?://");
            });
        }
    }
}
