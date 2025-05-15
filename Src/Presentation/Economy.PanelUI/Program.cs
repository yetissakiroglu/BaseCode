using Autofac.Core;
using Economy.Application.BaseRepositories;
using Economy.Base.Application.BaseRepositories;
using Economy.Base.Application.Interfaces;
using Economy.Base.Persistence.BaseRepositories;
using Economy.Base.Persistence.Providers;
using Economy.Core.Business;
using Economy.Core.ContextFactory;
using Economy.Core.Interfaces;
using Economy.Core.Interfaces.Economy.Panel.Application.Repositories;
using Economy.Core.Interfaces.Economy.Panel.Persistence.Services;
using Economy.Core.Services.Providers;
using Economy.Domain.Entites.Identities;
using Economy.Infrastructure.Services;
using Economy.Panel.Application.Interfaces;
using Economy.Panel.Application.Repositories;
using Economy.Panel.Persistence.Repositories;
using Economy.Panel.Persistence.Services;
using Economy.Panel.UI;
using Economy.Panel.UI.Middlewares;
using Economy.Persistence.BaseRepositories;
using Economy.Persistence.Contexts;
using Economy.Persistence.UnitOfWorks;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);


// MVC ve Razor Pages'ý ekleyin
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<DefaultDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"), configure =>
    {
        configure.MigrationsAssembly("Economy.Panel.UI");
    });
});

builder.Services.AddDbContext<HotelDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultHotelConnection"), configure =>
    {
        configure.MigrationsAssembly("Economy.Base.Persistence");
    });
});



// TokenOption ayarlarýný oku ve DI container'a ekle
builder.Services.Configure<TokenOption>(
    builder.Configuration.GetSection("TokenOption"));

// TokenOption doðrudan kullanýlacaksa (örneðin TokenService içinde ctor ile)
var tokenOptions = builder.Configuration.GetSection("TokenOption").Get<TokenOption>();
builder.Services.AddSingleton(tokenOptions);


builder.Services.AddIdentity<AppUser, AppRole>(options =>
{
    //options.SignIn.RequireConfirmedAccount =false;
    //options.Lockout.DefaultLockoutTimeSpan = 
    //options.Lockout.MaxFailedAccessAttempts = 
    // User Password Options
    options.Password.RequireDigit = false;
    options.Password.RequiredLength = 5;
    options.Password.RequiredUniqueChars = 0;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireUppercase = false;
    // User Username and Email Options
    options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+$";
    options.User.RequireUniqueEmail = true;
}).AddEntityFrameworkStores<DefaultDbContext>()
    .AddRoles<AppRole>().AddDefaultTokenProviders();

// Add Authentication and Cookie Configuration
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = new PathString("/Account/Login");
        options.LogoutPath = new PathString("/Account/Logout");
        options.Cookie.Name = "DijitalPanel";
        options.SlidingExpiration = true;
        options.ExpireTimeSpan = TimeSpan.FromDays(7);
    });


builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = new PathString("/Account/Login");
    options.LogoutPath = new PathString("/Account/Logout");
    options.Cookie = new CookieBuilder
    {
        Name = "DijitalPanel",
        HttpOnly = true,
        SameSite = SameSiteMode.Strict,
        SecurePolicy = CookieSecurePolicy.SameAsRequest // Always
    };
    options.SlidingExpiration = true;
    options.ExpireTimeSpan = System.TimeSpan.FromDays(7);
    options.AccessDeniedPath = new PathString($"/Error/{HttpStatusCode.Forbidden}");
});

// Repository'leri otomatik olarak ekle
builder.Services.AddRepositories(Assembly.GetExecutingAssembly());
// EfEntityRepositoryBase<T> kaydý
// HotelDbContextFactory'nin kaydedilmesi
builder.Services.AddScoped<IHotelDbContextFactory, HotelDbContextFactory>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

// IAppUserTokenBaseRepository ve AppUserTokenBaseRepository kaydýný yapalým.
builder.Services.AddScoped<IAppUserTokenBaseRepository, AppUserTokenBaseRepository>();
builder.Services.AddScoped<IAppUserBaseRepository, AppUserBaseRepository>();


// PanelAppUserRepository ve ConcretePanelAppUserRepository kaydýný yapalým.
builder.Services.AddScoped<PanelAppUserRepository, ConcretePanelAppUserRepository>(); // Concrete sýnýfý kullanýyoruz.
builder.Services.AddScoped<PanelAppUserTokenRepository, ConcretePanelAppUserTokenRepository>(); // Token repository'si.

// Service kaydýný yapalým.
builder.Services.AddScoped<IPanelAppUserService, PanelAppUserService>(); // Service sýnýfý kaydediliyor.



builder.Services.AddScoped<IAppBaseRepository, AppBaseRepository>();
builder.Services.AddScoped<PanelAppRepository, ConcretePanelAppRepository>(); // Concrete sýnýfý kullanýyoruz.
builder.Services.AddScoped<IPanelAppService, PanelAppService>(); // Service sýnýfý kaydediliyor.


builder.Services.AddScoped<IPanelAppSettingService, PanelAppSettingService>(); // Service sýnýfý kaydediliyor.
builder.Services.AddScoped<IPanelAppLanguageService, PanelAppLanguageService>(); // Service sýnýfý kaydediliyor.
builder.Services.AddScoped<IPanelAppSettingLogoService, PanelAppSettingLogoService>(); // Service sýnýfý kaydediliyor.
builder.Services.AddScoped<IPanelAppSettingReservationLinkService, PanelAppSettingReservationLinkService>(); // Service sýnýfý kaydediliyor.
builder.Services.AddScoped<IPanelAppSettingReservationNumberService, PanelAppSettingReservationNumberService>(); // Service sýnýfý kaydediliyor.
builder.Services.AddScoped<IPanelAppSettingWhatsappLineService, PanelAppSettingWhatsappLineService>(); // Service sýnýfý kaydediliyor.


// Token service kaydýný yapalým.
builder.Services.AddScoped<ITokenService, TokenService>(); // Token service kaydý
// Diðer servisler (örneðin AutoMapper)
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());




// Services
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<TenantProvider>();
builder.Services.AddScoped<MigrationService>();


builder.Services.Configure<FormOptions>(options =>
{
    options.MultipartBodyLengthLimit = 104857600; // 100 MB gibi büyük bir limit
});

var app = builder.Build();

// Uygulama baþlatýldýðýnda migrasyonlarý çalýþtýrmak için örneðin þöyle bir iþlev ekleyebilirsiniz:
using (var scope = app.Services.CreateScope())
{
    var migrationService = scope.ServiceProvider.GetRequiredService<MigrationService>();

    // Master veritabaný migrasyonunu baþlatma
    await migrationService.MigrateMasterDbAsync();

    // Ýstenirse her bir tenant için de migrasyon yapýlabilir
    // await migrationService.MigrateTenantAsync(tenantId);
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
// HTTPS yönlendirmesi ve routing iþlemleri
app.UseHttpsRedirection();
app.UseRouting();
app.MapStaticAssets();

// MVC, Razor Sayfalarý ve Blazor bileþenleri için routing iþlemleri
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// Authorization ve Authentication iþlemleri
app.UseAuthentication();
app.UseAuthorization();

// Custom middleware
app.UseMiddleware<HotelConnectionMiddleware>();
// Uygulamayý çalýþtýr
app.Run();
//// Https yönlendirmesi ve routing iþlemleri
//app.UseHttpsRedirection();
//app.UseRouting();

//app.MapStaticAssets();

//// Authorization ve Authentication iþlemleri
//app.UseAuthentication();
//app.UseAuthorization();

//app.MapRazorPages();


//app.MapControllerRoute(
//    name: "default",
//    pattern: "{controller=Home}/{action=Index}/{id?}");


//// Uygulamayý çalýþtýr
//app.Run();