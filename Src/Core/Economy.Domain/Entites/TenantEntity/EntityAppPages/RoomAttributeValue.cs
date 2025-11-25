using Economy.Domain.BaseEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Economy.Domain.Entites.TenantEntity.EntityAppPages
{
    public class RoomAttributeValue : BaseEntity<int>
    {
        public int AppPageId { get; set; }
        public AppPage AppPage { get; set; } = null!; 
        public int DefRoomAttributeId { get; set; }
        public DefRoomAttribute DefRoomAttribute { get; set; } = null!; 
        /// <summary> /// Option tipi özellikler için (Oda Tipi, Manzara, Yatak Tipi vb.).
        /// /// Bool/Number/Text tiplerinde genelde null kalır. 
        /// /// </summary> 
        public int? DefRoomAttributeOptionId { get; set; } 
        public DefRoomAttributeOption? DefRoomAttributeOption { get; set; } 
        /// <summary> /// Serbest metin gerektiren özellikler için. /// </summary> 
        public string? ValueText { get; set; } /// <summary> /// Sayısal değer gerektiren özellikler için (m², kişi sayısı vb.). /// </summary> 
        public int? ValueInt { get; set; } /// <summary> /// Evet/Hayır özellikler için (Balkon, Sigara, Wi-Fi, Smart TV vb.). /// </summary> 
        public bool? ValueBool { get; set; } }
    }
