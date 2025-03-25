using Economy.Domain.Entites.EntityCategories;
using Economy.Domain.Entites.EntityPages;
using Economy.Domain.Enums;
using Economy.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

namespace Economy.Persistence.Seeds
{
    public class AppContentSeeder
    {
        private readonly AppDbContext _context;

        public AppContentSeeder(AppDbContext context)
        {
            _context = context;
        }

        public async Task SeedAsync()
        {
            if (!_context.AppContents.Any())
            {
                var data = GetSeedContents();
                await _context.AppContents.AddRangeAsync(data);
                await _context.SaveChangesAsync();
            }
        }
        private IEnumerable<AppContent> GetSeedContents()
        {
            return new List<AppContent>
        {
            new   AppContent
        {
            Title = "Standart Oda İçeriği",
            ShortDescription = "Konfor ve şıklığı bir araya getiren standart odamızda keyifli bir konaklama deneyimi yaşayın.",
            Content = @"Standart odamız, modern dekorasyonu ve konforlu yapısı ile misafirlerimize huzurlu bir konaklama sunmaktadır. 
                        Odamızda çift kişilik geniş bir yatak, özel banyo, ücretsiz Wi-Fi, LED televizyon, minibar ve klima bulunmaktadır.
                        Günlük oda temizliği, 24 saat oda servisi ve otel olanakları ile konforunuzu en üst düzeye çıkarıyoruz.

                        Sabahları güne lezzetli bir kahvaltı ile başlayabilir, otelin sunduğu sosyal alanlarda keyifli vakit geçirebilirsiniz.
                        Standart odamız, iş veya tatil amaçlı seyahat eden misafirlerimiz için ideal bir seçimdir. 
                        Sessiz ve rahat atmosferi sayesinde hem dinlenmek hem de çalışmak için mükemmel bir ortam sunmaktadır.

                        Ekstra hizmetlerimiz arasında havaalanı transferi, çamaşırhane ve araç kiralama desteği bulunmaktadır. 
                        Rezervasyonunuzu hemen yapın ve konforun tadını çıkarın!",
            IsExternal = false,
            Url = "/standart-oda",
            MetaTitle = "Standart Oda - Konforlu Konaklama",
            MetaDescription = "Modern ve konforlu standart odalarımız, uygun fiyatlarla misafirlerimize en iyi deneyimi sunmaktadır.",
            ContentType = ContentType.Odalar, // Enum varsayılan olarak int olarak kaydedilir.
            PublicationStatus = PublicationStatus.Published, // Varsayılan yayında
            AppCategoryId = 23 // Önceden oluşturduğumuz kategoriye bağlıyoruz.
        }


            };
        }
    }

}
