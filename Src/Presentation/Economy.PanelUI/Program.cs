using Economy.Base.Persistence.ContextFactorys;
using Economy.Base.Persistence.Providers;
using Economy.Core.Interfaces;
using Economy.Core.Services.Providers;
using Economy.Domain.Entites.Identities;
using Economy.Persistence.Contexts;
using Economy.Persistence.UnitOfWorks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Net;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<DefaultDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"), configure =>
    {
        configure.MigrationsAssembly("Economy.Panel.UI");
    });
});

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


// Services
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<TenantProvider>();
builder.Services.AddScoped<MigrationService>();
builder.Services.AddScoped<HotelDbContextFactory>();

// UnitOfWork Master DB için
builder.Services.AddScoped<IUnitOfWork>(sp =>
    new UnitOfWork(sp.GetRequiredService<HotelDbContext>()));



//builder.Services.AddDbContext<AppDbContext>(options =>
//{
//    var connectionString = builder.Configuration.GetConnectionString("SqlServer") ?? throw new InvalidOperationException("Connection string 'SqlServer' not found.");
//    options.UseSqlServer(connectionString, configure =>
//    {
//        configure.MigrationsAssembly("Economy.Base.Persistence");
//    });
//});


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

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();
