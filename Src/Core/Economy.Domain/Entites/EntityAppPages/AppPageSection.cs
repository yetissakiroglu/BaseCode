using Economy.Domain.BaseEntities;

namespace Economy.Domain.Entites.EntityAppPages
{
	public class AppPageSection : BaseEntity<int>
    {
        public string Name { get; set; }
        public string? Content { get; set; }
        public bool IsVisible { get; set; }
        public int AppPageId { get; set; }
        public virtual AppPage AppPage { get; set; } = new AppPage();
        public int AppSectionId { get; set; }
        public virtual AppSection AppSection { get; set; } = new AppSection();
    }

}
