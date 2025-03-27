using Economy.Domain.BaseEntities;

namespace Economy.Domain.Entites.EntityAppSettings
{

    public class AppTechnicalSetting : BaseEntity<int>
    {
        /// <summary>
        /// Site yayında mı?
        /// False ise ziyaretçilere site bakımda mesajı gösterilir.
        /// </summary>
        public bool IsSiteLive { get; set; }

        /// <summary>
        /// HTTPS yönlendirmesi aktif mi?
        /// True ise HTTP istekleri otomatik olarak HTTPS’e yönlendirilir.
        /// </summary>
        public bool ForceSSL { get; set; }

        /// <summary>
        /// Sitenin tam domain adı.
        /// Örn: www.otelsitem.com
        /// </summary>
        public string DomainName { get; set; }

        /// <summary>
        /// CDN kullanımı aktif mi?
        /// True ise statik içerikler CDN üzerinden servis edilir.
        /// </summary>
        public bool EnableCDN { get; set; }

        /// <summary>
        /// Statik dosyaların yükleneceği harici adres (CDN URL’si).
        /// Örn: https://cdn.otelsitem.com
        /// </summary>
        public string StaticFileUrl { get; set; }

        /// <summary>
        /// Uygulama debug modda mı çalışıyor?
        /// True ise hata detayları geliştiriciye gösterilir.
        /// </summary>
        public bool EnableDebugMode { get; set; }

        /// <summary>
        /// Uygulamanın sürüm bilgisi.
        /// Örn: v1.0.0 — footer'da gösterim, log kayıtları için kullanılabilir.
        /// </summary>
        public string AppVersion { get; set; }

        /// <summary>
        /// Site bakım modundayken ziyaretçilere gösterilecek özel mesaj.
        /// Genellikle HTML desteklidir.
        /// </summary>
        public string MaintenanceMessage { get; set; }

        /// <summary>
        /// Sistem genelinde cache kullanımı aktif mi?
        /// True ise tüm servislerde önbellekleme yapılabilir.
        /// </summary>
        public bool EnableCache { get; set; }

        /// <summary>
        /// Sayfanın görünümüne özel ek stil (CSS) kodları.
        /// Direkt olarak sayfanın head kısmına basılabilir.
        /// </summary>
        public string CustomCss { get; set; }

        /// <summary>
        /// Sayfanın davranışına özel ek JavaScript kodları.
        /// Footer veya body sonunda çalıştırılabilir.
        /// </summary>
        public string CustomJs { get; set; }

        /// <summary>
        /// Sadece belirli IP adresleri bakımdayken siteye erişebilsin mi?
        /// True ise whitelist kontrolü yapılır.
        /// </summary>
        public bool EnableMaintenanceIpWhitelist { get; set; }

        /// <summary>
        /// Virgülle ayrılmış, bakım modundayken erişime izin verilen IP listesi.
        /// Örn: 127.0.0.1,192.168.1.5
        /// </summary>
        public string? AllowedIpAddresses { get; set; }

        /// <summary>
        /// Head bölümüne özel script kodları eklemeye izin verilsin mi?
        /// True ise admin panelde head script alanı aktif olur.
        /// </summary>
        public bool EnableCustomHeaderScripts { get; set; }

        /// <summary>
        /// Footer bölümüne özel script kodları eklemeye izin verilsin mi?
        /// </summary>
        public bool EnableCustomFooterScripts { get; set; }

        /// <summary>
        /// Google Analytics entegrasyonu için script kodu veya Tracking ID.
        /// Örn: G-1234XXXX
        /// </summary>
        public string? GoogleAnalyticsCode { get; set; }

        /// <summary>
        /// Facebook reklam takibi için kullanılacak Pixel kodu.
        /// </summary>
        public string? FacebookPixelCode { get; set; }

        /// <summary>
        /// Head ve Footer script alanları aktif mi?
        /// Bu alan True değilse custom script'ler render edilmez.
        /// </summary>
        public bool EnableGlobalScriptInjection { get; set; }

        /// <summary>
        /// Sayfa açılırken preloader animasyonu gösterilsin mi?
        /// </summary>
        public bool EnablePreloader { get; set; }

        /// <summary>
        /// Preloader içeriği (HTML olarak). SVG, div, lottie veya gif olabilir.
        /// </summary>
        public string? PreloaderHtml { get; set; }
    }

}
