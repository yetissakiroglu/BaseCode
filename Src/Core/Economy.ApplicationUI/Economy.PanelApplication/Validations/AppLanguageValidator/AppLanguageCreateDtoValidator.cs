using Economy.Panel.Application.Dtos.AppLanguageDtos;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Economy.Panel.Application.Validations.AppLanguageValidator
{
    public class AppLanguageCreateDtoValidator : AbstractValidator<AppLanguageCreateDto>
    {
        public AppLanguageCreateDtoValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Dil adı boş olamaz.")
                .MaximumLength(100).WithMessage("Dil adı en fazla 100 karakter olabilir.");

            RuleFor(x => x.Code)
                .NotEmpty().WithMessage("Dil kodu boş olamaz.")
                .MaximumLength(10).WithMessage("Dil kodu en fazla 10 karakter olabilir.");

            RuleFor(x => x.Icon)
                .MaximumLength(250).WithMessage("İkon yolu en fazla 250 karakter olabilir.");
        }
    }
}
