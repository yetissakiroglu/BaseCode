using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Economy.Application.Dtos.AppAuditLogDtos
{
    public class AuditLogQueryDto
    {
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }            // dahil: DateTo.Value.AddDays(1)
        public int? UserId { get; set; }
        public string? UserName { get; set; }
        public string? Action { get; set; }
        public string? EntityName { get; set; }
        public bool? Succeeded { get; set; }
        public int? StatusCode { get; set; }
        public string? Keyword { get; set; }             // Message/JSON içinde ara

        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 20;

        public string? Sort { get; set; } = "-created";  // -created (desc), created (asc)
    }
}
