using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Economy.Application.Dtos.DashboardSummaryDtos
{
    public class DashboardSummaryDto
    {
        // Cards
        public int TotalUsers { get; set; }
        public int ActiveUsers { get; set; }            // IsDeleted = false
        public int LockedOutUsers { get; set; }         // LockoutEnd > now
        public int TwoFactorEnabledUsers { get; set; }

        public int ErrorsLast24h { get; set; }
        public int FailedLoginsLast24h { get; set; }
        public DateTime? LastBackupUtc { get; set; }    // varsa

        // Charts
        public List<TimePoint> ErrorsPerHour24h { get; set; } = new();
        public List<TimePoint> FailedLoginsPerHour24h { get; set; } = new();
        public List<TimePoint> NewUsersPerDay7d { get; set; } = new();

        // Tables
        public List<RecentErrorDto> RecentErrors { get; set; } = new();
    }

    public class TimePoint
    {
        public string Label { get; set; } = "";  // "HH:mm" veya "dd MMM"
        public int Value { get; set; }
    }

    public class RecentErrorDto
    {
        public int Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public string? Message { get; set; }
        public string? RequestPath { get; set; }
        public int? StatusCode { get; set; }
        public string? ExceptionType { get; set; }
    }

}
