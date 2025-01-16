using Economy.Domain.BaseEntities;
using Economy.Domain.Entites.EntityPages;

namespace Economy.Domain.Entites.EntityAppContents
{
    public class AppContent_Document : BaseEntity<int>
    {
        public string DocumentId { get; set; }
        public string Title { get; set; }
        public int AppContentId { get; set; }
        public AppContent AppContent { get; set; }
    }
   
}
