using Economy.Web.UI.Models;

namespace Economy.Web.UI.Services.Abstractions
{
    public interface IMenuService
    {
        Task<List<MenuItem>> GetMainAsync();     // Header menü
        Task<List<MenuItem>> GetFooterAsync();   // Footer menü
        string TitleFor(MenuItem item, string culture);
    }
}
