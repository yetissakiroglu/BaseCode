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
            new AppCategory {ContentType = ContentType.General, PublicationStatus = PublicationStatus.Published },
             };
        }
    }

}
