using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Economy.Application.Dtos.AppAuditLogDtos
{
    public class AuditLogDto
    {
        public long Id { get; set; }
        public DateTime? CreatedDate { get; set; }

        public int? UserId { get; set; }
        public string? UserName { get; set; }

        public string Action { get; set; } = "";
        public string? EntityName { get; set; }
        public string? EntityId { get; set; }
        public string? Message { get; set; }

        public string? HttpMethod { get; set; }
        public string? RequestPath { get; set; }
        public string? IpAddress { get; set; }
        public string? CorrelationId { get; set; }

        public bool Succeeded { get; set; }
        public int? StatusCode { get; set; }
        public int? DurationMs { get; set; }

        public string? OldValuesJson { get; set; }
        public string? NewValuesJson { get; set; }
        public string? AffectedColumnsJson { get; set; }
    }
}
