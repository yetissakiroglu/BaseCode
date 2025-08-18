using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Economy.Application.Dtos.AppSecuritySettingDtos
{
    public class AppSecuritySettingCreateDto
    {
        public int PasswordRequiredLength { get; set; } = 6;
        public bool PasswordRequireDigit { get; set; } = true;
        public bool PasswordRequireLowercase { get; set; } = true;
        public bool PasswordRequireUppercase { get; set; } = false;
        public bool PasswordRequireNonAlphanumeric { get; set; } = false;

        public int LockoutTimeSpanMinutes { get; set; } = 30;
        public int LockoutMaxFailedAccessAttempts { get; set; } = 5;
        public bool LockoutAllowedForNewUsers { get; set; } = true;

        public bool SignInRequireConfirmedEmail { get; set; } = false;
        public bool SignInRequireConfirmedPhoneNumber { get; set; } = false;

        public bool TwoFactorRequired { get; set; } = false;
    }
}
