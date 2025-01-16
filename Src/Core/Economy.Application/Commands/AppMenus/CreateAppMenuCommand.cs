namespace Economy.Application.Commands.AppMenus
{
    public class CreateAppMenuCommand
    {
        public string Title { get; set; }
        public string Slug { get; set; }
        public bool IsExternal { get; set; }
        public int? ParentMenuId { get; set; }
    }
}
