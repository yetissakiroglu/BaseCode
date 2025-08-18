using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Economy.Application.Dtos.AppErrorLogDtos
{
    public class ErrorLogQueryDto
    {
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }     // dahil: < DateTo.AddDays(1)
        public int? UserId { get; set; }
        public string? UserName { get; set; }
        public int? StatusCode { get; set; }
        public string? CorrelationId { get; set; }
        public string? ExceptionType { get; set; }
        public string? Keyword { get; set; }      // Message/StackTrace/Source/Path içinde

        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 20;
        public string? Sort { get; set; } = "-created"; // -created, created, -status, status
    }

}
