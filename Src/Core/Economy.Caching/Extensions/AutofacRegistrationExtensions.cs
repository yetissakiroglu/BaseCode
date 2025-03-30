using Autofac;
using System.Reflection;

namespace Economy.Caching.Extensions
{
    public static class AutofacRegistrationExtensions
    {
        public static void RegisterCachingInterceptors(this ContainerBuilder container, params Assembly[] assemblies)
        {
            //container.RegisterType<CacheInterceptor>();

            //container.RegisterAssemblyTypes(assemblies)
            //         .Where(t => t.GetInterfaces().Any())
            //         .AsImplementedInterfaces()
            //         .EnableInterfaceInterceptors()
            //         .InterceptedBy(typeof(CacheInterceptor));
        }
    }
}
