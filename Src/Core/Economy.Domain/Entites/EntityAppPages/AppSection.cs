using Economy.Domain.BaseEntities;
using Economy.Domain.Enums;

namespace Economy.Domain.Entites.EntityAppPages
{
    public class AppSection:BaseEntity<int>
    {
        public string Name { get; set; }
        public string? Content { get; set; }
        public SectionType SectionType { get; set; }
        public virtual ICollection<AppPageSection> AppPageSections { get; set; } = [];
    }
}
