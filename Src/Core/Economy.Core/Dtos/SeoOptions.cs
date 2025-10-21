namespace Economy.Core.Dtos
{
    public class SeoOptions
    {
        public string SlugMode { get; set; } = "latin";
        public int MaxLength { get; set; } = 80;
        public bool EnsureUnique { get; set; } = true;
    }
}
