using MyHotelSite.Models;

namespace MyHotelSite.Repositories;
public interface ILanguageService { Task<List<(string Code, string Name, bool IsDefault)>> GetAllAsync(int appId); }
public interface IPageRepository { Task<Page?> GetAsync(int appId, string lang, int id); }
public interface IRoomRepository { Task<List<Room>> ListAsync(int appId, string lang); Task<Room?> GetBySlugAsync(int appId, string lang, string slug); }
public interface ICampaignRepository { Task<List<Campaign>> ListAsync(int appId, string lang); Task<Campaign?> GetBySlugAsync(int appId, string lang, string slug); }
public interface IGalleryRepository { Task<List<GalleryItem>> ListAsync(int appId, string lang); Task<GalleryItem?> GetBySlugAsync(int appId, string lang, string slug); }
public interface ILocalizationRepository { Task<string?> GetAsync(int appId, string lang, string key); }


public class FakeLanguageService : ILanguageService
{
    public Task<List<(string Code, string Name, bool IsDefault)>> GetAllAsync(int appId)
        => Task.FromResult(new List<(string, string, bool)> { ("tr", "Türkçe", true), ("en", "English", false), ("de", "Deutsch", false) });
}



public class FakePageRepository : IPageRepository
{
    private static readonly List<Page> _pages = new() {
        new() { Id=101, AppId=1, Lang="tr", SectionKey="page", Title="Ana Sayfa", Slug="ana-sayfa" },
        new() { Id=101, AppId=1, Lang="en", SectionKey="page", Title="Home", Slug="home" },
        new() { Id=201, AppId=1, Lang="tr", SectionKey="rooms", Title="Odalar", Slug="odalar" },
        new() { Id=201, AppId=1, Lang="en", SectionKey="rooms", Title="Rooms", Slug="rooms" },
        new() { Id=301, AppId=1, Lang="tr", SectionKey="room", Title="Deniz Manzaralı", Slug="deniz-manzarali" },
        new() { Id=301, AppId=1, Lang="en", SectionKey="room", Title="Sea View", Slug="sea-view" },
        new() { Id=302, AppId=1, Lang="tr", SectionKey="room", Title="Aile Suiti", Slug="aile-suiti" },
        new() { Id=302, AppId=1, Lang="en", SectionKey="room", Title="Family Suite", Slug="family-suite" }
    };
    public Task<Page?> GetAsync(int appId, string lang, int id)
        => Task.FromResult(_pages.FirstOrDefault(p => p.AppId == appId && p.Lang == lang && p.Id == id));
}

public class FakeRoomRepository : IRoomRepository
{
    private static readonly List<Room> _rooms = new() {
        new() { Id=1, AppId=1, Lang="tr", HotelName="Karadeniz Bungalov", RoomName="Deluxe Deniz Manzaralı", Slug="deluxe-deniz-manzarali", ShortDescription="Geniş, balkonlu.", MainImageUrl="/images/rooms/room1.jpg", Price=1200 },
        new() { Id=2, AppId=1, Lang="tr", HotelName="Karadeniz Bungalov", RoomName="Family Suite", Slug="family-suite", ShortDescription="Aileler için ideal.", MainImageUrl="/images/rooms/room2.jpg", Price=1800 },
        new() { Id=1, AppId=1, Lang="en", HotelName="Karadeniz Bungalows", RoomName="Deluxe Sea View", Slug="deluxe-sea-view", ShortDescription="Spacious with balcony.", MainImageUrl="/images/rooms/room1.jpg", Price=120 },
    };
    public Task<List<Room>> ListAsync(int appId, string lang) => Task.FromResult(_rooms.Where(r => r.AppId == appId && r.Lang == lang).ToList());
    public Task<Room?> GetBySlugAsync(int appId, string lang, string slug) => Task.FromResult(_rooms.FirstOrDefault(r => r.AppId == appId && r.Lang == lang && r.Slug == slug));
}

public class FakeCampaignRepository : ICampaignRepository
{
    private static readonly List<Campaign> _campaigns = new() {
        new() { Id=1, AppId=1, Lang="tr", Title="Yaz İndirimi %20", Slug="yaz-indirimi", ShortDescription="Erken rezervasyona özel.", HeroImage="/images/campaigns/c1.jpg", ValidThrough=DateTime.UtcNow.AddMonths(1) },
        new() { Id=1, AppId=1, Lang="en", Title="Summer Discount 20%", Slug="summer-discount", ShortDescription="Special early booking.", HeroImage="/images/campaigns/c1.jpg", ValidThrough=DateTime.UtcNow.AddMonths(1) }
    };
    public Task<List<Campaign>> ListAsync(int appId, string lang) => Task.FromResult(_campaigns.Where(c => c.AppId == appId && c.Lang == lang).ToList());
    public Task<Campaign?> GetBySlugAsync(int appId, string lang, string slug) => Task.FromResult(_campaigns.FirstOrDefault(c => c.AppId == appId && c.Lang == lang && c.Slug == slug));
}

public class FakeGalleryRepository : IGalleryRepository
{
    private static readonly List<GalleryItem> _g = new() {
        new() { Id=1, AppId=1, Lang="tr", Url="/images/gallery/g1.jpg", Caption="Sahil", IsFeatured=true, Slug="sahil" },
        new() { Id=2, AppId=1, Lang="tr", Url="/images/gallery/g2.jpg", Caption="Yemek", IsFeatured=true, Slug="yemek" },
        new() { Id=3, AppId=1, Lang="tr", Url="/images/gallery/g3.jpg", Caption="Havuz", IsFeatured=true, Slug="havuz" },
        new() { Id=4, AppId=1, Lang="tr", Url="/images/gallery/g4.jpg", Caption="Odalar", IsFeatured=true, Slug="odalar" },
        new() { Id=5, AppId=1, Lang="tr", Url="/images/gallery/g5.jpg", Caption="Doğa",  IsFeatured=true, Slug="doga"  }
    };
    public Task<List<GalleryItem>> ListAsync(int appId, string lang) => Task.FromResult(_g.Where(x => x.AppId == appId && x.Lang == lang).ToList());
    public Task<GalleryItem?> GetBySlugAsync(int appId, string lang, string slug) => Task.FromResult(_g.FirstOrDefault(x => x.AppId == appId && x.Lang == lang && x.Slug == slug));
}

public class FakeLocalizationRepository : ILocalizationRepository
{
    private static readonly Dictionary<(int, string, string), string> _dict = new()
    {
        {(1,"tr","cookie.title"), "Çerezler"},
        {(1,"tr","cookie.text"), "Bu site deneyiminizi geliştirmek için çerez kullanır."},
        {(1,"tr","cookie.accept"), "Kabul Et"},
        {(1,"tr","cookie.reject"), "Reddet"},
        {(1,"en","cookie.title"), "Cookies"},
        {(1,"en","cookie.text"), "We use cookies to improve your experience."},
        {(1,"en","cookie.accept"), "Accept"},
        {(1,"en","cookie.reject"), "Reject"},
    };
    public Task<string?> GetAsync(int appId, string lang, string key)
        => Task.FromResult(_dict.TryGetValue((appId, lang, key), out var v) ? v : null);
}
