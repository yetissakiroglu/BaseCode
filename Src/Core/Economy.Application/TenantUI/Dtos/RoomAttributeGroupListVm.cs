namespace Economy.Application.TenantUI.Dtos
{
    public class RoomAttributeGroupListVm
    {
        public int Id { get; set; }
        public int SortOrder { get; set; }
        public bool IsActive { get; set; } = true;
        public string Name { get; set; }
    }
    public class RoomAttributeGroupEditVm
    {
        public int? Id { get; set; }
        public int SortOrder { get; set; }
        public bool IsActive { get; set; } = true;

        public List<RoomAttributeGroupTranslationDto> Translations { get; set; }
            = new List<RoomAttributeGroupTranslationDto>();

    }
    public class RoomAttributeGroupTranslationDto
    {
        public int? Id { get; set; }
        public int AppLanguageId { get; set; }
        public string AppLanguageCode { get; set; } = default!;
        public string AppLanguageIcon { get; set; } = default!;
        public string? Name { get; set; }
    }


}
