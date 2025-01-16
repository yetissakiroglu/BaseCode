namespace Economy.Application.Infrastructure.ConfigurationModels
{
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
