using Economy.Application.TenantUI.Dtos.AppMenuDtos;

namespace Economy.Application.ApplicationUI.Dtos
{
    public record MenuNodeDto(string Title, string Url, bool IsExternal, bool IsActive, List<MenuNodeDto> Children)
    {
        public bool Selected { get; set; }        // tam eşleşen sayfa
        public bool BranchSelected { get; set; }  // kendisi veya çocuklarından biri aktif
    }
}
