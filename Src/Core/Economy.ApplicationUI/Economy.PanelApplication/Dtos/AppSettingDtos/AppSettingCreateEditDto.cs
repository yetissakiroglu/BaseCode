namespace Economy.Panel.Application.Dtos.AppSettingDtos
{
    public class AppSettingCreateEditDto
    {
        public int Id { get; set; }
        public List<AppSettingTranslationDto> Translations { get; set; } = new();
    }
}
