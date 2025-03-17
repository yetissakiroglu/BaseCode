using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoggingLibrary
{
    public class LogEntry
    {
        public int Id { get; set; }
        public string UserId { get; set; } // Kullanıcı bilgisi
        public string Layer { get; set; }  // Controller, Service, Repository
        public string Method { get; set; } // Çalıştırılan metod
        public string Parameters { get; set; } // Metod parametreleri
        public string Response { get; set; } // Dönüş değeri
        public string LogMessage { get; set; } // Dönüş değeri
        public string IpAddress { get; set; } // Dönüş değeri
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    }
}
