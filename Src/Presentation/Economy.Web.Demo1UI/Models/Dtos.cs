namespace MyHotelSite.Models;


public class HomeViewModel
{
    public List<string> SliderImages { get; set; } = new();
    public string? SliderVideoUrl { get; set; }
    public List<Room> Rooms { get; set; } = new();
    public List<Campaign> Campaigns { get; set; } = new();
    public List<GalleryItem> Gallery { get; set; } = new();
}
