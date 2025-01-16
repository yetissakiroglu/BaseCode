#nullable disable

namespace Economy.Domain.Entites.EntityAppFiles
{
    public class AppFileDocument
    {
        public Guid Id { get; set; }
        public string OriginalFileName { get; set; }
        public byte[] DocumentData { get; set; }

    }
}
