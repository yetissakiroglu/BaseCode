using Economy.Web.UI.Services.Abstractions;

namespace Economy.Web.UI.Services.Implementations
{
    public class InMemoryContentService : IContentService
    {
        private static string Img(string w, string h, string seed) => $"https://picsum.photos/seed/{Uri.EscapeDataString(seed)}/{w}/{h}";
        private readonly Dictionary<string, List<SliderItem>> _sliders = new()
        {
            ["tr-TR"] = new() { new(1, "Karadeniz’in Kalbinde", "Doğayla iç içe", Img("1600", "700", "slider-tr-1"), "/Rooms/Index") },
            ["en-US"] = new() { new(1, "Heart of Black Sea", "Nature & serenity", Img("1600", "700", "slider-en-1"), "/Rooms/Index") },
        };
        private readonly Dictionary<string, List<Room>> _rooms = new()
        {
            ["tr-TR"] = new() {
      new(1,"Standart Oda","standart-oda",Img("900","600","room-std"),2200,"Ekonomik ve konforlu."),
      new(2,"Deluxe Oda","deluxe-oda",Img("900","600","room-dlx"),3400,"Geniş ve konforlu.")
    },
            ["en-US"] = new() {
      new(1,"Standard Room","standard-room",Img("900","600","room-std"),220,"Comfortable & budget."),
      new(2,"Deluxe Room","deluxe-room",Img("900","600","room-dlx"),340,"Spacious & comfy.")
    },
        };

        public Task<List<SliderItem>> GetHomeSliderAsync(string culture) => Task.FromResult(_sliders[culture]);
        public Task<List<Room>> GetRoomsAsync(string culture) => Task.FromResult(_rooms[culture]);

        public Task<RoomDetail?> GetRoomAsync(string culture, string slug)
        {
            var r = _rooms[culture].FirstOrDefault(x => x.Slug == slug);
            if (r is null) return Task.FromResult<RoomDetail?>(null);
            var html = culture == "tr-TR" ? $"<p><strong>{r.Name}</strong> modern konforlar...</p>" : $"<p><strong>{r.Name}</strong> modern comforts...</p>";
            var gal = Enumerable.Range(1, 6).Select(i => Img("800", "500", $"{r.Slug}-{i}"));
            return Task.FromResult<RoomDetail?>(new(r.Id, r.Name, r.Slug, r.CoverImage, r.PricePerNight, r.ShortDesc, html, gal));
        }
    }

}
