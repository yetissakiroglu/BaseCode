using Economy.Application.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.FileProviders;
using System.Security.Cryptography;
using System.Text;

namespace Economy.Persistence.Services
{
   
    public sealed class CdnUrlService : ICdnUrlService
    {
        private readonly IPanelAppTechnicalSettingService _techService;
        private readonly IWebHostEnvironment _env;
        private readonly IHttpContextAccessor _http;

        public CdnUrlService(
            IPanelAppTechnicalSettingService techService,
            IWebHostEnvironment env,
            IHttpContextAccessor http)
        {
            _techService = techService;
            _env = env;
            _http = http;
        }

        public string Url(string path, bool appendVersion = false)
        {
            if (string.IsNullOrWhiteSpace(path)) return string.Empty;

            // Mutlak URL ya da data: şeması ise aynen dön
            if (path.StartsWith("http://", StringComparison.OrdinalIgnoreCase) ||
                path.StartsWith("https://", StringComparison.OrdinalIgnoreCase) ||
                path.StartsWith("data:", StringComparison.OrdinalIgnoreCase) ||
                path.StartsWith("//"))
            {
                return path;
            }

            // "~" => uygulama köküne çevir
            var contentPath = path.StartsWith("~/")
                ? path.Substring(1) // "~" i at => "/css/site.css"
                : (path.StartsWith("/") ? path : "/" + path);

            // Ayarları veritabanından oku (cache’siz)
            var res = _techService.GetAppTechnicalSetting(isDeleted: false);
            var tech = res.Data;

            var useCdn = tech is not null && tech.EnableCDN && !string.IsNullOrWhiteSpace(tech.StaticFileUrl);

            string baseUrl;
            if (useCdn)
            {
                baseUrl = tech!.StaticFileUrl!.TrimEnd('/');
            }
            else
            {
                // Mevcut host (http/https + host[:port])
                var req = _http.HttpContext?.Request;
                var scheme = req?.Scheme ?? "https";
                var host = req?.Host.ToString() ?? "localhost";
                baseUrl = $"{scheme}://{host}";
            }

            var url = $"{baseUrl}{contentPath}";

            if (appendVersion)
            {
                var ver = ComputeFileVersion(contentPath);
                if (!string.IsNullOrEmpty(ver))
                    url += (url.Contains('?') ? "&" : "?") + "v=" + ver;
            }

            return url;
        }

        // wwwroot altındaki fiziksel dosyanın son değişiklik zamanına göre basit versiyon
        private string? ComputeFileVersion(string webPath)
        {
            try
            {
                // "/css/site.css" -> "css/site.css"
                var relative = webPath.TrimStart('/');
                IFileInfo file = _env.WebRootFileProvider.GetFileInfo(relative);
                if (!file.Exists) return null;

                var ticks = file.LastModified.UtcDateTime.Ticks;
                // İstersen direkt ticks dönebilirsin:
                // return ticks.ToString();
                // Küçük bir hash (kısa olsun diye):
                using var sha1 = SHA1.Create();
                var bytes = sha1.ComputeHash(Encoding.UTF8.GetBytes(ticks.ToString()));
                return Convert.ToHexString(bytes[..6]); // 12 hex karakter
            }
            catch
            {
                return null;
            }
        }
    }

}
