namespace Economy.Core.Helpers.Dtos
{
    public class UploadFile
    {
        public string MediaName { get; set; }
        public string MediaURL { get; set; }
        public string MediaFullURL { get; set; }
        public long FileSize { get; set; }
        public string CombinedFolderPath { get; set; }
        public byte[] ByteArrayMedia { get; set; }
        public bool IsByteArray { get; set; }
        public string AltAttribute { get; set; }
        public string TitleAttribute { get; set; }
        public string MimeType { get; set; }
        public string Guid { get; set; }
    }

    public class FileUploadConfiguration
    {
        public ImageSettings Image { get; set; }
        public VideoSettings Video { get; set; }
        public DocumentSettings Document { get; set; }
    }


    public class ImageSettings
    {
        public int MaxUploadSizeMB { get; set; }
        public int MinUploadSizeMB { get; set; }
    }

    public class VideoSettings
    {
        public int MaxUploadSizeMB { get; set; }
        public int MinUploadSizeMB { get; set; }
    }

    public class DocumentSettings
    {
        public int MaxUploadSizeMB { get; set; }
        public int MinUploadSizeMB { get; set; }
    }

}
