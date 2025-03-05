using AppWeb.ActionFilters;
using Economy.Application;
using Economy.Persistence;
using Economy.Persistence.Seeds;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
// AppSettingsActionFilter'ý global olarak kaydedin
builder.Services.AddScoped<AppSettingsActionFilter>();

// MVC servislerini ekliyoruz
builder.Services.AddControllersWithViews(options =>
{
    // Action Filter'ý global olarak uygulamak
    options.Filters.AddService<AppSettingsActionFilter>();
})
    .AddDataAnnotationsLocalization()
    .AddViewLocalization();


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


// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
// Geliþtirme ortamý kontrolü ve cookie temizliði
// Cookie'yi temizle
app.Use(async (context, next) =>
{
    var routeData = context.GetRouteData();
    var lang1 = routeData?.Values["lang"]?.ToString();

    var routeValues = context.Request.RouteValues;

    // Eðer dil parametresi varsa, cookie'yi güncelle
    var lang = context.Request.RouteValues["lang"]?.ToString();
    if (!string.IsNullOrEmpty(lang))
    {  // Cookie'yi temizliyoruz
        var currentCulture = context.Request.Cookies["CurrentCulture"];
        context.Response.Cookies.Append("CurrentCulture", lang, new CookieOptions { Expires = DateTime.Now.AddYears(1) });
    }
    //else if (string.IsNullOrEmpty(currentCulture))
    //{
    //    // Cookie'de dil bilgisi yoksa, varsayýlan bir dil atayýn (örneðin "tr" veya "en")
    //    context.Response.Cookies.Append("CurrentCulture", "en", new CookieOptions { Expires = DateTime.Now.AddYears(1) });
    //}

    // Sonraki middleware'a geçiþ
    await next.Invoke();
});



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
