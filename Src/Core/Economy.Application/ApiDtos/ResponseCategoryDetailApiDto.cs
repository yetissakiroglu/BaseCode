using Economy.Domain.Entites.EntityCategories;
using Economy.Domain.Enums;

namespace Economy.Application.ApiDtos
{
    public class ResponseCategoryDetailApiDto
    {
        public int Id { get; set; }
        public string Name { get; set; } // Kategori adı
        public string Url { get; set; } // SEO uyumlu kategori URL'si
        public string? Description { get; set; } // Kategori açıklaması
        public ContentType ContentType { get; set; }
        // SEO alanları
        public string? MetaTitle { get; set; }
        public string? MetaDescription { get; set; }
        public object Breadcrumbs { get; set; }

        public static ResponseCategoryDetailApiDto FromEntity(AppCategory model)
        {
            return new ResponseCategoryDetailApiDto
            {
                Id = model.Id,
                Name = model.Name,
                Url = "/" + model.GetUrlPath(),
                Description = model.Description,
                MetaTitle = model.MetaTitle,
                MetaDescription = model.MetaDescription,
                ContentType = model.ContentType,
                Breadcrumbs = model.GetBreadcrumbs(),
            };
        }
        public static List<ResponseCategoryDetailApiDto> FromEntities(List<AppCategory> modelList)
        {
            return modelList.Select(model => FromEntity(model)).ToList();
        }
    }
}
