using Economy.Web.UI.Services.Abstractions;

namespace Economy.Web.UI.Services.Implementations
{
    public class InMemoryLocalizationStore : ILocalizationStore
    {
        private readonly Dictionary<string, Dictionary<string, string>> _map = new(StringComparer.OrdinalIgnoreCase)
        {
            ["tr-TR"] = new(StringComparer.OrdinalIgnoreCase)
            {
                ["Shared.Ana Sayfa"] = "Ana Sayfa",
                ["Shared.Odalarımız"] = "Odalarımız",
                ["Shared.Detay"] = "Detay",
                ["Shared.Gecelik"] = "Gecelik",
                ["Shared.Rezervasyon Talebi"] = "Rezervasyon Talebi",
                ["Shared.Talebi Gönder"] = "Talebi Gönder",
                ["Shared.Talebiniz alındı."] = "Talebiniz alındı.",
                ["Shared.Gönderim başarısız."] = "Gönderim başarısız.",
                ["Views.Home.Title"] = "Ana Sayfa",
                ["Views.Rooms.Title"] = "Odalarımız"
            },
            ["en-US"] = new(StringComparer.OrdinalIgnoreCase)
            {
                ["Shared.Ana Sayfa"] = "Home",
                ["Shared.Odalarımız"] = "Rooms",
                ["Shared.Detay"] = "Details",
                ["Shared.Gecelik"] = "Per Night",
                ["Shared.Rezervasyon Talebi"] = "Reservation Request",
                ["Shared.Talebi Gönder"] = "Send Request",
                ["Shared.Talebiniz alındı."] = "We received your request.",
                ["Shared.Gönderim başarısız."] = "Submission failed.",
                ["Views.Home.Title"] = "Home",
                ["Views.Rooms.Title"] = "Rooms"
            }
        };

        public Task<IDictionary<string, string>> GetNamespaceAsync(string culture, string ns)
        {
            var bag = _map.TryGetValue(culture, out var dict) ? dict : new(StringComparer.OrdinalIgnoreCase);
            var res = bag.Where(kv => kv.Key.StartsWith(ns + ".", StringComparison.OrdinalIgnoreCase))
                         .ToDictionary(kv => kv.Key, kv => kv.Value, StringComparer.OrdinalIgnoreCase);
            return Task.FromResult<IDictionary<string, string>>(res);
        }
    }

}
