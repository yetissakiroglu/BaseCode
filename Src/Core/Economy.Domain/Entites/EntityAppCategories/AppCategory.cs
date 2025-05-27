using Economy.Domain.BaseEntities;
using Economy.Domain.Entites.EntityAppCategories;
using Economy.Domain.Enums;
using Economy.Domain.Models;

namespace Economy.Domain.Entites.EntityCategories
{
    public class AppCategory : BaseEntity<int>
    {
        public ContentType ContentType { get; set; }

        public int? ParentCategoryId { get; set; }
        public AppCategory? ParentCategory { get; set; }

        public ICollection<AppCategory> SubCategories { get; set; } = new List<AppCategory>();

        public virtual ICollection<AppCategoryTranslation> Translations { get; set; } = new List<AppCategoryTranslation>();

        // Breadcrumb fonksiyonu: dil desteği getName fonksiyonu ile dışarıdan alınır
        public List<BreadcrumbDto> GetBreadcrumbs(Func<AppCategory, string> getName, Func<AppCategory, string> getUrl)
        {
            var breadcrumbs = new List<BreadcrumbDto>();
            var current = this;

            while (current != null)
            {
                breadcrumbs.Insert(0, new BreadcrumbDto
                {
                    Name = getName(current),
                    Url = "/" + getUrl(current)
                });

                current = current.ParentCategory;
            }

            return breadcrumbs;
        }
    }
}
