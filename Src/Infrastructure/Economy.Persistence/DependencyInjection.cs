﻿using Economy.Application.Interfaces;
using Economy.Application.Repositories.AppMenuRepositories;
using Economy.Core.Repositories;
using Economy.Core.UnitOfWorks;
using Economy.Persistence.Contexts;
using Economy.Persistence.Repositories.AppBase.EntityFramework;
using Economy.Persistence.Repositories.AppMenuRepositories;
using Economy.Persistence.Seeds;
using Economy.Persistence.Services;
using Economy.Persistence.UnitOfWorks;
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


            services.AddScoped(typeof(IEntityRepository<,>), typeof(EfEntityRepositoryBase<,>));
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            services.AddScoped<IAppMenuRepository, AppMenuRepository>();
            services.AddScoped<IAppMenuService, AppMenuService>();




            services.AddDbContext<AppDbContext>(options =>
            {
                options.UseSqlServer(connectionString, configure =>
                {
                    configure.MigrationsAssembly("AppWeb");
                });
            });
            services.AddScoped<AppMenuSeeder>();
           


            // Diğer Persistence katmanı servislerini ekleyebilirsin
            return services;
        }
    }
}
