using Economy.Application.Interfaces;
using Economy.Application.Repositories.AppLanguageRepositories;
using Economy.Application.Repositories.AppMenuRepositories;
using Economy.Application.Repositories.AppPageRepositories;
using Economy.Core.Repositories;
using Economy.Core.UnitOfWorks;
using Economy.Persistence.Contexts;
using Economy.Persistence.Repositories.AppBase.EntityFramework;
using Economy.Persistence.Repositories.AppLanguageRepositories;
using Economy.Persistence.Repositories.AppMenuRepositories;
using Economy.Persistence.Repositories.AppPageRepositories;
using Economy.Persistence.Seeds;
using Economy.Persistence.Services;
using Economy.Persistence.UnitOfWorks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Globalization;
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

            services.AddScoped<IAppSettingRepository, AppSettingRepository>();
            services.AddScoped<IAppSettingService, AppSettingService>();

            services.AddScoped<IAppLanguageRepository, AppLanguageRepository>();
            services.AddScoped<IAppLanguageService, AppLanguageService>();

            services.AddScoped<IAppPageRepository, AppPageRepository>();
            services.AddScoped<IAppPageService, AppPageService>();




            services.AddDbContext<AppDbContext>(options =>
            {
                options.UseSqlServer(connectionString, configure =>
                {
                    configure.MigrationsAssembly("AppWeb");
                });
            });



            services.AddScoped<AppMenuSeeder>();
            services.AddScoped<AppLanguageSeeder>();
            services.AddScoped<AppSettingSeeder>();
            services.AddScoped<AppPageSeeder>();

            


            // Diğer servisleri ekleyin
            services.AddScoped<LanguageService>();
            services.Configure<RequestLocalizationOptions>(options =>
            {
                // Dil bilgilerini veritabanından alıyoruz
                var languageService = services.BuildServiceProvider().GetRequiredService<LanguageService>();
                var supportedLanguages = languageService.GetSupportedLanguages();

                // Varsayılan dil (IsDefault = true olanı seçiyoruz)
                var defaultCulture = supportedLanguages.FirstOrDefault(l => l.IsDefault)?.Code ?? "tr";

                var supportedCultures = supportedLanguages
                    .Where(l => l.IsActive)
                    .Select(l => l.Code)
                    .ToArray();

                options.DefaultRequestCulture = new RequestCulture(defaultCulture);
                options.SupportedCultures = supportedCultures.Select(c => new CultureInfo(c)).ToList();
                options.SupportedUICultures = supportedCultures.Select(c => new CultureInfo(c)).ToList();

                // Dil değişikliğini cookie'ye kaydediyoruz
                options.RequestCultureProviders.Insert(0, new CustomRequestCultureProvider(async context =>
                { 
         
                    // Cookie'deki mevcut dil bilgisi
                    var cookieCulture = context.Request.Cookies["UserLanguage"];

                    // Eğer cookie yoksa veya boşsa, default dili kullanıyoruz
                    if (string.IsNullOrEmpty(cookieCulture))
                    {
                        cookieCulture = defaultCulture;
                        context.Response.Cookies.Append("UserLanguage",cookieCulture);
                    }

                    // Kullanıcının mevcut dilini belirliyoruz
                    return await Task.FromResult(new ProviderCultureResult(cookieCulture));
                }));
            });








            // Diğer Persistence katmanı servislerini ekleyebilirsin
            return services;
        }
    }
}
