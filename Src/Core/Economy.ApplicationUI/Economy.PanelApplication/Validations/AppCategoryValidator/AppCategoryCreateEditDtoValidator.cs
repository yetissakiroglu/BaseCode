using Economy.Panel.Application.Dtos.AppCategoryDtos;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Economy.Panel.Application.Validations.AppCategoryValidator
{
    public class AppCategoryCreateEditDtoValidator : AbstractValidator<AppCategoryCreateEditDto>
    {
        public AppCategoryCreateEditDtoValidator()
        {
            RuleFor(x => x.ContentType)
                .IsInEnum()
                .WithMessage("Geçerli bir içerik türü seçiniz.");

            RuleForEach(x => x.Translations)
                .SetValidator(new AppCategoryTranslationCreateEditDtoValidator());

            RuleFor(x => x.Translations)
                .NotEmpty()
                .WithMessage("En az bir dil çevirisi girilmelidir.");
        }
    }
}
