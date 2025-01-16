namespace Economy.Infrastructure.Services.FileHelperServices.Models
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
}
