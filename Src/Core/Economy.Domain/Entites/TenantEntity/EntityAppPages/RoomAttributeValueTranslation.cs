using Economy.Domain.BaseEntities;
using Economy.Domain.Entites.TenantEntity.EntityAppLanguages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Economy.Domain.Entites.TenantEntity.EntityAppPages
{
    public class RoomAttributeValueTranslation : BaseEntity<int>
    {
        /// <summary>
        /// Hangi oda + attribute kombinasyonuna ait değer?
        /// Örn: Oda 101 + CHECKIN_MESSAGE
        /// </summary>
        public int RoomAttributeValueId { get; set; }
        public RoomAttributeValue RoomAttributeValue { get; set; } = null!;

        /// <summary>
        /// Dil bilgisi (TR, EN vs.)
        /// AppLanguage tablosuna bağlı.
        /// </summary>
        public int AppLanguageId { get; set; }
        public AppLanguage AppLanguage { get; set; } = null!;

        /// <summary>
        /// Dil bazlı metin değeri.
        /// Örn (TR): "Otelimize hoş geldiniz..."
        /// Örn (EN): "Welcome to our hotel..."
        /// </summary>
        public string Text { get; set; } = "";
    }
}
