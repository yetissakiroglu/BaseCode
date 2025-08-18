using System.Text.RegularExpressions;

namespace Economy.Application.Helpers
{
    public static class GeneralSettingsValidationHelpers
    {
        private static readonly HashSet<string> AllowedThemes =
            new(StringComparer.OrdinalIgnoreCase) { "light", "dark" };

        private static readonly Regex HostRegex = new(
            @"^([a-zA-Z0-9-]+\.)+[a-zA-Z]{2,}$",
            RegexOptions.Compiled);

        public static bool IsAllowedTheme(string theme) => AllowedThemes.Contains(theme);

        public static bool IsValidHost(string? domain)
            => !string.IsNullOrWhiteSpace(domain) && HostRegex.IsMatch(domain.Trim());

        public static bool IsUrlOrPath(string? value)
        {
            if (string.IsNullOrWhiteSpace(value)) return true;
            var v = value.Trim();
            if (Uri.TryCreate(v, UriKind.Absolute, out var abs)
                && (abs.Scheme == Uri.UriSchemeHttp || abs.Scheme == Uri.UriSchemeHttps))
                return true;
            if (v.StartsWith("/")) return true;
            return false;
        }

        public static string[] SplitKeywords(string? keywords)
            => string.IsNullOrWhiteSpace(keywords)
               ? Array.Empty<string>()
               : keywords.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
    }
}
