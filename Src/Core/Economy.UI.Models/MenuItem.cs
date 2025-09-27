namespace Economy.UI.Models
{
    public class MenuItem
    {
        public int Id { get; set; }
        public string Lang { get; set; } = "tr";
        public int? ParentId { get; set; }
        public string Title { get; set; } = "";
        public bool IsExternal { get; set; } = false;
        public string? Url { get; set; }
        public int? PageId { get; set; }
        public int Order { get; set; }
        public bool IsActive { get; set; } = true;
    }

}
