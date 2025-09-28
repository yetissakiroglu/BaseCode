using Economy.Web.Demo1UI.Helpers;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.AspNetCore.ResponseCompression;
using MyHotelSite.Middlewares;
using MyHotelSite.Repositories;
using MyHotelSite.Services;

var builder = WebApplication.CreateBuilder(args);

// MVC + Razor
builder.Services.AddControllersWithViews();


builder.Services.AddHttpClient("ApiClient", client =>
{
    client.BaseAddress = new Uri("https://localhost:7248"); // API kök adresi
});

builder.Services.AddScoped<IApiClientHelper, ApiClientHelper>();

// Caching & Compression & OutputCache
builder.Services.AddMemoryCache();
// Program.cs
builder.Services.AddResponseCompression(o =>
{
    o.EnableForHttps = true;
    o.Providers.Clear();
    o.Providers.Add<BrotliCompressionProvider>();
    o.Providers.Add<GzipCompressionProvider>();
});
builder.Services.Configure<BrotliCompressionProviderOptions>(o => o.Level = System.IO.Compression.CompressionLevel.Optimal);
builder.Services.Configure<GzipCompressionProviderOptions>(o => o.Level = System.IO.Compression.CompressionLevel.Optimal);

// Program.cs
builder.Services.ConfigureApplicationCookie(opts =>
{
    opts.Cookie.HttpOnly = true;
    opts.Cookie.SecurePolicy = CookieSecurePolicy.Always;
    opts.Cookie.SameSite = SameSiteMode.Lax;
});

builder.Services.AddOutputCache(o =>
{
    o.AddPolicy("LangAnon300", b => b.Expire(TimeSpan.FromSeconds(300)).SetVaryByRouteValue("lang"));
    o.AddPolicy("LangAnon900", b => b.Expire(TimeSpan.FromSeconds(900)).SetVaryByRouteValue("lang"));
});

builder.Services.AddRateLimiter(o =>
{
    o.AddFixedWindowLimiter("contact-post", opt =>
    {
        opt.Window = TimeSpan.FromMinutes(1);
        opt.PermitLimit = 5; // dakika başına 5 POST
        opt.QueueLimit = 0;
    });
});

// Http helpers
builder.Services.AddHttpContextAccessor();
builder.Services.AddSingleton<Microsoft.AspNetCore.Mvc.Infrastructure.IActionContextAccessor, Microsoft.AspNetCore.Mvc.Infrastructure.ActionContextAccessor>();
//builder.Services.AddSingleton<Microsoft.AspNetCore.Mvc.IUrlHelperFactory, Microsoft.AspNetCore.Mvc.Routing.UrlHelperFactory>();

// Fake repositories

builder.Services.AddSingleton<ILanguageService, FakeLanguageService>();
builder.Services.AddSingleton<IPageRepository, FakePageRepository>();
builder.Services.AddSingleton<IRoomRepository, FakeRoomRepository>();
builder.Services.AddSingleton<ICampaignRepository, FakeCampaignRepository>();
builder.Services.AddSingleton<IGalleryRepository, FakeGalleryRepository>();
builder.Services.AddSingleton<ILocalizationRepository, FakeLocalizationRepository>();

// Services
builder.Services.AddScoped<ISiteConfigAccessor, SiteConfigAccessor>();
builder.Services.AddScoped<ISeoHelper, SeoHelper>();
builder.Services.AddScoped<ISeoUrlHelper, SeoUrlHelper>();
builder.Services.AddScoped<ICdnHelper, CdnHelper>();
builder.Services.AddScoped<IMenuService, MenuService>();

builder.Services.AddScoped<ILocalizationService, LocalizationService>();
builder.Services.AddScoped<IViewLocalizerHelper, ViewLocalizerHelper>();

builder.Services.AddScoped<IBreadcrumbService, BreadcrumbService>();

// Middlewares
//builder.Services.AddScoped<SecurityHeadersMiddleware>();

//builder.Services.AddScoped<UrlNormalizationMiddleware>();
//builder.Services.AddScoped<MaintenanceMiddleware>();
//builder.Services.AddScoped<LangResolverMiddleware>();

var app = builder.Build();
app.UseRateLimiter();

if (!app.Environment.IsDevelopment())
{
    app.UseHsts();                        // 30 gün default
    app.UseHttpsRedirection();
}

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error/Server");
}

app.UseStatusCodePagesWithReExecute("/Error/Status/{0}");

app.Use(async (ctx, next) => {
    ctx.Response.Headers["Cross-Origin-Opener-Policy"] = "same-origin";
    ctx.Response.Headers["Cross-Origin-Resource-Policy"] = "same-origin";
    await next();
});

app.UseForwardedHeaders(new ForwardedHeadersOptions
{
    ForwardedHeaders = ForwardedHeaders.XForwardedProto | ForwardedHeaders.XForwardedFor
});





// Teknik ayarlara göre compression vs (demo: compression zaten aktif)
app.UseResponseCompression();
app.UseStaticFiles(new StaticFileOptions
{
    OnPrepareResponse = ctx =>
    {
        var path = ctx.File.PhysicalPath?.ToLowerInvariant() ?? "";
        // resimler, css, js → 30 gün
        if (path.EndsWith(".jpg") || path.EndsWith(".jpeg") || path.EndsWith(".png") || path.EndsWith(".webp")
            || path.EndsWith(".avif") || path.EndsWith(".css") || path.EndsWith(".js"))
        {
            ctx.Context.Response.Headers["Cache-Control"] = "public,max-age=2592000,immutable";
        }
    }
});

// ⬇️ 1) Statikler ÖNCE
//app.UseStaticFiles();

// SEO sırası (statik dosyadan önce normalize/maintenance/lang)
//app.UseMiddleware<SecurityHeadersMiddleware>();

app.UseMiddleware<UrlNormalizationMiddleware>();
app.UseMiddleware<MaintenanceMiddleware>();
app.UseMiddleware<LangResolverMiddleware>();


app.UseRouting();
app.UseOutputCache();

//app.MapControllerRoute(
//    name: "pages",
//    pattern: "{lang}/{slug?}",
//    defaults: new { controller = "Pages", action = "Index", slug = "home" });

app.MapControllerRoute(
    name: "default",
    pattern: "{lang=tr}/{controller=Home}/{action=Index}/{id?}"
);

app.Run();
