using FluentValidation.Results;
using Microsoft.AspNetCore.Identity;
using System.Collections.ObjectModel;

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

        //public static IReadOnlyDictionary<TKey, TValue> ToReadOnlyDictionary<TKey, TValue>(
        //        this IEnumerable<KeyValuePair<TKey, TValue>> source)
        //        where TKey : notnull
        //{
        //    return new ReadOnlyDictionary<TKey, TValue>(source.ToDictionary(kv => kv.Key, kv => kv.Value));
        //}
    }
}
