using Economy.Application.ApplicationUI.Interfaces;
using Economy.Application.Dtos.AppDtos;
using Economy.Application.Dtos.AppGeneralSettingDtos;
using Economy.Application.Dtos.AppSecuritySettingDtos;
using Economy.Application.Dtos.AppSuperAdminUserDtos;
using Economy.Application.Interfaces;
using Economy.Application.Providers;
using Economy.Application.TenantUI.Dtos.AppMenuDtos;
using Economy.Application.TenantUI.Dtos.AppSettingDtos;
using Economy.Application.TenantUI.Dtos.AppSettingLogoDtos;
using Economy.Application.TenantUI.Dtos.AppSlideDtos;
using Economy.Application.TenantUI.Dtos.AppTechnicalSettingDtos;
using Economy.Application.TenantUI.Interfaces;
using Economy.Application.TenantUI.Validations.AppMenuIValidator;
using Economy.Application.TenantUI.Validations.AppSettingLogoValidator;
using Economy.Application.TenantUI.Validations.AppSettingValidator;
using Economy.Application.TenantUI.Validations.AppSlideValidator;
using Economy.Application.TenantUI.Validations.AppTechnicalSettingValidator;
using Economy.Application.Validations.AppSecuritySettingValidator;
using Economy.Application.Validations.AppSuperAdminValidator;
using Economy.Application.Validations.AppUserValidator;
using Economy.Application.Validations.AppValidator;
using Economy.Base.Application.Dtos.BaseModels;
using Economy.Base.Persistence.Providers;
using Economy.Core.Business;
using Economy.Core.ContextFactory;
using Economy.Core.Core;
using Economy.Core.Helpers;
using Economy.Core.Helpers.Dtos;
using Economy.Core.Interfaces;
using Economy.Core.Interfaces.Economy.Panel.Persistence.Services;
using Economy.Core.Options;
using Economy.Core.Services.Providers;
using Economy.Domain.Entites.Identities;
using Economy.Infrastructure.Services;
using Economy.Panel.Application.Interfaces;
using Economy.Panel.Persistence.Services;
using Economy.Panel.UI;
using Economy.Panel.UI.Filters;
using Economy.Panel.UI.Middlewares;
using Economy.Persistence.Contexts;
using Economy.Persistence.PersistenceUI.Services;
using Economy.Persistence.Providers;
using Economy.Persistence.Services;
using Economy.Persistence.Tenant.Services;
using Economy.Persistence.UnitOfWorks;
using FluentValidation;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using System.Net;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);


// MVC ve Razor Pages'ý ekleyin
builder.Services.AddControllersWithViews(o =>
{
    o.Filters.Add<SeoAndBrandingFilter>();
}).AddRazorRuntimeCompilation(); 

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


// FileManager ayarlarý + storage
builder.Services.Configure<FileManagerOptions>(
    builder.Configuration.GetSection("FileManager"));
builder.Services.AddSingleton<IImageStorage, LocalImageStorage>();

// TokenOption ayarlarýný oku ve DI container'a ekle
builder.Services.Configure<TokenOption>(
    builder.Configuration.GetSection("TokenOption"));

// TokenOption doðrudan kullanýlacaksa (örneðin TokenService içinde ctor ile)
var tokenOptions = builder.Configuration.GetSection("TokenOption").Get<TokenOption>();
builder.Services.AddSingleton(tokenOptions);

//  ayarlarýný oku ve DI container'a ekle
builder.Services.Configure<FileUploadConfiguration>(
    builder.Configuration.GetSection("FileUploadConfiguration"));

//  doðrudan kullanýlacaksa (örneðin TokenService içinde ctor ile)
var fileUploadOptions = builder.Configuration.GetSection("FileUploadConfiguration").Get<FileUploadConfiguration>();
builder.Services.AddSingleton(fileUploadOptions);


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

builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Account/Login";
    options.LogoutPath = "/Account/Logout";
    options.AccessDeniedPath = "/Error/403";
    options.Cookie = new CookieBuilder
    {
        Name = "DijitalPanel",
        HttpOnly = true,
        SameSite = SameSiteMode.Strict,
        SecurePolicy = CookieSecurePolicy.SameAsRequest
    };
    options.SlidingExpiration = true;
    options.ExpireTimeSpan = TimeSpan.FromDays(7);
    options.AccessDeniedPath = new PathString($"/Error/{HttpStatusCode.Forbidden}");

});

//builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
//    .AddCookie(options =>
//    {
//        options.LoginPath = new PathString("/Account/Login");
//options.LogoutPath = new PathString("/Account/Logout");
//options.Cookie.Name = "DijitalPanel";
//options.SlidingExpiration = true;
//options.ExpireTimeSpan = TimeSpan.FromDays(7);
//    });


//builder.Services.ConfigureApplicationCookie(options =>
//{
//    options.LoginPath = new PathString("/Account/Login");
//    options.LogoutPath = new PathString("/Account/Logout");
//    options.Cookie = new CookieBuilder
//    {
//        Name = "DijitalPanel",
//        HttpOnly = true,
//        SameSite = SameSiteMode.Strict,
//        SecurePolicy = CookieSecurePolicy.SameAsRequest // Always
//    };
//    options.SlidingExpiration = true;
//    options.ExpireTimeSpan = System.TimeSpan.FromDays(7);
//    options.AccessDeniedPath = new PathString($"/Error/{HttpStatusCode.Forbidden}");
//});
builder.Services.AddMemoryCache();
builder.Services.AddScoped<IAppSettingsProvider, AppSettingsProvider>();
// Repository'leri otomatik olarak ekle
builder.Services.AddRepositories(Assembly.GetExecutingAssembly());
// EfEntityRepositoryBase<T> kaydý
// HotelDbContextFactory'nin kaydedilmesi
builder.Services.AddScoped<IHotelDbContextFactory, HotelDbContextFactory>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

// Service kaydýný yapalým.
builder.Services.AddScoped<IPanelAppUserService, PanelAppUserService>(); // Service sýnýfý kaydediliyor.
builder.Services.AddScoped<IPanelAppManagerService, PanelAppManagerService>(); // Service sýnýfý kaydediliyor.
builder.Services.AddScoped<IPanelSuperAdminService, PanelSuperAdminService>(); // Service sýnýfý kaydediliyor.
builder.Services.AddScoped<IValidator<AppSuperAdminUserCreateDto>, SuperAdminCreateDtoValidator>();
builder.Services.AddScoped<IValidator<AppSuperAdminUserEditDto>, SuperAdminEditDtoValidator>();

builder.Services.AddScoped<IPanelAppMenuService, PanelAppMenuService>();
builder.Services.AddScoped<ISiteConfigAccessor, SiteConfigAccessor>();
builder.Services.AddScoped<IMenuAccessor, MenuAccessor>(); //
builder.Services.AddScoped<IPageAccessor, PageAccessor>();

builder.Services.AddScoped<IConnectionTesterService, ConnectionTesterService>(); // Service sýnýfý kaydediliyor.
builder.Services.AddScoped<IAuditLogWriter, AuditLogWriter>();
builder.Services.AddScoped<IPanelDashboardService, PanelDashboardService>();
builder.Services.AddScoped<IPanelLoginLogService, PanelLoginLogService>();
builder.Services.AddScoped<IPanelAppTechnicalSettingService, PanelAppTechnicalSettingService>();

builder.Services.AddScoped<IValidator<MenuItemDto>, AppMenuItemValidator>();

builder.Services.AddScoped<IValidator<AppUserCreateDto>, AppUserCreateDtoValidator>();

builder.Services.AddScoped<IValidator<AppGeneralSettingCreateDto>, AppGeneralSettingCreateDtoValidator>();
builder.Services.AddScoped<IValidator<AppGeneralSettingEditDto>, AppGeneralSettingEditDtoValidator>();

builder.Services.AddScoped<IValidator<AppSecuritySettingCreateDto>, AppSecuritySettingCreateDtoValidator>();
builder.Services.AddScoped<IValidator<AppSecuritySettingEditDto>, AppSecuritySettingEditDtoValidator>();
builder.Services.AddScoped<IValidator<AppTechnicalSettingCreateEditDto>, AppTechnicalSettingCreateEditDtoValidator>();
builder.Services.AddScoped<IValidator<AppSettingCreateEditDto>, AppSettingCreateEditDtoValidator>();



builder.Services.AddScoped<IValidator<AppSlideCreateEditDto>, AppSlideCreateEditDtoValidator>();
builder.Services.AddTransient<IValidator<AppSettingLogoCreateEditDto>, AppSettingLogoCreateEditDtoValidator>();



builder.Services.AddTransient<IValidator<AppCreateEditDto>, AppCreateEditDtoValidator>();

builder.Services.AddScoped<IPanelAppService, PanelAppService>(); // Service sýnýfý kaydediliyor.
builder.Services.AddScoped<IPanelAppSettingService, PanelAppSettingService>(); // Service sýnýfý kaydediliyor.
builder.Services.AddScoped<IPanelAppLanguageService, PanelAppLanguageService>(); // Service sýnýfý kaydediliyor.
builder.Services.AddScoped<IPanelAppSettingLogoService, PanelAppSettingLogoService>(); // Service sýnýfý kaydediliyor.
builder.Services.AddScoped<IPanelAppSlideService, PanelAppSlideService>(); // Service sýnýfý kaydediliyor.
builder.Services.AddScoped<IPanelAppGeneralSettingService, PanelAppGeneralSettingService>(); // Service sýnýfý kaydediliyor.
builder.Services.AddScoped<IPanelAppSecuritySettingService, PanelAppSecuritySettingService>();
builder.Services.AddScoped<IPanelAuditLogService, PanelAuditLogService>();
builder.Services.AddScoped<IPanelErrorLogService, PanelErrorLogService>();
builder.Services.AddScoped<IDatabaseBackupService, DatabaseBackupService>();
builder.Services.AddScoped<IPanelAppAccountService, PanelAppAccountService>();
builder.Services.AddScoped<IPanelAppPageService, PanelAppPageService>();



// Token service kaydýný yapalým.
builder.Services.AddScoped<ITokenService, TokenService>(); // Token service kaydý

builder.Services.AddScoped<IFileImageHelperService, FileImageHelperService>(); // Token service kaydý


// Diðer servisler (örneðin AutoMapper)
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());




// Services
builder.Services.AddHttpContextAccessor();
//builder.Services.AddScoped<ICdnUrlService, CdnUrlService>();

builder.Services.AddScoped<TenantProvider>();
builder.Services.AddScoped<MigrationService>();


builder.Services.Configure<FormOptions>(options =>
{
    options.MultipartBodyLengthLimit = 104857600; // 100 MB gibi büyük bir limit
});




var app = builder.Build();
app.UseMiddleware<ExceptionHandlingMiddleware>();


var fileProvider = new PhysicalFileProvider(
    Path.Combine(Directory.GetCurrentDirectory(), "Files"));

app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = fileProvider,
    RequestPath = "/Files"
});

//todo bak
app.UseForwardedHeaders(new ForwardedHeadersOptions
{
    ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
});

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

// Area routing: önce Areas!
app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");
// MVC, Razor Sayfalarý ve Blazor bileþenleri için routing iþlemleri
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// Authorization ve Authentication iþlemleri
app.UseAuthentication();
app.UseAuthorization();
app.UseMiddleware<ErrorLoggingMiddleware>();

// Custom middleware
app.UseMiddleware<HotelConnectionMiddleware>();
// Uygulamayý çalýþtýr
app.Run();
