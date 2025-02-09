namespace Economy.Domain.Extensions
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
    }
}
