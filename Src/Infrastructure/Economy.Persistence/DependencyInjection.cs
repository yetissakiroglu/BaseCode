using Economy.Application.Interfaces;
using Economy.Application.Queries.AppLanguages;
using Economy.Application.Repositories.AppContentRepositories;
using Economy.Application.Repositories.AppLanguageRepositories;
using Economy.Application.Repositories.AppMenuRepositories;
using Economy.Application.Repositories.AppPageRepositories;
using Economy.Application.Repositories.AppSettingRepositories;
using Economy.Application.Repositories.AppSlideRepositories;
using Economy.Core.Repositories;
using Economy.Core.UnitOfWorks;
using Economy.Persistence.Contexts;
using Economy.Persistence.Repositories.AppBase.EntityFramework;
using Economy.Persistence.Repositories.AppContentRepositories;
using Economy.Persistence.Repositories.AppLanguageRepositories;
using Economy.Persistence.Repositories.AppMenuRepositories;
using Economy.Persistence.Repositories.AppPageRepositories;
using Economy.Persistence.Repositories.AppSlideRepositories;
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
            services.AddScoped<IAppLogoSettingRepository, AppLogoSettingRepository>();
            services.AddScoped<IAppSettingService, AppSettingService>();

            services.AddScoped<IAppLanguageRepository, AppLanguageRepository>();
            services.AddScoped<IAppLanguageService, AppLanguageService>();

            services.AddScoped<IAppPageRepository, AppPageRepository>();
            services.AddScoped<IAppPageService, AppPageService>();

            services.AddScoped<IAppSlideRepository, AppSlideRepository>();
            services.AddScoped<IAppSlideService, AppSlideService>();

            services.AddScoped<IAppContentRepository, AppContentRepository>();
            services.AddScoped<IAppContentService, AppContentService>();


            

            services.AddDbContext<AppDbContext>(options =>
            {
                options.UseSqlServer(connectionString, configure =>
                {
                    configure.MigrationsAssembly("AppWeb");
                });
            });



            //services.AddScoped<AppMenuSeeder>();
            //services.AddScoped<AppLanguageSeeder>();
            //services.AddScoped<AppSettingSeeder>();
            //services.AddScoped<AppPageSeeder>();
            //services.AddScoped<AppSlideSeeder>();
            //services.AddScoped<AppCategorySeeder>();
            //services.AddScoped<AppContentSeeder>();

            


            // Diğer servisleri ekleyin
            services.Configure<RequestLocalizationOptions>(options =>
            {
                // Dil bilgilerini veritabanından alıyoruz
                var languageService = services.BuildServiceProvider().GetRequiredService<IAppLanguageService>();
                var supportedLanguages = languageService.GetAllForRead( new GetAllAppLanguageQuery(true));

                // Varsayılan dil (IsDefault = true olanı seçiyoruz)
                var defaultCulture = supportedLanguages.Data.FirstOrDefault(l => l.IsDefault)?.Code ?? "tr";

                var supportedCultures = supportedLanguages.Data
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
