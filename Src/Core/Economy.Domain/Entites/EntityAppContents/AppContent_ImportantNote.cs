using Economy.Domain.BaseEntities;
using Economy.Domain.Entites.EntityPages;

namespace Economy.Domain.Entites.EntityAppContents
{
    public class AppContent_ImportantNote:BaseEntity<int>
    {

        public AppContent_ImportantNote() { }
        public int AppContentId { get; set; }
        public AppContent? AppContent { get; set; }
        public string Title { get; set; }
        public string? Content { get; set; }
        public int Sequence { get; set; }

    }
}
