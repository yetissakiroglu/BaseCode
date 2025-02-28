using Microsoft.Extensions.DependencyInjection;

namespace Economy.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            // Buraya Application katmanına ait servisleri ekleyebilirsin
            // services.AddScoped<IMyService, MyService>();

            return services;
        }
    }
}
