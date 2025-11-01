using Economy.Core.Enums;

namespace Economy.Application.TenantUI.Dtos.AppBlockGroupDtos
{
    public class AppBlockGroupDto
    {
        public int? Id { get; set; }
        public BlockColumns Columns { get; set; } = BlockColumns.Three;
        public bool ShowTitle { get; set; } = true;
        public bool ShowDescription { get; set; } = true;
        public bool IsActive { get; set; } = true;
        public List<AppBlockGroupTranslationDto> Translations { get; set; } = new();
    }
}
