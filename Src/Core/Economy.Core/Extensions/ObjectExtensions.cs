using System.Text.Json;

namespace Economy.Core.Extensions
{
    public static class ObjectExtensions
    {
        private static readonly JsonSerializerOptions _jsonOpts = new()
        {
            PropertyNameCaseInsensitive = true,
            ReadCommentHandling = JsonCommentHandling.Skip,
            AllowTrailingCommas = true
        };

        public static T? SafeAs<T>(this object? obj)
        {
            try
            {
                if (obj is null)
                    return default;

                // Zaten doğru tipteyse direkt döndür
                if (obj is T ok)
                    return ok;

                // String JSON ise deserialize et
                if (obj is string s && !string.IsNullOrWhiteSpace(s))
                    return JsonSerializer.Deserialize<T>(s, _jsonOpts);

                // JsonElement ise deserialize et
                if (obj is JsonElement elem)
                    return elem.Deserialize<T>(_jsonOpts);

                // JsonDocument ise root’tan deserialize et
                if (obj is JsonDocument doc)
                    return doc.RootElement.Deserialize<T>(_jsonOpts);

                // Diğer tüm tipler için ToString ile şansımızı deneyelim
                var str = obj.ToString();
                if (!string.IsNullOrWhiteSpace(str))
                    return JsonSerializer.Deserialize<T>(str, _jsonOpts);

                return default;
            }
            catch
            {
                return default;
            }
        }

        // İstersen ama tek başına da çalışır
        public static T? SafeDeserialize<T>(this string? json)
        {
            if (string.IsNullOrWhiteSpace(json))
                return default;

            try
            {
                return JsonSerializer.Deserialize<T>(json, _jsonOpts);
            }
            catch
            {
                return default;
            }
        }
    }
}
