using Economy.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Economy.Persistence
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, string connectionString)
        {
            // AutoMapper'ı ekle
            services.AddAutoMapper(Assembly.GetExecutingAssembly());




            services.AddDbContext<AppDbContext>(options =>
            {
                options.UseSqlServer(connectionString, configure =>
                {
                    configure.MigrationsAssembly("AppWeb");
                });
            });





            // Diğer Persistence katmanı servislerini ekleyebilirsin
            return services;
        }
    }
}
