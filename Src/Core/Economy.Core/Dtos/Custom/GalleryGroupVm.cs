namespace Economy.Core.Dtos.Custom
{
    public class GalleryGroupVm
    {
        public string Key { get; set; } = "";
        public string Label { get; set; } = "";
        public List<MediaItem> Items { get; set; } = new();
        public string? CoverUrl { get; set; }
    }

    public class MediaItem
    {
        public int Id { get; set; }
        public string MediaUrl { get; set; }
    }

}
