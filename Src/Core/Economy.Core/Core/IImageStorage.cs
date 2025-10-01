using Microsoft.AspNetCore.Http;

namespace Economy.Core.Core
{
    public interface IImageStorage
    {
        Task<IReadOnlyList<ImageItemDto>> ListAsync(string relativeDir, CancellationToken ct = default);
        Task<IReadOnlyList<string>> ListDirsAsync(string relativeDir, CancellationToken ct = default);

        // Dönüş: kaydedilen dosyanın relative path'i (örn: "2025/10/foo.jpg")
        Task<string> UploadAsync(string relativeDir, IFormFile file, string? aspect = null, CancellationToken ct = default);
        Task DeleteAsync(string relativePath, CancellationToken ct = default);

        string ToPublicUrl(string relativePath);
        string? ToPublicWebpIfExists(string relativePath);
    }
}
