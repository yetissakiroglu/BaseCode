using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Economy.Application.AdminUI.Dtos.DashboardSummaryDtos
{
    public class DashboardSummaryDto
    {
        // Cards
        public int TotalUsers { get; set; }
        public int ActiveUsers { get; set; }            // IsDeleted = false
        public int LockedOutUsers { get; set; }         // LockoutEnd > now
        public int TwoFactorEnabledUsers { get; set; }

        public DateTime? LastBackupUtc { get; set; }    // varsa

    }

  


}
