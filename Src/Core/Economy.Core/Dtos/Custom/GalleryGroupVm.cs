namespace Economy.Core.Dtos.Custom
{
    public class GalleryGroupVm
    {
        public string Key { get; set; } = "";
        public string Label { get; set; } = "";
        public List<string> Items { get; set; } = new();
        public string? CoverUrl { get; set; }
    }
}
