using Economy.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

namespace Economy.Core.ContextFactory
{
    public interface IHotelDbContextFactory
    {
        HotelDbContext CreateDbContext(string connectionString);
    }
    public class HotelDbContextFactory : IHotelDbContextFactory
    {
        public HotelDbContext CreateDbContext(string connectionString)
        {
            var optionsBuilder = new DbContextOptionsBuilder<HotelDbContext>();
            optionsBuilder.UseSqlServer(connectionString);  // Bağlantı string'ini kullanarak DbContext oluşturuluyor.

            return new HotelDbContext(optionsBuilder.Options);
        }
    }
}
