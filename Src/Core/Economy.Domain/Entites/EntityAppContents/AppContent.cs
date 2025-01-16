using Economy.Domain.BaseEntities;
using Economy.Domain.Entites.EntityAppContents;
using Economy.Domain.Entites.EntityCategories;
using Economy.Domain.Entites.Identities;
using Economy.Domain.Enums;
using Economy.Domain.Extensions;
using Economy.Domain.Models;

namespace Economy.Domain.Entites.EntityPages
{
    public class AppContent :BaseEntity<int>
    {
        public string Title { get; set; }
        public string? ShortDescription { get; set; }
        public string Slug { get; set; }
        public string? Content { get; set; }                                 
        public bool IsExternal { get; set; }

        // SEO alanları
        public string? MetaTitle { get; set; }
        public string? MetaDescription { get; set; }
        public string? MetaKeywords { get; set; }

        /// <summary>
        /// Haber Manşet Bölgesi
        /// </summary>
        public bool IsHeadline { get; set; } = false;

        /// <summary>
        /// Öne Çıkan Haberler Bölgesi
        /// </summary>
        public bool IsFeatured { get; set; } = false;

        /// <summary>
        /// Son Dakika Haberler Bölgesi
        /// </summary>
        public bool IsBreakingNews { get; set; } = false;

        public ContentType ContentType { get; set; } = ContentType.News; // Onay durumu 
     
        // Yayın ve onay durumları
        public ApprovalStatus ApprovalStatus { get; set; } // Onay durumu
        public PublicationStatus PublicationStatus { get; set; } // Yayın durumu


        // İlişkiler
        public int AppCategoryId { get; set; } // Kategori ID'si     
        public virtual AppCategory AppCategory { get; set; }

        // URL oluşturmak için tam kategori yolunu ve haber başlığını kullanır
        public string GetUrl()
        {
            // Slug formatında URL oluşturuyoruz
            return $"/{AppCategory?.GetUrlPath()}/{Slug?.ToUrlFriendly()}-{Id}";
        }
        public List<BreadcrumbDto> GetBreadcrumbs()
        {
            var breadcrumbs = new List<BreadcrumbDto>();

            // Kategori yolunu alıyoruz
            var categoryPath = AppCategory.GetUrlPath().Split('/');
            var url = "";

            // Kategorilerin her birini ekliyoruz
            foreach (var category in categoryPath)
            {
                url += "/" + category;
                breadcrumbs.Add(new BreadcrumbDto
                {
                    Name = AppCategory.Name,  // Kategorinin adını baş harfini büyük yaparak ekliyoruz
                    Url = url
                });
            }

            // Haber başlığını ekliyoruz
            breadcrumbs.Add(new BreadcrumbDto
            {
                Name = Title,  // Haber başlığını ekliyoruz
                Url = $"{url}/{Slug.ToUrlFriendly()}",
                Active = true
            });

            return breadcrumbs;
        }

        public string AppUserId { get; set; } // Blog yazısının yazarı (Kullanıcı) ID'si
        public AppUser AppUser { get; set; } // Blog yazısının yazarı (Kullanıcı)


        // Opsiyonel alanlar
        public string? Author { get; set; } // Opsiyonel yazar adı (manuel giriş)
        public long ViewCount { get; set; } // Görüntülenme sayısı
        public string? FeaturedImage { get; set; } // Öne çıkan görsel URL'si


        public virtual ICollection<AppComment> AppComments { get; set; }
        public virtual ICollection<AppImageGroup> AppImageGroups { get; set; } 
        public virtual ICollection<AppAmenityGroup> AppAmenityGroups { get; set; }
        public virtual ICollection<AppContent_Document> AppContent_Documents { get; set; } = new List<AppContent_Document>();
        public ICollection<AppContent_ImportantNote> AppContent_ImportantNotes { get; set; } = new List<AppContent_ImportantNote>();
        public ICollection<AppContentTag> AppContentTags { get; set; } // Bloga ait etiketler (Many-to-Many)





    }

  

}
