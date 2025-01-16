using Economy.Domain.Entites.EntityAppFiles;
using Microsoft.AspNetCore.Http;

namespace Economy.Application.Services.FileServices
{
    public interface IFileImageService
    {
        Task<Guid> UploadImageAsync(IFormFile? file);
        Task<Guid> UpdateImageAsync(Guid? imageId, IFormFile? newFile);
        Task<AppFileImage> GetImageAsync(Guid? id);
        Task<string> GetImageUrlFromImageIdAsync(string? id);
    }
}
