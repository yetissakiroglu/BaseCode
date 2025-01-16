using Economy.Domain.BaseEntities;

namespace Economy.Domain.Entites.EntityAppContents
{
    public class AppTag:BaseEntity<int>
    {
        public string Name { get; set; } 
        public string Slug { get; set; } 
        public string? Description { get; set; }
        public ICollection<AppContentTag> AppContentTags { get; set; } // -- (Many-to-Many)
    }
}
