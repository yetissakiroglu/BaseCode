#nullable disable

namespace Economy.Domain.Entites.EntityAppFiles
{
    public class AppFileImage
    {
        public Guid Id { get; set; }
        public string OriginalFileName { get; set; }
        public byte[] ImageData { get; set; }
    }
}
