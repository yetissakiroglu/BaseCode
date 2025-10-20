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
        public int? Id { get; set; }
        public string? MediaUrl { get; set; }
        public int SortOrder { get; set; }
        public List<MediaItemTranslation> Translations { get; set; } = new();
    }

    public class MediaItemTranslation
    {
        public int? Id { get; set; }
        public int AppLanguageId { get; set; }
        public string AppLanguageCode { get; set; } = default!;
        public string? AppLanguageIcon { get; set; } = default!;
        public string? Alt { get; set; }
        public string? Caption { get; set; }
    }
}
