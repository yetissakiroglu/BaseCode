using Economy.Web.Demo1.Models;

namespace Economy.Web.Demo1.Services
{
    public interface IMenuService
    {
        Task<List<MenuNode>> GetTreeAsync(string lang);
    }
}
