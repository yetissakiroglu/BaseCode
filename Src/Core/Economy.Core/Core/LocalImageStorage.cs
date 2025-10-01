using Economy.Core.Options;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Webp;
using SixLabors.ImageSharp.Processing;
using System.Text.RegularExpressions;

namespace Economy.Core.Core
{

    public sealed class LocalImageStorage : IImageStorage
    {
        private readonly FileManagerOptions _opt;
        private readonly string _rootFullPath;
        private static readonly Regex SafeNameRegex = new("^[\\w\\-. ]+$", RegexOptions.Compiled);

        public LocalImageStorage(IOptions<FileManagerOptions> options)
        {
            _opt = options.Value;
            _rootFullPath = Path.GetFullPath(_opt.RootPath);
            Directory.CreateDirectory(_rootFullPath);
        }

        public string ToPublicUrl(string relativePath)
            => $"{_opt.PublicRequestPath}/{relativePath.Replace('\\', '/').TrimStart('/')}";

        public string? ToPublicWebpIfExists(string relativePath)
        {
            var webpRel = Path.ChangeExtension(relativePath, ".webp")!;
            var full = CombineUnderRoot(webpRel);
            return File.Exists(full) ? ToPublicUrl(webpRel) : null;
        }

        private string Normalize(string? rel)
            => (rel ?? string.Empty).Replace('\\', '/').Trim('/');

        private string CombineUnderRoot(string relative)
        {
            var full = Path.GetFullPath(Path.Combine(_rootFullPath, relative));
            if (!full.StartsWith(_rootFullPath, StringComparison.OrdinalIgnoreCase))
                throw new InvalidOperationException("Path traversal engellendi.");
            return full;
        }

        private static void EnsureSafeFileName(string name)
        {
            var withoutExt = Path.GetFileNameWithoutExtension(name);
            if (string.IsNullOrWhiteSpace(withoutExt) || !SafeNameRegex.IsMatch(withoutExt))
                throw new InvalidOperationException("Geçersiz dosya adı.");
        }

        public async Task<IReadOnlyList<ImageItemDto>> ListAsync(string relativeDir, CancellationToken ct = default)
        {
            relativeDir = Normalize(relativeDir);
            var dirFull = CombineUnderRoot(relativeDir);
            Directory.CreateDirectory(dirFull);

            return Directory.EnumerateFiles(dirFull)
                .Select(p => new FileInfo(p))
                .OrderByDescending(fi => fi.LastWriteTimeUtc)
                .Select(fi => new ImageItemDto(
                    fi.Name,
                    Path.Combine(relativeDir, fi.Name).Replace('\\', '/'),
                    fi.Length,
                    fi.LastWriteTimeUtc))
                .ToList();
        }

        public async Task<IReadOnlyList<string>> ListDirsAsync(string relativeDir, CancellationToken ct = default)
        {
            relativeDir = Normalize(relativeDir);
            var dirFull = CombineUnderRoot(relativeDir);
            Directory.CreateDirectory(dirFull);

            return Directory
                .EnumerateDirectories(dirFull)
                .Select(p => new DirectoryInfo(p).Name)
                .OrderBy(n => n, StringComparer.OrdinalIgnoreCase)
                .ToList();
        }

        private (int w, int h) ParseAspect(string? aspect)
        {
            if (string.IsNullOrWhiteSpace(aspect)) return (0, 0);
            aspect = aspect.Replace("x", ":", StringComparison.OrdinalIgnoreCase);
            var p = aspect.Split(':', StringSplitOptions.RemoveEmptyEntries);
            if (p.Length != 2) return (0, 0);
            if (int.TryParse(p[0], out var aw) && int.TryParse(p[1], out var ah) && aw > 0 && ah > 0)
                return (aw, ah);
            return (0, 0);
        }

        private static Rectangle CenterCropRect(int srcW, int srcH, int tw, int th)
        {
            var sr = (double)srcW / srcH;
            var tr = (double)tw / th;
            int cw, ch;
            if (sr > tr) { ch = srcH; cw = (int)Math.Round(ch * tr); }
            else { cw = srcW; ch = (int)Math.Round(cw / tr); }
            var x = (srcW - cw) / 2; var y = (srcH - ch) / 2;
            return new Rectangle(x, y, cw, ch);
        }

        private async Task ProcessAndSaveAsync(string fullPath, string? aspect, CancellationToken ct)
        {
            var proc = _opt.ImageProcessing;
            using var img = await Image.LoadAsync(fullPath, ct);

            var (aw, ah) = ParseAspect(aspect);
            if (aw > 0 && ah > 0)
            {
                var crop = CenterCropRect(img.Width, img.Height, aw, ah);
                img.Mutate(p => p.Crop(crop));
            }

            if (proc.MaxWidth > 0 && img.Width > proc.MaxWidth)
            {
                var newH = (int)Math.Round(img.Height * (proc.MaxWidth / (double)img.Width));
                img.Mutate(p => p.Resize(proc.MaxWidth, newH));
            }

            await img.SaveAsync(fullPath, ct);

            if (proc.CreateWebpCopy)
            {
                var webpPath = Path.ChangeExtension(fullPath, ".webp")!;
                var encoder = new WebpEncoder { Quality = proc.WebpQuality };
                await img.SaveAsync(webpPath, encoder, ct);
            }
        }

        public async Task<string> UploadAsync(string relativeDir, IFormFile file, string? aspect = null, CancellationToken ct = default)
        {
            if (file is null || file.Length == 0) throw new InvalidOperationException("Dosya boş.");

            var ext = Path.GetExtension(file.FileName).ToLowerInvariant();
            if (_opt.AllowedExtensions.Length > 0 && !_opt.AllowedExtensions.Contains(ext, StringComparer.OrdinalIgnoreCase))
                throw new InvalidOperationException("Sadece resim uzantılarına izin verilir.");

            if (file.ContentType is not null && !file.ContentType.StartsWith("image/", StringComparison.OrdinalIgnoreCase))
                throw new InvalidOperationException("Yüklenen dosya bir resim değil.");

            var maxBytes = _opt.MaxUploadSizeMB * 1024L * 1024L;
            if (file.Length > maxBytes) throw new InvalidOperationException($"Maksimum boyut: {_opt.MaxUploadSizeMB} MB.");

            var safeName = Path.GetFileName(file.FileName);
            EnsureSafeFileName(safeName);

            // Yıl/Ay klasörü
            var stamp = DateTime.UtcNow;
            relativeDir = Normalize(string.IsNullOrEmpty(relativeDir) ? $"{stamp:yyyy}/{stamp:MM}" : relativeDir);

            var destDir = CombineUnderRoot(relativeDir);
            Directory.CreateDirectory(destDir);

            var destRel = $"{relativeDir}/{safeName}".Replace('\\', '/');
            var destFull = CombineUnderRoot(destRel);

            using (var fs = new FileStream(destFull, FileMode.Create, FileAccess.Write, FileShare.None))
                await file.CopyToAsync(fs, ct);

            await ProcessAndSaveAsync(destFull, aspect, ct);

            return destRel;
        }

        // Basit delete
        public Task DeleteAsync(string relativePath, CancellationToken ct = default)
        {
            var norm = Normalize(relativePath);
            var full = CombineUnderRoot(norm);
            if (File.Exists(full)) File.Delete(full);

            // webp varsa onu da silebilirsin:
            var webp = Path.ChangeExtension(full, ".webp")!;
            if (File.Exists(webp)) File.Delete(webp);

            return Task.CompletedTask;
        }
    }
}
