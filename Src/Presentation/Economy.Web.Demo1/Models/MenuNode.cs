namespace Economy.Web.Demo1.Models
{

    public record MenuNode(string Title, string Url, bool IsExternal, bool IsActive, List<MenuNode> Children)
    {
        public bool Selected { get; set; }        // tam eşleşen sayfa
        public bool BranchSelected { get; set; }  // kendisi veya çocuklarından biri aktif
    }
}
