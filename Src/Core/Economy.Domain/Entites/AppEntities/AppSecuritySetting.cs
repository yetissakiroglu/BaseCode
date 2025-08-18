using Economy.Domain.BaseEntities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Economy.Domain.Entites.AppEntities
{
    [Table("AppSecuritySettings")]
    public class AppSecuritySetting : BaseEntity<int>
    {
        // Password policy
        [Range(4, 128)]
        public int PasswordRequiredLength { get; set; } = 6;

        public bool PasswordRequireDigit { get; set; } = true;
        public bool PasswordRequireLowercase { get; set; } = true;
        public bool PasswordRequireUppercase { get; set; } = false;
        public bool PasswordRequireNonAlphanumeric { get; set; } = false;

        // Lockout policy
        [Range(1, 1440)]
        public int LockoutTimeSpanMinutes { get; set; } = 30;

        [Range(1, 20)]
        public int LockoutMaxFailedAccessAttempts { get; set; } = 5;

        public bool LockoutAllowedForNewUsers { get; set; } = true;

        // SignIn policy
        public bool SignInRequireConfirmedEmail { get; set; } = false;
        public bool SignInRequireConfirmedPhoneNumber { get; set; } = false;

        // 2FA policy (kurumsal zorunluluk)
        public bool TwoFactorRequired { get; set; } = false;
    }
}
