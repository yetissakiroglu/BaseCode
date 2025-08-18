using Economy.Domain.BaseEntities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Economy.Domain.Entites.AppEntities
{
    [Table("AppAuditLogs")]
    public class AppAuditLog : BaseEntity<int> // BaseEntity<long> -> Id, CreatedDate, UpdatedDate, IsDeleted...
    {
        // Zaman damgası
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        // Kim
        public int? UserId { get; set; }
        [StringLength(256)]
        public string? UserName { get; set; }

        // Ne
        [Required, StringLength(64)]
        public string Action { get; set; } = "";       // Create, Update, Delete, Login, Logout, etc.
        [StringLength(128)]
        public string? EntityName { get; set; }        // "AppUser", "Order", "Settings"...
        [StringLength(128)]
        public string? EntityId { get; set; }          // "42" gibi

        // Mesaj/Değişim
        [StringLength(512)]
        public string? Message { get; set; }           // Kısa özet
        public string? OldValuesJson { get; set; }     // JSON snapshot
        public string? NewValuesJson { get; set; }     // JSON snapshot
        public string? AffectedColumnsJson { get; set; }

        // HTTP bağlam
        [StringLength(16)]
        public string? HttpMethod { get; set; }        // GET/POST/PUT/DELETE
        [StringLength(256)]
        public string? RequestPath { get; set; }       // /Settings/General
        [StringLength(64)]
        public string? IpAddress { get; set; }
        [StringLength(256)]
        public string? UserAgent { get; set; }
        [StringLength(64)]
        public string? CorrelationId { get; set; }

        // Sonuç
        public bool Succeeded { get; set; } = true;
        public int? StatusCode { get; set; }           // 200, 400, 500...
        public int? DurationMs { get; set; }           // işlem süresi (opsiyonel)
    }
}
