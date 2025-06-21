using FluentValidation.Results;
using Microsoft.AspNetCore.Identity;

namespace Economy.Panel.Application.Extensions
{
    public static class ValidationResultExtensions
    {
        public static IReadOnlyDictionary<string, string[]> ToValidationDictionary(
        this IEnumerable<IdentityError> identityErrors)
        {
            return identityErrors
                .GroupBy(e => e.Code)
                .ToDictionary(
                    g => g.Key,
                    g => g.Select(e => e.Description).ToArray());
        }
        public static IReadOnlyDictionary<string, string[]> ToValidationDictionary(this ValidationResult validationResult)
        {
            return validationResult.Errors
                .GroupBy(e => e.PropertyName)
                .ToDictionary(
                    g => g.Key,
                    g => g.Select(e => e.ErrorMessage).ToArray()
                );
        }
    }
}
