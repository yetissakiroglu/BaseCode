using Economy.Core.Extensions;
using Economy.Core.Helpers.Dtos;
using Economy.Core.Tools.Result;
using Microsoft.Extensions.Options;

namespace Economy.Core.Helpers
{
    public class FileImageHelperService : IFileImageHelperService
    {

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

                //// 🧠 Dosya boyutu kontrolü
                //long maxAllowedSize = _fileUploadSettings.Image.MaxUploadSizeMB * 1024 * 1024;
                //long minAllowedSize = _fileUploadSettings.Image.MinUploadSizeMB * 1024 * 1024;

                //if (fileBytes.Length < minAllowedSize)
                //{
                //    return ServiceResult<UploadFile>.Failure($"Dosya çok küçük. Minimum izin verilen boyut: {minAllowedSize / (1024 * 1024)} MB.");
                //}
                //else if (fileBytes.Length > maxAllowedSize)
                //{
                //    return ServiceResult<UploadFile>.Failure($"Dosya çok büyük. Maksimum izin verilen boyut: {maxAllowedSize / (1024 * 1024)} MB.");
                //}

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
