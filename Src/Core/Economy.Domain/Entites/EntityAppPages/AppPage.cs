using Economy.Domain.BaseEntities;
using Economy.Domain.Entites.EntityCategories;
using Economy.Domain.Extensions;
using Economy.Domain.Models;

namespace Economy.Domain.Entites.EntityAppPages
{
	public class AppPage : BaseEntity<int>
    {
        public string Title { get; set; }
        public string? ShortDescription { get; set; }
        public string Slug { get; set; }
        public string? Content { get; set; }
        public string? MetaTitle { get; set; }
        public string? MetaDescription { get; set; }
        public bool IsHomePage { get; set; }

        // URL oluşturmak için tam kategori yolunu ve haber başlığını kullanır
        public string GetUrl()
        {
            // Slug formatında URL oluşturuyoruz
            return $"/{Slug?.ToUrlFriendly()}-{Id}";
        }
        public List<BreadcrumbDto> GetBreadcrumbs()
        {
            var breadcrumbs = new List<BreadcrumbDto>();

            // Haber başlığını ekliyoruz
            breadcrumbs.Add(new BreadcrumbDto
            {
                Name = Title,  // Haber başlığını ekliyoruz
                Url = $"{Slug.ToUrlFriendly()}",
                Active = true
            });

            return breadcrumbs;
        }

        public virtual ICollection<AppPageSection> AppPageSections { get; set; } = [];
    }
}
