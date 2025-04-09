using Economy.Domain.BaseEntities;
using Economy.Domain.Entites.EntityAppCategories;
using Economy.Domain.Enums;

namespace Economy.Domain.Entites.EntityCategories
{
    public class AppCategory : BaseEntity<int>
    {
       

        public ContentType ContentType { get; set; }
        public PublicationStatus PublicationStatus { get; set; }

        public int? ParentCategoryId { get; set; }
        public AppCategory ParentCategory { get; set; }
        public ICollection<AppCategory> SubCategories { get; set; } = new List<AppCategory>();

        public virtual ICollection<AppCategoryTranslation> Translations { get; set; } = new List<AppCategoryTranslation>();






        //public List<BreadcrumbDto> GetBreadcrumbs()
        //{
        //    var breadcrumbs = new List<BreadcrumbDto>();
        //    var currentCategory = this;

        //    // Kategori hiyerarşisini baştan sona alıyoruz
        //    while (currentCategory != null)
        //    {
        //        breadcrumbs.Insert(0, new BreadcrumbDto
        //        {
        //            Name = currentCategory.Name, // Geçerli kategorinin adı
        //            Url ="/"+ currentCategory.GetUrlPath() // Geçerli kategorinin URL yolu
        //        });

        //        currentCategory = currentCategory.ParentCategory; // Bir üst kategoriye geçiyoruz
        //    }

        //    return breadcrumbs;
        //}

        //// URL formatında tam yol
        //public string GetUrlPath()
        //{
        //    return ParentCategory != null ? $"{ParentCategory.GetUrlPath()}/{Url.ToLowerInvariant()}" : Url.ToLowerInvariant();
        //}


    }
}
