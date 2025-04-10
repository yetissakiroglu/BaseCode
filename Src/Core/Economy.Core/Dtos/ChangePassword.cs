using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Economy.Core.Dtos
{
    public class ChangePassword
    {
        public int UserId { get; set; } // AppUser int ID kullandığı için
        public string CurrentPassword { get; set; }
        public string NewPassword { get; set; }
    }
}
