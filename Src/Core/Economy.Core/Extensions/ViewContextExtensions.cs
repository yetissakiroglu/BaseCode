using Microsoft.AspNetCore.Mvc.Rendering;

namespace Economy.Core.Extensions
{
    public static class ViewContextExtensions
    {
        public static string GetNavClass(this ViewContext viewContext, string page)
        {
            var activePage = viewContext.ViewData["ActivePage"] as string
                ?? System.IO.Path.GetFileNameWithoutExtension(viewContext.ActionDescriptor.DisplayName);
            return string.Equals(activePage, page, StringComparison.OrdinalIgnoreCase) ? "active" : null;
        }
    }
}
