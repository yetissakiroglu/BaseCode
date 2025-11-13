using Economy.UI.Dtos;

namespace HotelMultiTenant.Models
{
    public class HeaderComponentViewModel
    {
        public string Title { get; set; }
        public string Subtitle { get; set; }
        public string BackgroundImageUrl { get; set; }
        public List<MenuNodeDto> Menus { get; set; } = new List<MenuNodeDto>();
    }
}
