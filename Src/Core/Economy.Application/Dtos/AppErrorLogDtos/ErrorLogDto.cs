using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Economy.Application.Dtos.AppErrorLogDtos
{
    public class ErrorLogDto
    {
        public long Id { get; set; }
        public DateTime CreatedAt { get; set; }

        public int? UserId { get; set; }
        public string? UserName { get; set; }

        public string? HttpMethod { get; set; }
        public string? RequestPath { get; set; }
        public string? QueryString { get; set; }
        public string? IpAddress { get; set; }
        public string? UserAgent { get; set; }
        public string? CorrelationId { get; set; }
        public int? StatusCode { get; set; }

        public string? ExceptionType { get; set; }
        public string? Message { get; set; }
        public string? StackTrace { get; set; }
        public string? Source { get; set; }
        public string? TargetSite { get; set; }

        public string? HeadersJson { get; set; }
        public string? RequestBodyTruncated { get; set; }
        public string? CustomDataJson { get; set; }
    }
}
