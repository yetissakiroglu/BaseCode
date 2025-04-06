using Economy.Domain.BaseEntities;

namespace Economy.Domain.Entites.EntityAppPages
{
    public class AppSectionImage : BaseEntity<int>
    {
        public int AppSectionId { get; set; }
        public AppSection AppSection { get; set; }
        public int Sequence { get; set; }
        public string Name { get; set; }
        public string? Content { get; set; }
        public string? Thumbnail { get; set; }

    }
}
