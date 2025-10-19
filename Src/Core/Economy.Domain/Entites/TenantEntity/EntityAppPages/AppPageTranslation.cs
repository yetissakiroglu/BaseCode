using Economy.Domain.BaseEntities;
using Economy.Domain.Entites.TenantEntity.EntityAppLanguages;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Economy.Domain.Entites.TenantEntity.EntityAppPages
{
    public class AppPageTranslation : BaseEntity<int>
    {
        public int AppPageId { get; set; }
        public required string Slug { get; set; }
        public string? Title { get; set; }
        public string? Summary { get; set; }
        public string? Body { get; set; }
        public string? MetaTitle { get; set; }
        public string? MetaDescription { get; set; }
        public int AppLanguageId { get; set; }
        public AppLanguage AppLanguage { get; set; } = null!;
    }
}
