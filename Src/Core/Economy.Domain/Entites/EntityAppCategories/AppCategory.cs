using Economy.Domain.BaseEntities;
using Economy.Domain.Enums;
using Economy.Domain.Models;

namespace Economy.Domain.Entites.EntityCategories
{
    public class AppCategory : BaseEntity<int>
    {
        public string Name { get; set; } 
        public string Url { get; set; } 
        public string? ShortDescription { get; set; } // Kategori açıklaması
        public string? Content { get; set; }

        public string? MetaTitle { get; set; }
        public string? MetaDescription { get; set; }

        public ContentType ContentType { get; set; }
        public PublicationStatus PublicationStatus { get; set; }

        public int? ParentCategoryId { get; set; }
        public AppCategory ParentCategory { get; set; }
        public ICollection<AppCategory> SubCategories { get; set; } = new List<AppCategory>();

        public List<BreadcrumbDto> GetBreadcrumbs()
        {
            var breadcrumbs = new List<BreadcrumbDto>();
            var currentCategory = this;

            // Kategori hiyerarşisini baştan sona alıyoruz
            while (currentCategory != null)
            {
                breadcrumbs.Insert(0, new BreadcrumbDto
                {
                    Name = currentCategory.Name, // Geçerli kategorinin adı
                    Url ="/"+ currentCategory.GetUrlPath() // Geçerli kategorinin URL yolu
                });

                currentCategory = currentCategory.ParentCategory; // Bir üst kategoriye geçiyoruz
            }

            return breadcrumbs;
        }

        // URL formatında tam yol
        public string GetUrlPath()
        {
            return ParentCategory != null ? $"{ParentCategory.GetUrlPath()}/{Url.ToLowerInvariant()}" : Url.ToLowerInvariant();
        }

     
    }
}
