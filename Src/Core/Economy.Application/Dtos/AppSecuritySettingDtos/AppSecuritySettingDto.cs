using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Economy.Application.Dtos.AppSecuritySettingDtos
{
    public class AppSecuritySettingDto
    {
        public int Id { get; set; }

        public int PasswordRequiredLength { get; set; }
        public bool PasswordRequireDigit { get; set; }
        public bool PasswordRequireLowercase { get; set; }
        public bool PasswordRequireUppercase { get; set; }
        public bool PasswordRequireNonAlphanumeric { get; set; }

        public int LockoutTimeSpanMinutes { get; set; }
        public int LockoutMaxFailedAccessAttempts { get; set; }
        public bool LockoutAllowedForNewUsers { get; set; }

        public bool SignInRequireConfirmedEmail { get; set; }
        public bool SignInRequireConfirmedPhoneNumber { get; set; }

        public bool TwoFactorRequired { get; set; }


    }

   

    

}
