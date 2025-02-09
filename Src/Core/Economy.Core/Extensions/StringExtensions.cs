using Microsoft.AspNetCore.Http;
using System.Text.RegularExpressions;

namespace Economy.Core.Extensions
{
    public static class StringExtensions
    {
        public static string ToUrlFriendly(this string text)
        {
            return text
                .ToLowerInvariant()
                .Replace(" ", "-")
                .Replace("ç", "c")
                .Replace("ş", "s")
                .Replace("ğ", "g")
                .Replace("ü", "u")
                .Replace("ö", "o")
                .Replace("ı", "i");
        }
        public static string GetTextBetween(this string str, string start = "", string end = "")
        {
            try
            {
                var startingIndex = !string.IsNullOrEmpty(start) ? str.IndexOf(start) + start.Length : 0;

                var endingIndex = (!string.IsNullOrEmpty(end) ? str.IndexOf(end) : str.Length);

                var length = endingIndex - startingIndex;

                return str.Substring(startingIndex, length).Trim();
            }
            catch
            {
                return string.Empty;
            }
        }
        public static string ToInitial(this string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return string.Empty;
            }
            string[] words = input.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            if (words.Length == 0)
            {
                return string.Empty;
            }
            else if (words.Length == 1)
            {
                return words[0].Substring(0, 1);
            }
            else
            {
                return $"{words[0][0]}{words[1][0]}";
            }
        }
        public static string ToSlug(this string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return string.Empty;
            }
            return input.ToLower().Replace(' ', '-');
        }
        public static string GenerateSlug(this string phrase)
        {
            if (string.IsNullOrWhiteSpace(phrase))
                return string.Empty;

            // 1. Tüm harfleri küçük harfe dönüştür
            string slug = phrase.ToLowerInvariant();

            // 2. Türkçe karakterleri İngilizce karşılıklarına dönüştür
            slug = slug.Replace("ç", "c")
                       .Replace("ğ", "g")
                       .Replace("ı", "i")
                       .Replace("ö", "o")
                       .Replace("ş", "s")
                       .Replace("ü", "u");

            // 3. Özel karakterleri temizle
            slug = Regex.Replace(slug, @"[^a-z0-9\s-]", "");

            // 4. Boşlukları tire ile değiştir
            slug = Regex.Replace(slug, @"\s+", "-").Trim('-');

            // 5. Tekrar eden tireleri kaldır
            slug = Regex.Replace(slug, @"-+", "-");

            return slug;
        }
        public static string GenerateCanonicalUrl(this string baseUrl, string category, string slug)
        {
            return $"{baseUrl}/{category}/{slug}";
        }
        public static string ToSiteURL(this string pageURL, HttpContext httpContext, bool forceHTTPS = false)
        {
            var request = httpContext.Request;
            var scheme = forceHTTPS ? "https" : request.Scheme;
            return $"{scheme}://{request.Host}/";
        }
        public static string SanitizeString(this string str)
        {
            string sanitizedString = string.Empty;
            if (!string.IsNullOrEmpty(str))
            {
                sanitizedString = Regex.Replace(str.Trim(), "[^a-zA-Z0-9-]", "-", RegexOptions.Compiled);
                sanitizedString = Regex.Replace(sanitizedString, "-{2,}", "-", RegexOptions.Compiled);
            }

            return sanitizedString;
        }
        public static string ToDateTimeString(this DateTime dateTime)
        {
            return dateTime.ToString("d MMMM yyyy HH:mm");
        }
    }
}