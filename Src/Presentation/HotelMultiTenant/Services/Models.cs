namespace HotelMultiTenant.Services
{

    public record HomeVm(string Title, string Subtitle, string HeroImageUrl, IEnumerable<string> Highlights);
    public record AboutVm(string Title, string BodyHtml, string CoverImageUrl);
    public record RoomItemVm(string Name, string Slug, string ThumbUrl, string ShortDesc, decimal? PricePerNight);
    public record RoomsVm(string Title, IEnumerable<RoomItemVm> Rooms);
    public record ServicesVm(string Title, IEnumerable<(string Name, string Desc)> Items);
    public record ContactVm(string Title, string Address, string Phone, string Email, string MapEmbedHtml);
}
