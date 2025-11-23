using Economy.Application.AdminUI.Dtos.AppAccountDtos;
using Economy.Application.AdminUI.Dtos.AppDtos;
using Economy.Application.AdminUI.Dtos.AppGeneralSettingDtos;
using Economy.Application.AdminUI.Dtos.AppSuperAdminUserDtos;
using Economy.Application.AdminUI.Interfaces;
using Economy.Application.AdminUI.Validations.AppSuperAdminValidator;
using Economy.Application.AdminUI.Validations.AppValidator;
using Economy.Application.AdminUI.Validations.PanelAppAccountValidator;
using Economy.Application.ApplicationUI.Interfaces;
using Economy.Application.Interfaces;
using Economy.Application.Providers;
using Economy.Application.TenantUI.Dtos.AppMenuDtos;
using Economy.Application.TenantUI.Dtos.AppSettingDtos;
using Economy.Application.TenantUI.Dtos.AppSettingLogoDtos;
using Economy.Application.TenantUI.Dtos.AppTechnicalSettingDtos;
using Economy.Application.TenantUI.Interfaces;
using Economy.Application.TenantUI.Validations.AppMenuIValidator;
using Economy.Application.TenantUI.Validations.AppSettingLogoValidator;
using Economy.Application.TenantUI.Validations.AppSettingValidator;
using Economy.Application.TenantUI.Validations.AppTechnicalSettingValidator;
using Economy.Core.ContextFactory;
using Economy.Core.Core;
using Economy.Core.Dtos;
using Economy.Core.Helpers;
using Economy.Core.Interfaces;
using Economy.Core.Options;
using Economy.Domain.Entites.AdminEntity.EntityAppUsers;
using Economy.Infrastructure;
using Economy.Panel.UI;
using Economy.Panel.UI.Filters;
using Economy.Panel.UI.Middlewares;
using Economy.Panel.UI.Tenancy;
using Economy.Persistence.Admin.Services;
using Economy.Persistence.ApplicationUI;
using Economy.Persistence.Contexts;
using Economy.Persistence.Providers;
using Economy.Persistence.Repositories.UnitOfWork;
using Economy.Persistence.Services;
using Economy.Persistence.Tenant.Services;
using FluentValidation;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Microsoft.OpenApi.Models;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);


// MVC ve Razor Pages'ı ekleyin
builder.Services.AddControllersWithViews(o =>
{
    o.Filters.Add<SeoAndBrandingFilter>();
}).AddRazorRuntimeCompilation();

// (İsteğe bağlı ama faydalı) CORS – UI başka origin'den çağıracaksa aç
//builder.Services.AddCors(opt =>
//{
//    opt.AddPolicy("UI", p => p
//        .WithOrigins("http://xotel.local:5011", "https://senin-ui-domainin.com")
//        .AllowAnyHeader()
//        .AllowAnyMethod());
//});

// (İsteğe bağlı) Sıkıştırma + Caching
//builder.Services.AddResponseCompression();
//builder.Services.AddResponseCaching();

// (İsteğe bağlı) Swagger – dev’de API’yi gör
// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(opt =>
{
    opt.SwaggerDoc("v1", new OpenApiInfo { Title = "Hotel UI API", Version = "v1" });
    opt.DocInclusionPredicate((docName, apiDesc) =>
    {
        if (!string.Equals(docName, "v1", StringComparison.OrdinalIgnoreCase)) return false;
        var isApiArea = apiDesc.ActionDescriptor.RouteValues.TryGetValue("area", out var area)
                        && string.Equals(area, "Api", StringComparison.OrdinalIgnoreCase);
        return isApiArea;
    });
    opt.OperationFilter<AddTenantHeaderOperationFilter>();
});



// Services
builder.Services.AddHttpContextAccessor();
builder.Services.AddSingleton<ITenantContextAccessor, TenantContextAccessor>();


// DefaultDb (sabit connection) – Tenants, TenantDomains, UserTenants burada
builder.Services.AddDbContext<DefaultDbContext>(o =>
    o.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// TenantDb (dinamik connection – OnConfiguring içinde set edilecek)
builder.Services.AddDbContext<HotelDbContext>(o =>
    o.UseSqlServer("Server=.;Database=__placeholder;Trusted_Connection=True;"));





//builder.Services.AddDbContext<DefaultDbContext>(options =>
//{
//    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"), configure =>
//    {
//        configure.MigrationsAssembly("Economy.Panel.UI");
//    });
//});

//builder.Services.AddDbContext<HotelDbContext>(options =>
//{
//    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultHotelConnection"), configure =>
//    {
//        configure.MigrationsAssembly("Economy.Base.Persistence");
//    });
//});


// FileManager ayarları + storage
builder.Services.Configure<FileManagerOptions>(builder.Configuration.GetSection("FileManager"));
builder.Services.AddSingleton<IImageStorage, LocalImageStorage>();

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
    options.AccessDeniedPath = "/Error/403"; // Aynı satırı iki kez set etmeyelim

    options.Cookie = new CookieBuilder
    {
        Name = "DijitalPanel",
        HttpOnly = true,
        SameSite = SameSiteMode.Strict,      // dış yönlendirme/iframe gerekirse Lax yaparsın
        SecurePolicy = CookieSecurePolicy.SameAsRequest // prod HTTPS’te Always önerilir
    };

    options.SlidingExpiration = true;
    options.ExpireTimeSpan = TimeSpan.FromDays(7);
});


builder.Services.AddMemoryCache();
builder.Services.AddScoped<IAppSettingsProvider, AppSettingsProvider>();
// Repository'leri otomatik olarak ekle
builder.Services.AddRepositories(Assembly.GetExecutingAssembly());
// EfEntityRepositoryBase<T> kaydı
// HotelDbContextFactory'nin kaydedilmesi
builder.Services.AddScoped<IHotelDbContextFactory, HotelDbContextFactory>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

// Service kaydını yapalım.
builder.Services.AddScoped<IPanelAppManagerService, PanelAppManagerService>(); // Service sınıfı kaydediliyor.
builder.Services.AddScoped<IPanelSuperAdminService, PanelSuperAdminService>(); // Service sınıfı kaydediliyor.
builder.Services.AddScoped<IValidator<AppSuperAdminUserCreateDto>, SuperAdminCreateDtoValidator>();
builder.Services.AddScoped<IValidator<AppSuperAdminUserEditDto>, SuperAdminEditDtoValidator>();
builder.Services.AddScoped<IPanelAppMenuService, PanelAppMenuService>();
builder.Services.AddScoped<IConnectionTesterService, ConnectionTesterService>(); // Service sınıfı kaydediliyor.
builder.Services.AddScoped<IPanelDashboardService, PanelDashboardService>();
builder.Services.AddScoped<IPanelAppTechnicalSettingService, PanelAppTechnicalSettingService>();
builder.Services.AddScoped<IValidator<MenuItemDto>, AppMenuItemValidator>();


builder.Services.AddScoped<IPanelAppBlockGroupService, PanelAppBlockGroupService>();
builder.Services.AddScoped<IPanelPageBlockGroupService, PanelPageBlockGroupService>();

builder.Services.AddScoped<IValidator<AppGeneralSettingCreateDto>, AppGeneralSettingCreateDtoValidator>();
builder.Services.AddScoped<IValidator<AppGeneralSettingEditDto>, AppGeneralSettingEditDtoValidator>();
builder.Services.AddScoped<IValidator<AppTechnicalSettingCreateEditDto>, AppTechnicalSettingCreateEditDtoValidator>();
builder.Services.AddScoped<IValidator<AppSettingCreateEditDto>, AppSettingCreateEditDtoValidator>();
builder.Services.AddTransient<IValidator<AppSettingLogoCreateEditDto>, AppSettingLogoCreateEditDtoValidator>();

builder.Services.AddTransient<IValidator<AppCreateEditDto>, AppCreateEditDtoValidator>();
builder.Services.AddTransient<IValidator<AppSignInDto>, AppSignInDtoValidator>();



builder.Services.AddScoped<IPanelAppService, PanelAppService>(); // Service sınıfı kaydediliyor.
builder.Services.AddScoped<IPanelAppSettingService, PanelAppSettingService>(); // Service sınıfı kaydediliyor.
builder.Services.AddScoped<IPanelAppLanguageService, PanelAppLanguageService>(); // Service sınıfı kaydediliyor.
builder.Services.AddScoped<IPanelAppSettingLogoService, PanelAppSettingLogoService>(); // Service sınıfı kaydediliyor.
builder.Services.AddScoped<IPanelAppGeneralSettingService, PanelAppGeneralSettingService>(); // Service sınıfı kaydediliyor.
builder.Services.AddScoped<IDatabaseBackupService, DatabaseBackupService>();
builder.Services.AddScoped<IPanelAppAccountService, PanelAppAccountService>();
builder.Services.AddScoped<IPanelAppPageService, PanelAppPageService>();
builder.Services.AddScoped<IPanelAppPageMediaService, PanelAppPageMediaService>();
builder.Services.AddScoped<IPanelAppBlockService, PanelAppBlockService>();

builder.Services.AddScoped<IApplicationMenuService, ApplicationMenuService>();



builder.Services.Configure<SeoOptions>(builder.Configuration.GetSection("Seo"));
builder.Services.AddScoped<ISlugService, SlugService>();

builder.Services.AddScoped<IFileImageHelperService, FileImageHelperService>(); // Token service kaydı

// Diğer servisler (örneğin AutoMapper)
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());


//builder.Services.AddScoped<ICdnUrlService, CdnUrlService>();

//builder.Services.AddScoped<TenantProvider>();

//builder.Services.Configure<FormOptions>(options =>
//{
//    options.MultipartBodyLengthLimit = 104857600; // 100 MB gibi büyük bir limit
//});




var app = builder.Build();
app.UseMiddleware<ExceptionHandlingMiddleware>();
var fileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "Files"));
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

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
     // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
// HTTPS yönlendirmesi ve routing işlemleri
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.MapStaticAssets();
app.UseResponseCaching();

//app.UseResponseCompression();
//app.UseCors("UI");

// Authorization ve Authentication işlemleri
app.UseAuthentication();
// 1) Tenant seçimi zorunluluğu
app.UseMiddleware<PanelTenantSelectionGuardMiddleware>();

// 2) Tenant DB bağlantısını set et
app.UseMiddleware<HotelConnectionMiddleware>();

app.UseAuthorization();
//app.UseMiddleware<ErrorLoggingMiddleware>();
// Swagger UI
// 🔸 Klasik Swagger kullanımı (UseSwagger / UseSwaggerUI)
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Hotel UI API v1");
    c.RoutePrefix = "swagger";
});
// Area routing: önce Areas!
//app.MapControllerRoute(
//    name: "areas",
//    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");
// Areas (Api + diğerleri)
app.MapAreaControllerRoute("api", "Api", "api/{controller=Home}/{action=Index}/{id?}");
app.MapAreaControllerRoute("tenant", "Tenant", "tenant/{controller=Home}/{action=Index}/{id?}");
app.MapAreaControllerRoute("admin", "Admin", "admin/{controller=Dashboard}/{action=Index}/{id?}");

// MVC, Razor Sayfaları ve Blazor bileşenleri için routing işlemleri
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");


// Uygulamayı çalıştır
app.Run();
