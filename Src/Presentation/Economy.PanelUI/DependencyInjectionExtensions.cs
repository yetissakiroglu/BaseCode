using Economy.Core.Interfaces;
using System.Reflection;

namespace Economy.Panel.UI
{
    public static class DependencyInjectionExtensions
    {
        public static void AddRepositories(this IServiceCollection services, Assembly assembly)
        {
            var repositoryTypes = assembly.GetTypes()
                .Where(t => t.GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IEntityRepository<,>)))
                .ToList();

            foreach (var repositoryType in repositoryTypes)
            {
                var interfaceType = repositoryType.GetInterfaces().First(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IEntityRepository<,>));
                services.AddScoped(interfaceType, repositoryType);
            }
        }
    }
}
