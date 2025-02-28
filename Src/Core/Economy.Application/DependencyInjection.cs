using Economy.Application.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Economy.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            // MediatR'ı ekleyerek bu assembly'deki tüm handlerları tarar
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(Assembly.GetExecutingAssembly()));




            // Buraya Application katmanına ait servisleri ekleyebilirsin

            return services;
        }
    }
}
