using AppWeb.ActionFilters;
using AppWeb.Providers;
using Autofac.Extensions.DependencyInjection;
using Autofac;
using Economy.Application;
using Economy.Persistence;
using Economy.Persistence.Seeds;
using LoggingLibrary;
using LoggingLibrary.Data;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using LoggingLibrary.Extensions;
using Autofac.Extras.DynamicProxy;
using LoggingLibrary.Interceptors;
using Economy.Application.Interfaces;
using Economy.Persistence.Services;

var builder = WebApplication.CreateBuilder(args);
// AppSettingsActionFilter'ı global olarak kaydedin
builder.Services.AddScoped<AppSettingsActionFilter>();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);  // Session süresi
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

// MVC servislerini ekliyoruz
builder.Services.AddControllersWithViews(options =>
{
    // Action Filter'ı global olarak uygulamak
    options.Filters.AddService<AppSettingsActionFilter>();
})
    .AddDataAnnotationsLocalization()
    .AddViewLocalization();

// HttpContextAccessor'ı DI container'a ekliyoruz
builder.Services.AddHttpContextAccessor();

// LanguageProvider'ı DI container'a ekliyoruz ve kullanıcı dil sağlayıcıyı kullanıyoruz
builder.Services.AddScoped<LanguageProvider, UserLanguageProvider>();


// 📌 Autofac Kullanımı
builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());

builder.Host.ConfigureContainer<ContainerBuilder>(container =>
{
    container.RegisterType<LoggingInterceptor>()
             .AsSelf()
             .InstancePerLifetimeScope(); // 📌 **Scoped olarak kaydedildi**

    container.RegisterType<HttpContextAccessor>()
             .As<IHttpContextAccessor>()
             .SingleInstance();

    // 📌 **Servislerin olduğu tüm Assembly'leri al**
    var assemblies = new[]
    {
        Assembly.GetExecutingAssembly(), // **Ana proje**
        Assembly.Load("Economy.Persistence") // **Eklenen Class Library**
    };

    // 📌 **Tüm servisleri otomatik kaydet (IService şeklindeki interface'lere karşılık gelenleri)**
    container.RegisterAssemblyTypes(assemblies)
             .Where(t => t.Name.EndsWith("Service"))
             .AsImplementedInterfaces()
             .EnableInterfaceInterceptors() // 📌 Interceptor'u etkinleştir
             .InterceptedBy(typeof(LoggingInterceptor))
             .InstancePerLifetimeScope();

    // 📌 LoggingDbContext'i Autofac Container'ına ekle
    container.AddLoggingDbContextWithAutofac(builder.Configuration);
});
// 📌 LoggingLibrary’i API’ye Entegre Et


// Servisleri ilgili extension metodlar ile ekliyoruz
builder.Services.AddApplicationServices();
var connectionString = builder.Configuration.GetConnectionString("SqlServer") ?? throw new InvalidOperationException("Connection string 'SqlServer' not found.");
builder.Services.AddInfrastructureServices(connectionString);


var app = builder.Build();


// Seed the database
var seederAppMenu = app.Services.CreateScope().ServiceProvider.GetRequiredService<AppMenuSeeder>();
await seederAppMenu.SeedAsync();
var seederAppLanguage = app.Services.CreateScope().ServiceProvider.GetRequiredService<AppLanguageSeeder>();
await seederAppLanguage.SeedAsync();
var seederAppSetting = app.Services.CreateScope().ServiceProvider.GetRequiredService<AppSettingSeeder>();
await seederAppSetting.SeedAsync();
var seederAppPage = app.Services.CreateScope().ServiceProvider.GetRequiredService<AppPageSeeder>();
await seederAppPage.SeedAsync();



// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

// Session kullanımı için bu satır gereklidir
app.UseSession();
// Localization middleware
app.UseRequestLocalization();

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting(); // Routing middleware'ini ilk sırada kullanın
app.UseAuthorization(); // Authorization ve diğer middleware'ler sonrasında

app.MapControllerRoute(
    name: "localized",
    pattern: "{lang?}/{controller=Home}/{action=Index}/{id?}");



app.Run();
