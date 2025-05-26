using Economy.Core.Extensions;
using Economy.Core.Helpers.Dtos;
using Economy.Core.Tools.Result;
using Microsoft.Extensions.Options;

namespace Economy.Core.Helpers
{
    public class FileImageHelperService : IFileImageHelperService
    {
        private readonly FileUploadConfiguration _fileUploadSettings;

        public FileImageHelperService(IOptions<FileUploadConfiguration> options)
        {
            _fileUploadSettings = options.Value; // Tüm ayarları al
        }

        public ServiceResult<UploadFile> UploadBase64(string? base64String, List<string> folderPaths)
        {
            if (string.IsNullOrWhiteSpace(base64String))
                return ServiceResult<UploadFile>.Failure("Base64 verisi boş olamaz.");

            try
            {
                var base64Parts = base64String.Split(',');
                string base64Data = base64Parts.Length > 1 ? base64Parts[1] : base64Parts[0];

                string mimeType = base64Parts.Length > 1 && base64Parts[0].StartsWith("data:")
                    ? base64Parts[0].Split(';')[0].Replace("data:", "")
                    : "application/octet-stream";

                byte[] fileBytes = Convert.FromBase64String(base64Data);

                // 🧠 Dosya boyutu kontrolü
                long maxAllowedSize = _fileUploadSettings.Image.MaxUploadSizeMB * 1024 * 1024;
                long minAllowedSize = _fileUploadSettings.Image.MinUploadSizeMB * 1024 * 1024;

                if (fileBytes.Length < minAllowedSize)
                {
                    return ServiceResult<UploadFile>.Failure($"Dosya çok küçük. Minimum izin verilen boyut: {minAllowedSize / (1024 * 1024)} MB.");
                }
                else if (fileBytes.Length > maxAllowedSize)
                {
                    return ServiceResult<UploadFile>.Failure($"Dosya çok büyük. Maksimum izin verilen boyut: {maxAllowedSize / (1024 * 1024)} MB.");
                }

                // 🗂 Klasör oluşturma
                string combinedFolderPath = Path.Combine(folderPaths.ToArray());
                string relativeFolderPath = Path.Combine("Files", combinedFolderPath);
                string fullFolderPath = Path.Combine(Directory.GetCurrentDirectory(), relativeFolderPath);

                if (!Directory.Exists(fullFolderPath))
                    Directory.CreateDirectory(fullFolderPath);

                // 📛 Dosya adı
                string fileExtension = GetFileExtensionFromMimeType(mimeType);
                string fileName = "uploaded-file";
                string randomName = Path.GetRandomFileName().Replace(".", "");
                string finalFileName = $"{fileName.SanitizeString()}-{randomName}-{DateTime.Now.Ticks}{fileExtension}";

                string relativeFilePath = Path.Combine(relativeFolderPath, finalFileName);
                string absoluteFilePath = Path.Combine(fullFolderPath, finalFileName);

                // 💾 Dosyayı diske yaz
                File.WriteAllBytes(absoluteFilePath, fileBytes);
                var fileInfo = new FileInfo(absoluteFilePath);

                var uploadModel = new UploadFile
                {
                    MediaURL = finalFileName.ToLower(),
                    MediaName = fileName,
                    MediaFullURL = "/" + relativeFilePath.Replace("\\", "/").ToLower(),
                    CombinedFolderPath = relativeFolderPath.Replace("\\", "/").ToLower(),
                    FileSize = fileInfo.Length,
                    IsByteArray = true,
                    ByteArrayMedia = fileBytes,
                    AltAttribute = fileName,
                    TitleAttribute = fileName,
                    MimeType = mimeType,
                    Guid = Guid.NewGuid().ToString()
                };

                return ServiceResult<UploadFile>.Success(uploadModel, "Dosya başarıyla yüklendi.");
            }
            catch (FormatException)
            {
                return ServiceResult<UploadFile>.Failure("Geçersiz Base64 formatı.");
            }
            catch (Exception ex)
            {
                return ServiceResult<UploadFile>.Failure("Dosya yüklenirken hata oluştu.", new[] { ex.Message });
            }
        }




        //public async Task<ResponseModel<UploadFile>> UploadBase64(string? base64String, List<string> folderPaths)
        //{
        //    var response = new ResponseModel<UploadFile>();

        //    if (string.IsNullOrWhiteSpace(base64String))
        //    {
        //        response.IsSuccess = false;
        //        //response.Message = "Base64 string is null or empty.";
        //        return response;
        //    }

        //    try
        //    {
        //        // base64'ü ayrıştır
        //        var base64Parts = base64String.Split(',');

        //        string base64Data = base64Parts.Length > 1 ? base64Parts[1] : base64Parts[0];
        //        string mimeType = base64Parts.Length > 1 && base64Parts[0].Contains("data:")
        //            ? base64Parts[0].Split(';')[0].Replace("data:", "")
        //            : "application/octet-stream";

        //        byte[] fileBytes = Convert.FromBase64String(base64Data);

        //        // Klasör oluştur
        //        var combinedFolderPath = Path.Combine(folderPaths.ToArray());
        //        var folder = Path.Combine("Files", combinedFolderPath);
        //        var fullPath = Path.Combine(Directory.GetCurrentDirectory(), folder);

        //        if (!Directory.Exists(fullPath))
        //        {
        //            Directory.CreateDirectory(fullPath);
        //        }

        //        // Dosya adı oluştur
        //        var fileExtension = GetFileExtensionFromMimeType(mimeType);
        //        var fileName = "uploaded-file"; // istersen farklı isimlendirme stratejisi uygulanabilir
        //        var randomString = Path.GetRandomFileName().Replace(".", "");
        //        var randomFileName = $"{fileName.SanitizeString()}-{randomString}-{DateTime.Now.Ticks}{fileExtension}";

        //        var filePath = Path.Combine(folder, randomFileName);
        //        var fullFilePath = Path.Combine(fullPath, randomFileName);

        //        // Dosyayı diske yaz
        //        await File.WriteAllBytesAsync(fullFilePath, fileBytes);

        //        var fileInfo = new FileInfo(fullFilePath);

        //        var uploadModel = new UploadFile
        //        {
        //            MediaURL = randomFileName.ToLower(),
        //            MediaName = fileName,
        //            MediaFullURL = "/" + filePath.ToLower().Replace("\\", "/"),
        //            CombinedFolderPath = folder.ToLower().Replace("\\", "/"),
        //            FileSize = fileInfo.Length,
        //            IsByteArray = true,
        //            ByteArrayMedia = fileBytes,
        //            AltAttribute = fileName,
        //            TitleAttribute = fileName,
        //            MimeType = mimeType,
        //            Guid = Guid.NewGuid().ToString()
        //        };

        //        response.Data = uploadModel;
        //        response.IsSuccess = true;
        //        //response.Message = "Dosya başarıyla yüklendi.";
        //    }
        //    catch (Exception ex)
        //    {
        //        response.IsSuccess = false;
        //        //response.Message = "Dosya yüklenirken hata oluştu: " + ex.Message;
        //    }

        //    return response;
        //}

        //public async Task<ResponseModel<UploadFile>> UploadFile(IFormFile file, List<string> folderPaths)
        //{
        //    var uploadResults = new UploadFile();

        //    // Dosya veya klasör yolu kontrolü
        //    if (file == null || folderPaths == null || folderPaths.Count == 0)
        //    {
        //        return ResponseModel<UploadFile>.Fail("No files or paths provided.", HttpStatusCode.NotFound);
        //    }

        //    // Boş dosya kontrolü
        //    if (file.Length == 0)
        //    {
        //        uploadResults = new UploadFile
        //        {
        //            MediaName = string.Empty,
        //        };
        //    }
        //    else
        //    {
        //        long maxAllowedSize;
        //        long minAllowedSize;


        //        maxAllowedSize = _fileUploadSettings.Image.MaxUploadSizeMB * 1024 * 1024; // MB'den byte'a çevir
        //        minAllowedSize = _fileUploadSettings.Image.MinUploadSizeMB * 1024 * 1024; // MB'den byte'a çevir


        //        // Boyut kontrolü
        //        if (file.Length < minAllowedSize)
        //        {
        //            uploadResults = new UploadFile
        //            {
        //                MediaName = string.Empty
        //            };
        //            return ResponseModel<UploadFile>.Fail($"File is too small. Minimum size is {minAllowedSize / (1024 * 1024)} MB.");
        //        }
        //        else if (file.Length > maxAllowedSize)
        //        {
        //            uploadResults = new UploadFile
        //            {
        //                MediaName = string.Empty
        //            };
        //            return ResponseModel<UploadFile>.Fail($"File is too large. Maximum size is {maxAllowedSize / (1024 * 1024)} MB.");
        //        }

        //        var result = await UploadFileEx(file, folderPaths);
        //        uploadResults = result;
        //    }

        //    return ResponseModel<UploadFile>.Success(uploadResults, HttpStatusCode.OK);
        //}

        //public async Task<ResponseModel<List<UploadFile>>> UploadFiles(List<IFormFile> files, List<string> folderPaths)
        //{
        //    var uploadResults = new List<UploadFile>();

        //    // Dosya veya klasör yolu kontrolü
        //    if (files == null || files.Count == 0 || folderPaths == null || folderPaths.Count == 0)
        //    {
        //        return ResponseModel<List<UploadFile>>.Fail("No files or paths provided.");
        //    }

        //    foreach (var file in files)
        //    {
        //        // Boş dosya kontrolü
        //        if (file == null || file.Length == 0)
        //        {
        //            uploadResults.Add(new UploadFile
        //            {
        //                MediaName = string.Empty,
        //            });
        //        }
        //        else
        //        {
        //            long maxAllowedSize;
        //            long minAllowedSize;


        //            maxAllowedSize = _fileUploadSettings.Image.MaxUploadSizeMB * 1024 * 1024; // MB'den byte'a çevir
        //            minAllowedSize = _fileUploadSettings.Image.MinUploadSizeMB * 1024 * 1024; // MB'den byte'a çevir

        //            // Boyut kontrolü
        //            if (file.Length < minAllowedSize)
        //            {
        //                uploadResults.Add(new UploadFile
        //                {
        //                    MediaName = string.Empty
        //                });
        //                continue; // Bu dosyayı atla
        //            }
        //            else if (file.Length > maxAllowedSize)
        //            {
        //                uploadResults.Add(new UploadFile
        //                {
        //                    MediaName = string.Empty
        //                });
        //                continue; // Bu dosyayı atla
        //            }

        //            var result = await UploadFileEx(file, folderPaths);
        //            uploadResults.Add(result);
        //        }
        //    }

        //    return ResponseModel<List<UploadFile>>.Success(uploadResults, HttpStatusCode.OK);
        //}

        //private async Task<UploadFile> UploadFileEx(IFormFile file, List<string> folderPaths)
        //{
        //    var combinedFolderPath = Path.Combine(folderPaths.ToArray());
        //    var folder = Path.Combine("Files", combinedFolderPath);
        //    var fullPath = Path.Combine(Directory.GetCurrentDirectory(), folder);

        //    // Klasör var mı kontrolü
        //    if (!Directory.Exists(fullPath))
        //    {
        //        Directory.CreateDirectory(fullPath);
        //    }

        //    var fileName = Path.GetFileNameWithoutExtension(file.FileName);
        //    var fileExtension = Path.GetExtension(file.FileName);

        //    var randomString = Path.GetRandomFileName().Replace(".", "");
        //    var randomFileName = $"{fileName.SanitizeString()}-{randomString}-{DateTime.Now.Ticks}{fileExtension}";

        //    var filePath = Path.Combine(folder, randomFileName);
        //    var fullFilePath = Path.Combine(fullPath, randomFileName);

        //    // Dosyayı kaydetme
        //    using (var stream = new FileStream(fullFilePath, FileMode.Create))
        //    {
        //        await file.CopyToAsync(stream);
        //    }

        //    var fileInfo = new FileInfo(fullFilePath);

        //    byte[] fileBytes;
        //    using (var memoryStream = new MemoryStream())
        //    {
        //        await file.CopyToAsync(memoryStream);
        //        fileBytes = memoryStream.ToArray();
        //    }

        //    var uploadModel = new UploadFile
        //    {
        //        MediaURL = randomFileName.ToLower(),
        //        MediaName = fileName,
        //        MediaFullURL = "/" + filePath.ToLower().Replace("\\", "/"),
        //        CombinedFolderPath = folder.ToLower().Replace("\\", "/"),
        //        FileSize = fileInfo.Length,
        //        IsByteArray = true,
        //        ByteArrayMedia = fileBytes,
        //        AltAttribute = fileName,
        //        TitleAttribute = fileName,
        //        MimeType = file.ContentType,
        //        Guid = Guid.NewGuid().ToString()
        //    };

        //    return uploadModel;
        //}

        private string GetFileExtensionFromMimeType(string mimeType)
        {
            return mimeType switch
            {
                "image/jpeg" => ".jpg",
                "image/png" => ".png",
                "image/gif" => ".gif",
                "application/pdf" => ".pdf",
                "text/plain" => ".txt",
                "application/zip" => ".zip",
                _ => ".bin" // bilinmeyen dosya tipi
            };
        }

       
    }


}
