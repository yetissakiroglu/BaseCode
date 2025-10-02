namespace Economy.Application.TenantUI.Dtos.AppLanguageDtos
{
    public class AppLanguageDto
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public bool IsRTL { get; set; }
        public string Icon { get; set; }
        public bool IsActive { get; set; }
        public bool IsDefault { get; set; }
    }
}
