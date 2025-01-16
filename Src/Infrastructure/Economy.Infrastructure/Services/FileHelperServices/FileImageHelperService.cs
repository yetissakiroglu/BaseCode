using Economy.Application.Infrastructure.ConfigurationModels;
using Economy.Application.Infrastructure.Services;
using Economy.Core.Extensions;
using Economy.Infrastructure.Services.FileHelperServices.Models;
using Economy.Persistence.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System.Net;

namespace Economy.Infrastructure.Services.FileHelperServices
{
    public class FileImageHelperService : IFileImageHelperService
    {
        private readonly FileUploadConfiguration _fileUploadSettings;

        public FileImageHelperService(IOptions<FileUploadConfiguration> options)
        {
            _fileUploadSettings = options.Value; // Tüm ayarları al
        }

        public async Task<ServiceResult<UploadFile>> UploadFile(IFormFile file, List<string> folderPaths)
        {
            var uploadResults = new UploadFile();

            // Dosya veya klasör yolu kontrolü
            if (file == null || folderPaths == null || folderPaths.Count == 0)
            {
                return ServiceResult<UploadFile>.Fail("No files or paths provided.",HttpStatusCode.NotFound);
            }

            // Boş dosya kontrolü
            if (file.Length == 0)
            {
                uploadResults = new UploadFile
                {
                    MediaName = string.Empty,
                };
            }
            else
            {
                long maxAllowedSize;
                long minAllowedSize;


                maxAllowedSize = _fileUploadSettings.Image.MaxUploadSizeMB * 1024 * 1024; // MB'den byte'a çevir
                minAllowedSize = _fileUploadSettings.Image.MinUploadSizeMB * 1024 * 1024; // MB'den byte'a çevir


                // Boyut kontrolü
                if (file.Length < minAllowedSize)
                {
                    uploadResults = new UploadFile
                    {
                        MediaName = string.Empty
                    };
                    return ServiceResult<UploadFile>.Fail($"File is too small. Minimum size is {minAllowedSize / (1024 * 1024)} MB.");
                }
                else if (file.Length > maxAllowedSize)
                {
                    uploadResults = new UploadFile
                    {
                        MediaName = string.Empty
                    };
                    return ServiceResult<UploadFile>.Fail($"File is too large. Maximum size is {maxAllowedSize / (1024 * 1024)} MB.");
                }

                var result = await UploadFileEx(file, folderPaths);
                uploadResults = result;
            }

            return ServiceResult<UploadFile>.Success(uploadResults, HttpStatusCode.OK);
        }

        public async Task<ServiceResult<List<UploadFile>>> UploadFiles(List<IFormFile> files, List<string> folderPaths)
        {
            var uploadResults = new List<UploadFile>();

            // Dosya veya klasör yolu kontrolü
            if (files == null || files.Count == 0 || folderPaths == null || folderPaths.Count == 0)
            {
                return ServiceResult<List<UploadFile>>.Fail("No files or paths provided.");
            }

            foreach (var file in files)
            {
                // Boş dosya kontrolü
                if (file == null || file.Length == 0)
                {
                    uploadResults.Add(new UploadFile
                    {
                        MediaName = string.Empty,
                    });
                }
                else
                {
                    long maxAllowedSize;
                    long minAllowedSize;


                    maxAllowedSize = _fileUploadSettings.Image.MaxUploadSizeMB * 1024 * 1024; // MB'den byte'a çevir
                    minAllowedSize = _fileUploadSettings.Image.MinUploadSizeMB * 1024 * 1024; // MB'den byte'a çevir

                    // Boyut kontrolü
                    if (file.Length < minAllowedSize)
                    {
                        uploadResults.Add(new UploadFile
                        {
                            MediaName = string.Empty
                        });
                        continue; // Bu dosyayı atla
                    }
                    else if (file.Length > maxAllowedSize)
                    {
                        uploadResults.Add(new UploadFile
                        {
                            MediaName = string.Empty
                        });
                        continue; // Bu dosyayı atla
                    }

                    var result = await UploadFileEx(file, folderPaths);
                    uploadResults.Add(result);
                }
            }

            return ServiceResult<List<UploadFile>>.Success(uploadResults, HttpStatusCode.OK);
        }

        private async Task<UploadFile> UploadFileEx(IFormFile file, List<string> folderPaths)
        {
            var combinedFolderPath = Path.Combine(folderPaths.ToArray());
            var folder = Path.Combine("Files", combinedFolderPath);
            var fullPath = Path.Combine(Directory.GetCurrentDirectory(), folder);

            // Klasör var mı kontrolü
            if (!Directory.Exists(fullPath))
            {
                Directory.CreateDirectory(fullPath);
            }

            var fileName = Path.GetFileNameWithoutExtension(file.FileName);
            var fileExtension = Path.GetExtension(file.FileName);

            var randomString = Path.GetRandomFileName().Replace(".", "");
            var randomFileName = $"{fileName.SanitizeString()}-{randomString}-{DateTime.Now.Ticks}{fileExtension}";

            var filePath = Path.Combine(folder, randomFileName);
            var fullFilePath = Path.Combine(fullPath, randomFileName);

            // Dosyayı kaydetme
            using (var stream = new FileStream(fullFilePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            var fileInfo = new FileInfo(fullFilePath);

            byte[] fileBytes;
            using (var memoryStream = new MemoryStream())
            {
                await file.CopyToAsync(memoryStream);
                fileBytes = memoryStream.ToArray();
            }

            var uploadModel = new UploadFile
            {
                MediaURL = randomFileName.ToLower(),
                MediaName = fileName,
                MediaFullURL = "/" + filePath.ToLower().Replace("\\", "/"),
                CombinedFolderPath = folder.ToLower().Replace("\\", "/"),
                FileSize = fileInfo.Length,
                IsByteArray = true,
                ByteArrayMedia = fileBytes,
                AltAttribute = fileName,
                TitleAttribute = fileName,
                MimeType = file.ContentType,
                Guid = Guid.NewGuid().ToString()
            };

            return uploadModel;
        }
    }
}
