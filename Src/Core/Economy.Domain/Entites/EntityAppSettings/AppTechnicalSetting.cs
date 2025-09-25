using Economy.Core.Enums;
using Economy.Domain.BaseEntities;

namespace Economy.Domain.Entites.EntityAppSettings
{

    public class AppTechnicalSetting : BaseEntity<int>
    {

        public bool IsSiteLive { get; set; }
        public bool ForceSSL { get; set; }

        // "www.otelsitem.com" gibi; protokol içermez (öneri)
        public string? DomainName { get; set; }

        public bool EnableCDN { get; set; }
        public string? StaticFileUrl { get; set; }

        public bool EnableDebugMode { get; set; }

        public string? MaintenanceMessage { get; set; }

        public bool EnableCache { get; set; }  // Cache altyapısı kullanmasan bile feature flag olarak kalabilir

        // Stil & Script alanları (sık kullanılan)
        public string? CustomCss { get; set; }    // <head> içine <style> ile basılacak
        public string? CustomJs { get; set; }     // </body> öncesi <script> ile basılacak

        // Bakım/Whitelist
        public bool EnableMaintenanceIpWhitelist { get; set; }
        public string? AllowedIpAddresses { get; set; } // "127.0.0.1,192.168.1.5"

        // ---- YENİ MİMARİ ----
        // Hangi konumlara özel HTML/snippet enjekte etmeye izin verildiği:
        public CustomScriptPlacement AllowedCustomScriptPlacements { get; set; }

        // Header/Footer’a serbest HTML/snippet (GA tag, meta verification, third-party init vb.)
        public string? CustomHeaderScripts { get; set; } // <head> içine basılır
        public string? CustomFooterScripts { get; set; } // </body> öncesi basılır

        // Analytics
        public string? GoogleAnalyticsCode { get; set; } // "G-XXXX" veya tam snippet
        public string? FacebookPixelCode { get; set; }   // ID veya snippet


    }

}
