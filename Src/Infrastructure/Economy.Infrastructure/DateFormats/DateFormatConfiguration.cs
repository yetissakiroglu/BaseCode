using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Economy.Infrastructure.DateFormats
{
    public class DateFormatConfiguration
    {
        public string ServerSideAudit { get; set; } = string.Empty;
        public string ServerSideGeneral { get; set; } = string.Empty;
        public string ClientSideOutput { get; set; } = string.Empty;
    }
}
