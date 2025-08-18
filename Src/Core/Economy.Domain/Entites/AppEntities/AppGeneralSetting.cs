using Economy.Domain.BaseEntities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Economy.Domain.Entites.AppEntities
{
    [Table("AppGeneralSettings")]
    public class AppGeneralSetting : BaseEntity<int>
    {
        [Required, StringLength(200)]
        public string SiteName { get; set; } = "";

        [StringLength(200)]
        public string? Domain { get; set; }

        [Required, StringLength(20)]
        public string Theme { get; set; } = "light";

        [StringLength(300)]
        public string? LogoUrl { get; set; }

        [StringLength(120)]
        public string? MetaTitleSuffix { get; set; }

        [StringLength(300)]
        public string? DefaultMetaDescription { get; set; }

    }
}
