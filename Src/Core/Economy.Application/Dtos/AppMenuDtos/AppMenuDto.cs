namespace Economy.Application.Dtos.AppMenuDtos
{
    public class AppMenuDto
    {
        public int Id { get; set; }
        public required string Title { get; set; }
        public required string Slug { get; set; }
        public bool IsExternal { get; set; }
        public int? ParentMenuId { get; set; }
        public List<AppMenuDto> SubMenus { get; set; } = new List<AppMenuDto>();
    }
}
