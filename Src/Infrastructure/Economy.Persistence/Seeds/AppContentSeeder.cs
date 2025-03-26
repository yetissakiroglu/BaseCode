using Economy.Domain.Entites.EntityAppContents.AppContents;
using Economy.Domain.Enums;
using Economy.Persistence.Contexts;

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
                new AppContent
    {
        ContentType = ContentType.General, // Enum’a göre güncelleyebilirsin
        PublicationStatus = PublicationStatus.Published,
        AppCategoryId = 23,
        Thumbnail = "img/room_home_1.jpg",
        Translations = new List<AppContentTranslation>
        {
            new AppContentTranslation {
                AppLanguageId = 1,
                Title = "Deniz ve Plaj Olanakları",
                Url = "deniz-ve-plaj-olanaklari",
                ShortDescription = "Ege'nin eşsiz denizi ve özel plajımızda huzuru keşfedin.",
                Content = @"Otelimize özel plaj alanımızda güneşin ve denizin tadını doyasıya çıkarın. 
                            Şezlong, şemsiye ve havlu hizmetimizle birlikte, konforlu bir deniz keyfi sizi bekliyor. 
                            Ayrıca deniz sporları, kano ve paddleboard gibi aktivitelere katılabilirsiniz.",
                IsExternal = false,
                MetaTitle = "Deniz ve Plaj - Özel Plaj Olanakları",
                MetaDescription = "Otelimize özel plaj ve deniz olanakları ile tatilin keyfini çıkarın."
            },
            new AppContentTranslation {
                AppLanguageId = 2,
                Title = "Beach and Sea Facilities",
                Url = "beach-and-sea-facilities",
                ShortDescription = "Discover peace at our private beach on the unique Aegean coast.",
                Content = @"Enjoy the sun and sea at our hotel's private beach area. 
                            With sunbeds, umbrellas, and towel service, comfort is guaranteed. 
                            You can also join water sports like canoeing and paddleboarding.",
                IsExternal = false,
                MetaTitle = "Beach and Sea - Private Beach Services",
                MetaDescription = "Enjoy a relaxing vacation with our hotel's private beach and sea activities."
            }
        }
    },

    new AppContent
    {
        ContentType = ContentType.General,
        PublicationStatus = PublicationStatus.Published,
        AppCategoryId = 23,
        Thumbnail = "img/room_home_1.jpg",
        Translations = new List<AppContentTranslation>
        {
            new AppContentTranslation {
                AppLanguageId = 1,
                Title = "Havuz ve Spa",
                Url = "havuz-ve-spa",
                ShortDescription = "Rahatlatıcı bir tatil için havuz ve spa hizmetlerimizden faydalanın.",
                Content = @"Açık ve kapalı havuzlarımız, modern spa merkezimiz ile sizleri dinlenmeye davet ediyor. 
                            Türk hamamı, sauna, buhar odası ve profesyonel masaj hizmetleri ile stresten arının.",
                IsExternal = false,
                MetaTitle = "Havuz & Spa - Dinlenmenin Adresi",
                MetaDescription = "Spa, sauna, havuz ve masaj hizmetleriyle kendinizi yenileyin."
            },
            new AppContentTranslation {
                AppLanguageId = 2,
                Title = "Pool and Spa",
                Url = "pool-and-spa",
                ShortDescription = "Take advantage of our relaxing pool and spa services during your holiday.",
                Content = @"Our indoor and outdoor pools and modern spa center invite you to relax. 
                            Relieve your stress with our Turkish bath, sauna, steam room, and professional massage services.",
                IsExternal = false,
                MetaTitle = "Pool & Spa - A Place to Relax",
                MetaDescription = "Refresh yourself with our spa, sauna, pool, and massage services."
            }
        }
    },

    new AppContent
    {
        ContentType = ContentType.General,
        PublicationStatus = PublicationStatus.Published,
        AppCategoryId = 23,
        Thumbnail = "img/room_home_1.jpg",
        Translations = new List<AppContentTranslation>
        {
            new AppContentTranslation {
                AppLanguageId = 1,
                Title = "Aktiviteler ve Eğlence",
                Url = "aktiviteler-ve-eglence",
                ShortDescription = "Her yaşa uygun aktivitelerle tatilinizi eğlenceli hale getirin.",
                Content = @"Gün boyu süren spor aktiviteleri, yoga seansları, çocuk kulübü ve akşam eğlenceleriyle tatilinizi dolu dolu geçirin. 
                            Ayrıca canlı müzik ve temalı geceler de sizleri bekliyor.",
                IsExternal = false,
                MetaTitle = "Aktiviteler ve Eğlence - Eğlenceli Tatil",
                MetaDescription = "Otelimizde spor, müzik ve eğlenceyle dolu bir tatil sizi bekliyor."
            },
            new AppContentTranslation {
                AppLanguageId = 2,
                Title = "Activities and Entertainment",
                Url = "activities-and-entertainment",
                ShortDescription = "Make your holiday fun with activities for all ages.",
                Content = @"Enjoy full days with sports activities, yoga sessions, kids club, and evening shows. 
                            Live music and theme nights are also waiting for you.",
                IsExternal = false,
                MetaTitle = "Activities and Entertainment - Fun Holiday",
                MetaDescription = "A holiday full of sports, music, and fun at our hotel."
            }
        }
    },
            new   AppContent
            {

            ContentType = ContentType.Odalar, // Enum varsayılan olarak int olarak kaydedilir.
            PublicationStatus = PublicationStatus.Published, // Varsayılan yayında
            AppCategoryId = 23 // Önceden oluşturduğumuz kategoriye bağlıyoruz.
            ,Thumbnail="img/room_home_1.jpg"
            ,Translations = new List<AppContentTranslation>
            {
                new AppContentTranslation { AppLanguageId = 1, Title = "Standart Oda", Url = "standart-oda",
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
            MetaTitle = "Standart Oda - Konforlu Konaklama",
            MetaDescription = "Modern ve konforlu standart odalarımız, uygun fiyatlarla misafirlerimize en iyi deneyimi sunmaktadır."
                },
                new AppContentTranslation { AppLanguageId = 2, Title = "Standard Room", Url = "standard-room",  ShortDescription = "Konfor ve şıklığı bir araya getiren standart odamızda keyifli bir konaklama deneyimi yaşayın.",
            Content = @"Standart odamız, modern dekorasyonu ve konforlu yapısı ile misafirlerimize huzurlu bir konaklama sunmaktadır. 
                        Odamızda çift kişilik geniş bir yatak, özel banyo, ücretsiz Wi-Fi, LED televizyon, minibar ve klima bulunmaktadır.
                        Günlük oda temizliği, 24 saat oda servisi ve otel olanakları ile konforunuzu en üst düzeye çıkarıyoruz.

                        Sabahları güne lezzetli bir kahvaltı ile başlayabilir, otelin sunduğu sosyal alanlarda keyifli vakit geçirebilirsiniz.
                        Standart odamız, iş veya tatil amaçlı seyahat eden misafirlerimiz için ideal bir seçimdir. 
                        Sessiz ve rahat atmosferi sayesinde hem dinlenmek hem de çalışmak için mükemmel bir ortam sunmaktadır.

                        Ekstra hizmetlerimiz arasında havaalanı transferi, çamaşırhane ve araç kiralama desteği bulunmaktadır. 
                        Rezervasyonunuzu hemen yapın ve konforun tadını çıkarın!",
            IsExternal = false,
            MetaTitle = "Standart Oda - Konforlu Konaklama",
            MetaDescription = "Modern ve konforlu standart odalarımız, uygun fiyatlarla misafirlerimize en iyi deneyimi sunmaktadır."  }
            }


            }


            };
        }
    }

}
           
                