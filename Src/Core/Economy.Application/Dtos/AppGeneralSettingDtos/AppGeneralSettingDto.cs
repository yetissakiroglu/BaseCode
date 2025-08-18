namespace Economy.Application.Dtos.AppGeneralSettingDtos
{
    public class AppGeneralSettingDto
    {
        public int Id { get; set; }
        public string SiteName { get; set; } = "";
        public string? Domain { get; set; }
        public string Theme { get; set; } = "light";
        public string? LogoUrl { get; set; }
        public string? MetaTitleSuffix { get; set; }
        public string? DefaultMetaDescription { get; set; }
      
    }
}
