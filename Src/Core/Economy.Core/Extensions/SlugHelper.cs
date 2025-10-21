using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Unidecode.NET;

namespace Economy.Core.Extensions
{
    public static class SlugHelper
    {
        public static string ToSlug(string input, string mode = "latin", int maxLength = 80)
        {
            if (string.IsNullOrWhiteSpace(input)) return string.Empty;

            string s = input.Trim();

            if (string.Equals(mode, "native", StringComparison.OrdinalIgnoreCase))
            {
                s = Regex.Replace(s, @"\s+", "-");
                s = Regex.Replace(s, @"[^\p{L}\p{Nd}-]", "");
            }
            else
            {
                s = s.ToLowerInvariant();
                s = s.Unidecode(); // Çin/Arap/Kiril → Latin
                s = s.Normalize(NormalizationForm.FormD);
                var sb = new StringBuilder();
                foreach (var ch in s)
                    if (CharUnicodeInfo.GetUnicodeCategory(ch) != UnicodeCategory.NonSpacingMark)
                        sb.Append(ch);
                s = sb.ToString().Normalize(NormalizationForm.FormC);
                s = Regex.Replace(s, @"[^a-z0-9\s-]", "");
                s = Regex.Replace(s, @"\s+", "-");
            }

            s = Regex.Replace(s, @"-+", "-").Trim('-');

            // (İsteğe bağlı) basit stop-word temizliği
            s = RemoveStopWords(s, new[] { "ve", "ile", "veya", "bir", "the", "and", "or", "of", "a", "an" });
            s = Regex.Replace(s, @"-+", "-").Trim('-');

            if (maxLength > 0 && s.Length > maxLength)
                s = s[..maxLength].Trim('-');

            return s;
        }

        private static string RemoveStopWords(string slug, string[] words)
        {
            if (string.IsNullOrEmpty(slug)) return slug;
            foreach (var w in words)
            {
                slug = Regex.Replace(slug,
                    $@"(?<=^|-)({Regex.Escape(w)})-(?=-|$)|(?<=^|-)({Regex.Escape(w)})$",
                    "", RegexOptions.IgnoreCase);
            }
            return Regex.Replace(slug, @"-+", "-").Trim('-');
        }
    }
}
