using Economy.Domain.BaseEntities;

namespace Economy.Domain.Entites.EntityAppSettings
{
    public class AppSetting : BaseEntity<int>
    {

        /// <summary>
        /// Site yayında mı? False ise bakım mesajı gösterilir.
        /// </summary>
        public bool IsSiteLive { get; set; }

        /// <summary>
        /// Tüm istekler HTTPS'e yönlendirilsin mi?
        /// </summary>
        public bool ForceSSL { get; set; }

        /// <summary>
        /// Sitenin ana alan adı. Örn: www.otelsitem.com
        /// </summary>
        public string DomainName { get; set; }

        /// <summary>
        /// CDN aktif mi? (örn: resimler dış CDN'den yüklensin mi?)
        /// </summary>
        public bool EnableCDN { get; set; }

        /// <summary>
        /// Statik dosyalar için kullanılan dış bağlantı (CDN/S3 vs.)
        /// </summary>
        public string StaticFileUrl { get; set; }

        /// <summary>
        /// Geliştirme ortamı için hata ayıklama modu.
        /// </summary>
        public bool EnableDebugMode { get; set; }

        /// <summary>
        /// Sayfanın stiline özel CSS kodları.
        /// </summary>
        public string CustomCss { get; set; }

        /// <summary>
        /// Sayfanın davranışına özel JavaScript kodları.
        /// </summary>
        public string CustomJs { get; set; }

        /// <summary>
        /// Uygulama versiyonu (örn: v1.0.0)
        /// </summary>
        public string AppVersion { get; set; }

        /// <summary>
        /// Site kapalıysa ziyaretçilere gösterilecek bakım mesajı.
        /// </summary>
        public string MaintenanceMessage { get; set; }

        /// <summary>
        /// Bu ayarların bellekte (cache) tutulup tutulmayacağını belirler.
        /// </summary>
        public bool EnableCache { get; set; }

        public virtual ICollection<AppSettingTranslation> Translations { get; set; } = new List<AppSettingTranslation>();


    }
}
