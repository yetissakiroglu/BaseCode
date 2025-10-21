using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Economy.Application.Interfaces
{
    public interface ISlugService
    {

        Task<string> GenerateForPageTranslationAsync(
            string title,
            int languageId,
            int? pageTranslationId = null, // edit’te çeviri id’si
            CancellationToken ct = default);
    }

}
