using Economy.Domain.BaseEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Economy.Domain.Entites.TenantEntity.EntityAppContents
{
    public class AppPageBlock:BaseEntity<int>
    {
        public int AppPageId { get; set; }
        public AppPage AppPage { get; set; } = null!;

        public BlockType Type { get; set; }
        public int SortOrder { get; set; }
        public bool IsActive { get; set; } = true;
        public ContentStage Stage { get; set; } = ContentStage.Draft;

        // Dilden bağımsız alanlar (layout, görsel url, kolon, vb.)
        public string SharedJson { get; set; } = "{}";

        // Kütüphane ile ilişki (opsiyonel)
        public int? AppBlockLibraryId { get; set; }
        public bool IsLinkedToLibrary { get; set; }
        public AppBlockLibrary? AppBlockLibrary { get; set; }

        public ICollection<AppPageBlockTranslation> Translations { get; set; } = [];
    }
}
