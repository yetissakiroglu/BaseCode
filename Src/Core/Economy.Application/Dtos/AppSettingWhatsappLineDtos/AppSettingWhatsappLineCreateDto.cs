using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Economy.Panel.Application.Dtos.AppSettingWhatsappLineDtos
{
    public class AppSettingWhatsappLineCreateDto
    {
        public string CountryCode { get; set; }  // Ülke kodu, örn: "+90", "+1"
        public string Number { get; set; }       // Telefon numarasının geri kalanı, örn: "5551112233"
        public bool IsPrimary { get; set; }      // Birincil numara mı?
    }
}
