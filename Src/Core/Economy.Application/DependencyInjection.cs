using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Economy.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            // AutoMapper'ı ekle
            services.AddAutoMapper(typeof(AssemblyService));

            // Economy.Application içindeki tüm IRequestHandler'ları otomatik ekler
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(
                Assembly.GetAssembly(typeof(AssemblyService))!
            ));


            // Buraya Application katmanına ait servisleri ekleyebilirsin

            return services;
        }
    }
}
