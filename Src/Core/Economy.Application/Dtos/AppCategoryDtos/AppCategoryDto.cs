using Economy.Domain.Entites.EntityCategories;

namespace Economy.Application.Dtos.AppCategoryDtos
{
    public record AppCategoryDto
    {
        public int Id { get; init; } // Zorunlu alan
        public string Name { get; set; } // Kategori adı
        public string Slug { get; set; } // SEO uyumlu kategori URL'si
        public string? Description { get; set; } // Kategori açıklaması

        // SEO alanları
        public string? MetaTitle { get; set; }
        public string? MetaDescription { get; set; }
        public int? ParentID { get; init; } // Üst kategori ID'si
        public ICollection<AppCategoryDto> Children { get; init; } = new List<AppCategoryDto>(); // Alt kategoriler

        // Entity'den DTO'ya dönüşüm metodu
        public static AppCategoryDto FromEntity(AppCategory entity)
        {
            if (entity == null) return null;

            return new AppCategoryDto
            {
                Id = entity.Id,
                Name = entity.Name,
                Slug = entity.Slug,
                Description = entity.Description,
                MetaTitle = entity.MetaTitle,
                MetaDescription = entity.MetaDescription,
            };
        }

        // Listeleme metodu
        public static List<AppCategoryDto> List(List<AppCategory> entities)
        {
            return entities.Select(FromEntity).ToList();
        }

        
    }


}
