namespace HotelMultiTenant.Services
{
    public class InMemoryContentService : IContentService
    {
        private static readonly Dictionary<int, HomeVm> Home = new()
        {
            [1] = new HomeVm("X Otel’e Hoş Geldiniz", "Denize sıfır konfor", "/themes/hero-classic.jpg",
                new[] { "Özel plaj", "Açık büfe kahvaltı", "Spa & Wellness" }),
            [2] = new HomeVm("Y Otel – Şehrin Kalbi", "Modern ve şık", "/themes/hero-modern.jpg",
                new[] { "Merkezi konum", "Roof bar", "Akıllı odalar" }),
            [3] = new HomeVm("Z Otel – Seaside", "Ege’nin mavisi", "/themes/hero-seaside.jpg",
                new[] { "Deniz manzaralı odalar", "Organik mutfak", "Tekne turları" }),
        };

        private static readonly Dictionary<int, AboutVm> About = new()
        {
            [1] = new AboutVm("Hakkımızda", "<p>1985’ten beri misafirperverlik...</p>", "/themes/abt-classic.jpg"),
            [2] = new AboutVm("Biz Kimiz?", "<p>Yeni nesil şehir oteli...</p>", "/themes/abt-modern.jpg"),
            [3] = new AboutVm("Hakkımızda", "<p>Sahil kasabasında huzur...</p>", "/themes/abt-seaside.jpg"),
        };

        private static readonly Dictionary<int, RoomsVm> Rooms = new()
        {
            [1] = new RoomsVm("Odalar", new[]
            {
            new RoomItemVm("Standart Oda", "standart", "/themes/r1.jpg", "Konforlu ve ekonomik", 120),
            new RoomItemVm("Deluxe Oda", "deluxe", "/themes/r2.jpg", "Geniş ve aydınlık", 180),
        }),
            [2] = new RoomsVm("Suitler ve Odalar", new[]
            {
            new RoomItemVm("City Suite", "city-suite", "/themes/r3.jpg", "Şehir manzaralı", 220),
            new RoomItemVm("Studio", "studio", "/themes/r4.jpg", "Minimal & modern", 150),
        }),
            [3] = new RoomsVm("Odalar", new[]
            {
            new RoomItemVm("Sea View", "sea-view", "/themes/r5.jpg", "Maviye karşı uyanın", 200),
            new RoomItemVm("Garden Room", "garden", "/themes/r6.jpg", "Doğayla iç içe", 140),
        }),
        };

        private static readonly Dictionary<int, ServicesVm> Svc = new()
        {
            [1] = new ServicesVm("Hizmetler", new[] { ("Spa", "Masaj & sauna"), ("Restoran", "Yerel lezzetler") }),
            [2] = new ServicesVm("Hizmetlerimiz", new[] { ("Gym", "24/7 spor"), ("Roof Bar", "Manzara eşliğinde") }),
            [3] = new ServicesVm("Hizmetler", new[] { ("Plaj", "Özel iskele"), ("Organik Mutfak", "Günlük menü") }),
        };

        private static readonly Dictionary<int, ContactVm> Cnt = new()
        {
            [1] = new ContactVm("İletişim", "Adres 1", "+90 555 000 00 01", "info@xotel.local",
                "<iframe src='https://maps.example/x'></iframe>"),
            [2] = new ContactVm("Bize Ulaşın", "Adres 2", "+90 555 000 00 02", "hello@yotel.local",
                "<iframe src='https://maps.example/y'></iframe>"),
            [3] = new ContactVm("İletişim", "Adres 3", "+90 555 000 00 03", "hi@zotel.local",
                "<iframe src='https://maps.example/z'></iframe>"),
        };

        public Task<HomeVm> GetHomeAsync(int id, CancellationToken ct = default) => Task.FromResult(Home[id]);
        public Task<AboutVm> GetAboutAsync(int id, CancellationToken ct = default) => Task.FromResult(About[id]);
        public Task<RoomsVm> GetRoomsAsync(int id, CancellationToken ct = default) => Task.FromResult(Rooms[id]);
        public Task<ServicesVm> GetServicesAsync(int id, CancellationToken ct = default) => Task.FromResult(Svc[id]);
        public Task<ContactVm> GetContactAsync(int id, CancellationToken ct = default) => Task.FromResult(Cnt[id]);
    }
}
