using Economy.Domain.BaseEntities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Economy.Domain.Entites.AppEntities
{
    [Table("AppErrorLogs")]
    public class AppErrorLog : BaseEntity<int> // BaseEntity<long> içinde CreatedDate yoksa sorun değil
    {
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Kim/bağlam
        public int? UserId { get; set; }
        [StringLength(256)] public string? UserName { get; set; }

        // HTTP
        [StringLength(16)] public string? HttpMethod { get; set; }
        [StringLength(256)] public string? RequestPath { get; set; }
        [StringLength(512)] public string? QueryString { get; set; }
        [StringLength(64)] public string? IpAddress { get; set; }
        [StringLength(256)] public string? UserAgent { get; set; }
        [StringLength(64)] public string? CorrelationId { get; set; }
        public int? StatusCode { get; set; }

        // Hata
        [StringLength(256)] public string? ExceptionType { get; set; }
        [StringLength(1024)] public string? Message { get; set; }
        public string? StackTrace { get; set; }
        [StringLength(256)] public string? Source { get; set; }
        [StringLength(256)] public string? TargetSite { get; set; }

        // Ek bilgiler
        public string? HeadersJson { get; set; }
        public string? RequestBodyTruncated { get; set; }
        public string? CustomDataJson { get; set; }
    }
}
