namespace Economy.Domain.Models
{
    public class BreadcrumbDto
    {
        public string Name { get; set; }  // Kategori adı ya da başlık
        public string Url { get; set; }   // Kategori URL'si
        public bool Active { get; set; } = false;
    }
}
