namespace Economy.Web.UI.Services.Abstractions
{
    public record SliderItem(int Id, string Title, string Subtitle, string ImageUrl, string? LinkUrl);
    public record Room(int Id, string Name, string Slug, string CoverImage, decimal PricePerNight, string ShortDesc);
    public record RoomDetail(int Id, string Name, string Slug, string CoverImage, decimal PricePerNight, string ShortDesc, string HtmlDetail, IEnumerable<string> Gallery);

    public interface IContentService
    {
        Task<List<SliderItem>> GetHomeSliderAsync(string culture);
        Task<List<Room>> GetRoomsAsync(string culture);
        Task<RoomDetail?> GetRoomAsync(string culture, string slug);
    }

}
