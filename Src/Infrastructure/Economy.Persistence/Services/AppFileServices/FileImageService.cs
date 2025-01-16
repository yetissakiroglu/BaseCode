using Economy.Application.Services.FileServices;
using Economy.Domain.Entites.EntityAppFiles;
using Economy.Persistence.Contexts;
using Microsoft.AspNetCore.Http;

namespace Economy.Persistence.Services.AppFileServices
{
    public class FileImageService : IFileImageService
    {
        private readonly AppDbContext _context;

        public FileImageService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Guid> UploadImageAsync(IFormFile? file)
        {
            if (file == null || file.Length == 0)
            {
                throw new ArgumentException("Invalid file");
            }

            if (!IsImageFile(file))
            {
                throw new ArgumentException("Invalid file type. Only image files are allowed.");
            }

            if (file.Length > 5 * 1024 * 1024) // 5 MB in bytes
            {
                throw new ArgumentException("File size exceeds the maximum allowed size of 5 MB.");
            }

            var image = new AppFileImage
            {
                Id = Guid.NewGuid(),
                OriginalFileName = file.FileName
            };

            using (var memoryStream = new MemoryStream())
            {
                await file.CopyToAsync(memoryStream);
                image.ImageData = memoryStream.ToArray();
            }

            _context.AppFileImages.Add(image);
            await _context.SaveChangesAsync();

            return image.Id;
        }

        public async Task<Guid> UpdateImageAsync(Guid? imageId, IFormFile? newFile)
        {
            if (imageId == null)
            {
                throw new ArgumentException("Invalid imageId");
            }

            if (newFile == null || newFile.Length == 0)
            {
                throw new ArgumentException("Invalid file");
            }

            if (!IsImageFile(newFile))
            {
                throw new ArgumentException("Invalid file type. Only image files are allowed.");
            }

            if (newFile.Length > 5 * 1024 * 1024) // 5 MB in bytes
            {
                throw new ArgumentException("File size exceeds the maximum allowed size of 5 MB.");
            }

            var existingImage = await _context.AppFileImages.FindAsync(imageId);

            if (existingImage == null)
            {
                throw new ArgumentException($"Image with ID {imageId} not found");
            }

            // Update the existing image with the new data
            existingImage.OriginalFileName = newFile.FileName;

            using (var memoryStream = new MemoryStream())
            {
                await newFile.CopyToAsync(memoryStream);
                existingImage.ImageData = memoryStream.ToArray();
            }

            await _context.SaveChangesAsync();

            return existingImage.Id;
        }


        public async Task<AppFileImage> GetImageAsync(Guid? id)
        {
            if (id == null)
            {
                id = new Guid(Guid.Empty.ToString());
            }

            var fileImage = await _context.AppFileImages.FindAsync(id);

            if (fileImage == null)
            {
                var defaultImagePath = Path.Combine("wwwroot", "admin", "assets", "img", "generic", "noimage.png");

                fileImage = new AppFileImage
                {
                    Id = Guid.Empty,
                    OriginalFileName = "NoImage.png",
                    ImageData = File.ReadAllBytes(defaultImagePath)
                };
            }

            return fileImage;
        }
        public async Task<string> GetImageUrlFromImageIdAsync(string? id)
        {
            if (string.IsNullOrEmpty(id))
            {
                id = Guid.Empty.ToString();
            }
            var image = await GetImageAsync(new Guid(id));
            var url = $"data:image/png;base64,{Convert.ToBase64String(image.ImageData)}";
            return url;
        }

        private bool IsImageFile(IFormFile file)
        {
            var allowedImageTypes = new[] { "image/jpeg", "image/jpg", "image/png", "image/gif", "image/bmp", "image/webp" };
            return allowedImageTypes.Contains(file.ContentType);
        }
    }

}
