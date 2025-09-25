// Program.cs (API YOK, in-memory servisler)
using Economy.Web.UI.Infrastructure;
using Economy.Web.UI.Services.Abstractions;
using Economy.Web.UI.Services.Implementations;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.Localization;
using System.Globalization;
using System.Threading.RateLimiting;

// --- builder
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddMemoryCache();
builder.Services.AddHttpContextAccessor();

builder.Services.AddControllersWithViews()
    .AddViewLocalization()
    .AddDataAnnotationsLocalization();

// In-memory servis kayıtları (API yerine fake servisler)
builder.Services.AddSingleton<ILanguageService, InMemoryLanguageService>();
builder.Services.AddSingleton<ILocalizationStore, InMemoryLocalizationStore>();
builder.Services.AddSingleton<IStringLocalizerFactory, ServiceStringLocalizerFactory>();
builder.Services.AddSingleton<ISlugService, InMemorySlugService>();
builder.Services.AddSingleton<IContentService, InMemoryContentService>();
builder.Services.AddSingleton<ICommentsService, InMemoryCommentsService>();
builder.Services.AddSingleton<IReservationService, InMemoryReservationService>();
builder.Services.AddSingleton<IRecaptchaService, InMemoryRecaptchaService>();

// Blog / SSS / Galeri (eklediysen)
builder.Services.AddSingleton<IBlogService, InMemoryBlogService>();
builder.Services.AddSingleton<IFaqService, InMemoryFaqService>();
builder.Services.AddSingleton<IGalleryService, InMemoryGalleryService>();

// 🔧 IStringLocalizer (generic olmayan) alias kaydı — view'lerde @inject IStringLocalizer için
builder.Services.AddSingleton(typeof(IStringLocalizer<>), typeof(StringLocalizer<>));
builder.Services.AddScoped<IStringLocalizer>(sp =>
{
    var factory = sp.GetRequiredService<IStringLocalizerFactory>();
    // Varsayılan namespace'i "Shared" tutuyoruz; ServiceStringLocalizerFactory kültürü otomatik çözecek
    return factory.Create("Shared", location: null);
});

// Altyapı
builder.Services.AddSingleton<LanguageCache>();
builder.Services.AddSingleton<ShortCultureConstraint>();
builder.Services.AddHostedService<LocalizationWarmupService>();

// kısa dil kodu route constraint
builder.Services.Configure<RouteOptions>(o =>
{
    o.ConstraintMap["shortculture"] = typeof(ShortCultureConstraint);
});

// (opsiyonel) Rate limit – .NET 7/8
builder.Services.AddRateLimiter(opt =>
{
    opt.AddPolicy("forms", _ =>
        RateLimitPartition.GetFixedWindowLimiter("forms", _ => new FixedWindowRateLimiterOptions
        {
            PermitLimit = 10,
            Window = TimeSpan.FromMinutes(1),
            QueueLimit = 2,
            QueueProcessingOrder = QueueProcessingOrder.OldestFirst
        }));
    opt.AddPolicy("comments", _ =>
        RateLimitPartition.GetFixedWindowLimiter("comments", _ => new FixedWindowRateLimiterOptions
        {
            PermitLimit = 5,
            Window = TimeSpan.FromMinutes(1),
            QueueLimit = 2,
            QueueProcessingOrder = QueueProcessingOrder.OldestFirst
        }));
});

var app = builder.Build();

// Proxy arkasındaysan
app.UseForwardedHeaders(new ForwardedHeadersOptions
{
    ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
});

// === RequestLocalization ===
using (var scope = app.Services.CreateScope())
{
    var langSvc = scope.ServiceProvider.GetRequiredService<ILanguageService>();
    var langs = await langSvc.GetAsync();
    var def = await langSvc.GetDefaultAsync();
    var cultures = langs.Select(l => new CultureInfo(l.Code)).ToList();

    var locOptions = new RequestLocalizationOptions
    {
        DefaultRequestCulture = new RequestCulture(def.Code),
        SupportedCultures = cultures,
        SupportedUICultures = cultures
    };
    // URL’de kısa dil kodu (/tr, /en)
    locOptions.RequestCultureProviders.Insert(0, new DynamicRouteRequestCultureProvider());

    app.UseRequestLocalization(locOptions);
}

// static dosyalar + CDN cache
app.UseStaticFiles();

// routing / ratelimit / auth
app.UseRouting();
app.UseRateLimiter();
app.UseAuthorization();

// Basit ETag demo (HTML’de 60 sn cache)
// Güvenli ETag/Cache-Control ekleme (HTML için)
// === HTML için güvenli ETag / Cache-Control (60 sn) ===
app.Use(async (ctx, next) =>
{
    // Yalnızca GET/HEAD için ve tarayıcı sayfa isteklerinde (AJAX vs. de olabilir ama Content-Type filtreleyeceğiz)
    if (!HttpMethods.IsGet(ctx.Request.Method) && !HttpMethods.IsHead(ctx.Request.Method))
    {
        await next();
        return;
    }

    // Statik dosyaları framework zaten cache'ler; uzantıya göre kaba filtre
    var path = ctx.Request.Path.Value ?? "";
    if (path.EndsWith(".js", StringComparison.OrdinalIgnoreCase) ||
        path.EndsWith(".css", StringComparison.OrdinalIgnoreCase) ||
        path.EndsWith(".png", StringComparison.OrdinalIgnoreCase) ||
        path.EndsWith(".jpg", StringComparison.OrdinalIgnoreCase) ||
        path.EndsWith(".jpeg", StringComparison.OrdinalIgnoreCase) ||
        path.EndsWith(".webp", StringComparison.OrdinalIgnoreCase) ||
        path.EndsWith(".gif", StringComparison.OrdinalIgnoreCase) ||
        path.EndsWith(".svg", StringComparison.OrdinalIgnoreCase))
    {
        await next();
        return;
    }

    // Dakikada bir değişen zayıf ETag üret (yol + kültür + dakika)
    var culture = System.Threading.Thread.CurrentThread.CurrentUICulture.Name;
    var etag = $"W/\"html:{culture}:{path}:{DateTime.UtcNow:yyyyMMddHHmm}\"";

    // 1) Eğer istemci aynı ETag'i gönderiyorsa, hiç body yazmadan 304 döndür
    var inm = ctx.Request.Headers.IfNoneMatch.ToString();
    if (!string.IsNullOrEmpty(inm) && inm.Split(',').Any(t => string.Equals(t.Trim(), etag, StringComparison.Ordinal)))
    {
        if (!ctx.Response.HasStarted)
        {
            ctx.Response.StatusCode = StatusCodes.Status304NotModified;
            ctx.Response.Headers["ETag"] = etag;
            ctx.Response.Headers["Cache-Control"] = "public, max-age=60";
            // HTML değilse de sorun yok; 304'te gövde yok.
            return;
        }
    }

    // 2) Aksi halde response başlamadan hemen önce header ekle
    ctx.Response.OnStarting(() =>
    {
        // Başlamadıysa ve başarıysa ve içerik HTML ise ekle
        if (!ctx.Response.HasStarted &&
            ctx.Response.StatusCode == StatusCodes.Status200OK &&
            (ctx.Response.ContentType?.Contains("text/html", StringComparison.OrdinalIgnoreCase) ?? false))
        {
            if (!ctx.Response.Headers.ContainsKey("ETag"))
                ctx.Response.Headers["ETag"] = etag;

            if (!ctx.Response.Headers.ContainsKey("Cache-Control"))
                ctx.Response.Headers["Cache-Control"] = "public, max-age=60";

            // Tarayıcıya varyasyonu belirt (opsiyonel ama iyi pratik)
            if (!ctx.Response.Headers.ContainsKey("Vary"))
                ctx.Response.Headers["Vary"] = "Accept-Encoding";
        }
        return Task.CompletedTask;
    });

    await next();
});


// kısa dil kodu ile default rota
app.MapControllerRoute(
    name: "localized",
    pattern: "{culture:shortculture?}/{controller=Home}/{action=Index}/{id?}");

app.Run();
