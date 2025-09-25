namespace Economy.Web.UI.Infrastructure
{
    using Microsoft.AspNetCore.Routing;
    public class ShortCultureConstraint : IRouteConstraint
    {
        private readonly LanguageCache _cache;
        public ShortCultureConstraint(LanguageCache cache) => _cache = cache;

        public bool Match(HttpContext? httpContext, IRouter? route, string routeKey,
                          RouteValueDictionary values, RouteDirection routeDirection)
        {
            // ❗ URL üretiminde asla engelleme — aksi halde Url.Action null döner
            if (routeDirection == RouteDirection.UrlGeneration)
                return true;

            if (values.TryGetValue(routeKey, out var val) && val is string s && !string.IsNullOrWhiteSpace(s))
                return _cache.IsShort(s) || _cache.IsFull(s);

            // segment opsiyonel -> boş ise engelleme
            return true;
        }

        //private readonly LanguageCache _cache;
        //public ShortCultureConstraint(LanguageCache cache) => _cache = cache;
        //public bool Match(HttpContext? httpContext, IRouter? route, string routeKey, RouteValueDictionary values, RouteDirection routeDirection)
        //{
        //    if (values.TryGetValue(routeKey, out var val) && val is string s && !string.IsNullOrWhiteSpace(s))
        //        return _cache.IsShort(s) || _cache.IsFull(s);
        //    return true;
        //}
    }

}
