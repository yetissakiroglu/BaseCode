using Economy.Web.Demo1.Models;

namespace Economy.Web.Demo1.Helpers
{
    public interface ISeoHelper
    {
        Task<SeoViewModel> BuildAsync(
            SeoSeed seed, string controller, string action,
            object? routeValuesBase = null, string? currentLang = null,
            string? xDefaultUrl = null
        );
    }
}
