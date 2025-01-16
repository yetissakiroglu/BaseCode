using Economy.Domain.Entites.EntityPages;

namespace Economy.Domain.Entites.EntityAppContents
{
    public class AppContentTag: IEntity<int>
    {
        public int AppContentId { get; set; } // Blog ID'si
        public AppContent AppContent { get; set; } // Blog yazısı

        public int AppTagId { get; set; } // Etiket ID'si
        public AppTag AppTag { get; set; } // Etiket
    }
}
