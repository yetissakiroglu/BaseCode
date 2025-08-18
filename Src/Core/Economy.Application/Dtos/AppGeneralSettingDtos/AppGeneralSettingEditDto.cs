namespace Economy.Application.Dtos.AppGeneralSettingDtos
{
    public class AppGeneralSettingEditDto
    {
        public int Id { get; set; } // zorunlu
        public string SiteName { get; set; } = "";
        public string? Domain { get; set; }
        public string Theme { get; set; } = "light";
        public string? LogoUrl { get; set; }
        public string? MetaTitleSuffix { get; set; }
        public string? DefaultMetaDescription { get; set; }
    }
}
