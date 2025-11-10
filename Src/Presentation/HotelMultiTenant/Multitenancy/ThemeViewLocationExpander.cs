using Microsoft.AspNetCore.Mvc.Razor;

namespace HotelMultiTenant.Multitenancy
{

    public class ThemeViewLocationExpander : IViewLocationExpander
    {
        public void PopulateValues(ViewLocationExpanderContext context)
        {
            var tenant = context.ActionContext.HttpContext.GetTenant()?.Current;
            context.Values["theme"] = tenant?.ThemeKey ?? "Classic";
        }

        public IEnumerable<string> ExpandViewLocations(ViewLocationExpanderContext context, IEnumerable<string> viewLocations)
        {
            var theme = context.Values.TryGetValue("theme", out var v) ? v : "Classic";
            var themed = new[]
            {
            $"/Themes/{theme}/Views/{{1}}/{{0}}.cshtml",
            $"/Themes/{theme}/Views/Shared/{{0}}.cshtml"
        };

            return themed.Concat(viewLocations);
        }
    }
}
