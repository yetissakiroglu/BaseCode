using Economy.Domain.Entites.EntityAppMenus;
using Economy.Domain.Entites.EntityCategories;
using Economy.Domain.Entites.EntityMenuItems;
using Economy.Domain.Enums;
using Economy.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

namespace Economy.Persistence.Seeds
{
    public class AppCategorySeeder
    {
        private readonly AppDbContext _context;

        public AppCategorySeeder(AppDbContext context)
        {
            _context = context;
        }

        public async Task SeedAsync()
        {
  
            if (!_context.AppCategories.Any())
            {
                var data = GetSeedCategories();
                await _context.AppCategories.AddRangeAsync(data);
                await _context.SaveChangesAsync();
            }
        }
        private IEnumerable<AppCategory> GetSeedCategories()
        {
            return new List<AppCategory>
        {
            new AppCategory { Name = "Elektronik", Url = "elektronik", ShortDescription = "Elektronik ürünler", ContentType = ContentType.General, PublicationStatus = PublicationStatus.Published },
        //    new AppCategory { Id = 2, Name = "Bilgisayar", Url = "bilgisayar", ParentCategoryId = 1, ShortDescription = "Bilgisayar ve bileşenleri", ContentType = ContentType.General, PublicationStatus = PublicationStatus.Published },
        //    new AppCategory { Id = 3, Name = "Telefon", Url = "telefon", ParentCategoryId = 1, ShortDescription = "Akıllı telefonlar", ContentType = ContentType.General, PublicationStatus = PublicationStatus.Published },
        //    new AppCategory { Id = 4, Name = "Moda", Url = "moda", ShortDescription = "Giyim, aksesuar ve moda", ContentType = ContentType.General, PublicationStatus = PublicationStatus.Published },
        //    new AppCategory { Id = 5, Name = "Erkek Giyim", Url = "erkek-giyim", ParentCategoryId = 4, ShortDescription = "Erkek giyim ürünleri", ContentType = ContentType.General, PublicationStatus = PublicationStatus.Published },
        //    new AppCategory { Id = 6, Name = "Kadın Giyim", Url = "kadin-giyim", ParentCategoryId = 4, ShortDescription = "Kadın giyim ürünleri", ContentType = ContentType.General, PublicationStatus = PublicationStatus.Published },
        //    new AppCategory { Id = 7, Name = "Ev ve Yaşam", Url = "ev-yasam", ShortDescription = "Ev eşyaları ve yaşam ürünleri", ContentType = ContentType.General, PublicationStatus = PublicationStatus.Published },
        //    new AppCategory { Id = 8, Name = "Mobilya", Url = "mobilya", ParentCategoryId = 7, ShortDescription = "Mobilya ve dekorasyon", ContentType = ContentType.General, PublicationStatus = PublicationStatus.Published },
        //    new AppCategory { Id = 9, Name = "Mutfak Gereçleri", Url = "mutfak-gerecleri", ParentCategoryId = 7, ShortDescription = "Mutfak için gerekli araçlar", ContentType = ContentType.General, PublicationStatus = PublicationStatus.Published }
        };
        }
    }

}
