using Economy.Web.Demo1.Helpers;
using Economy.Web.Demo1.Middlewares;
using Economy.Web.Demo1.Services;
using Microsoft.AspNetCore.Mvc.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddHttpClient("ApiClient", client =>
{
    client.BaseAddress = new Uri("https://localhost:7248"); // API kök adresi
});
builder.Services.AddScoped<IApiClientHelper, ApiClientHelper>();
builder.Services.AddScoped<ISiteConfigAccessor, SiteConfigAccessor>();
builder.Services.AddScoped<IMenuService, MenuService>();

builder.Services.AddScoped<ISeoHelper, SeoHelper>();
builder.Services.AddHttpContextAccessor();
builder.Services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();

builder.Services.ConfigureApplicationCookie(opts =>
{
    opts.Cookie.HttpOnly = true;
    opts.Cookie.SecurePolicy = CookieSecurePolicy.Always;
    opts.Cookie.SameSite = SameSiteMode.Lax;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}


app.UseMiddleware<LangResolverMiddleware>();
app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthorization();
app.MapStaticAssets();

// (Teþhis) eþleþen endpoint’i consola yaz
app.Use(async (ctx, next) =>
{
    await next();
    Console.WriteLine($"[ROUTE] {ctx.Request.Path} -> {ctx.GetEndpoint()?.DisplayName ?? "(eþleþme yok)"}");
});

// 1) /{lang}/{slug}  -> Pages.Index
app.MapControllerRoute(
    name: "pages-with-slug",
    pattern: "{lang:length(2)}/{slug}",
    defaults: new { controller = "Pages", action = "Index" }
);

// 2) /{lang}         -> Pages.Anasayfa
app.MapControllerRoute(
    name: "pages-root",
    pattern: "{lang:length(2)}",
    defaults: new { controller = "Pages", action = "Anasayfa" }
);

// (opsiyonel) diðer controller/action rotalarý en SONDA kalsýn
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}"
).WithStaticAssets(); 

// (opsiyonel) kökü /tr’ye yönlendir
//app.MapGet("/", ctx => { ctx.Response.Redirect("/tr", false); return Task.CompletedTask; });




app.Run();
