using HotelMultiTenant.Multitenancy;
using HotelMultiTenant.Services;

var builder = WebApplication.CreateBuilder(args);

// Output cache (host bazlý vary)
builder.Services.AddOutputCache(o =>
{
    o.AddPolicy("PerHost", b => b.SetVaryByHost(true)
                                 .Expire(TimeSpan.FromSeconds(300)));
});

builder.Services.AddHttpContextAccessor();
builder.Services.AddSingleton<IApiClient, ApiClient>();

// Tenant & Content servisleri (mock/in-memory)
builder.Services.AddSingleton<ITenantDirectory, InMemoryTenantDirectory>();
builder.Services.AddSingleton<IContentService, InMemoryContentService>();

builder.Services.AddHttpClient("api", c =>
{
    c.BaseAddress = new Uri("https://localhost:7248");
    c.Timeout = TimeSpan.FromSeconds(30);
});


// Multitenancy bileþenleri
builder.Services.AddTransient<TenantMiddleware>();
builder.Services.AddControllersWithViews()
    .AddRazorOptions(o => o.ViewLocationExpanders.Add(new ThemeViewLocationExpander()));



var app = builder.Build();

app.UseMiddleware<TenantMiddleware>();

// Basit canonical / bakým modu (tenant ayarlarýndan)
app.Use(async (ctx, next) =>
{
    var t = ctx.GetTenant()?.Current;
    if (t != null)
    {
        if (t.Settings.MaintenanceMode)
        {
            ctx.Response.StatusCode = 503;
            await ctx.Response.WriteAsync("Bakým Modu Aktif");
            return;
        }
        var host = ctx.Request.Host.Host;
        if (!string.IsNullOrWhiteSpace(t.Settings.CanonicalHost) &&
            !host.Equals(t.Settings.CanonicalHost, StringComparison.OrdinalIgnoreCase))
        {
            var target = $"{ctx.Request.Scheme}://{t.Settings.CanonicalHost}{ctx.Request.Path}{ctx.Request.QueryString}";
            ctx.Response.Redirect(target, permanent: true);
            return;
        }
    }
    await next();
});

app.UseStaticFiles();
app.UseRouting();
app.UseOutputCache();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

await app.RunAsync();
