using Castle.DynamicProxy;
using Economy.Caching.Attributes;
using Microsoft.Extensions.Caching.Memory;

namespace Economy.Caching.Interceptors
{
    public class CacheInterceptor : IInterceptor
    {
        private readonly IMemoryCache _cache;

        public CacheInterceptor(IMemoryCache cache)
        {
            _cache = cache;
        }

        public void Intercept(IInvocation invocation)
        {


            // Asıl implementasyondaki metodu al
            var method = invocation.MethodInvocationTarget;

            if (method == null)
            {
                invocation.Proceed();
                return;
            }

            var attr = method.GetCustomAttributes(typeof(CacheAttribute), true)
                             .FirstOrDefault() as CacheAttribute;

            if (attr == null)
            {
                invocation.Proceed();
                return;
            }

            var args = string.Join("_", invocation.Arguments.Select(a => a?.ToString() ?? "null"));
            var key = $"{method.DeclaringType.FullName}.{method.Name}_{args}";

            if (_cache.TryGetValue(key, out var cached))
            {
                invocation.ReturnValue = cached;
                return;
            }

            invocation.Proceed();

            if (method.ReturnType == typeof(Task))
            {
                invocation.ReturnValue = InterceptAsync((dynamic)invocation.ReturnValue, key, attr.Duration);
            }
            else
            {
                _cache.Set(key, invocation.ReturnValue, TimeSpan.FromMinutes(attr.Duration));
            }
        }

        private async Task<T> InterceptAsync<T>(Task<T> task, string key, int duration)
        {
            var result = await task;
            _cache.Set(key, result, TimeSpan.FromMinutes(duration));
            return result;
        }
    }
}
