using AppWeb.ActionFilters;
using AppWeb.Providers;
using Economy.Application;
using Economy.Persistence;
using Economy.Persistence.Seeds;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
// AppSettingsActionFilter'ý global olarak kaydedin
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
    // Action Filter'ý global olarak uygulamak
    options.Filters.AddService<AppSettingsActionFilter>();
})
    .AddDataAnnotationsLocalization()
    .AddViewLocalization();

// HttpContextAccessor'ý DI container'a ekliyoruz
builder.Services.AddHttpContextAccessor();

// LanguageProvider'ý DI container'a ekliyoruz ve kullanýcý dil saðlayýcýyý kullanýyoruz
builder.Services.AddScoped<LanguageProvider, UserLanguageProvider>();



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

// Session kullanýmý için bu satýr gereklidir
app.UseSession();
// Localization middleware
app.UseRequestLocalization();

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting(); // Routing middleware'ini ilk sýrada kullanýn
app.UseAuthorization(); // Authorization ve diðer middleware'ler sonrasýnda

app.MapControllerRoute(
    name: "localized",
    pattern: "{lang?}/{controller=Home}/{action=Index}/{id?}");



app.Run();
