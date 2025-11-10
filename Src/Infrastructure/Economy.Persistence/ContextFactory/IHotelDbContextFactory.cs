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
            var options = new DbContextOptionsBuilder<HotelDbContext>()
                .UseSqlServer(connectionString)
                .Options;

            return new HotelDbContext(options);
        }
    }
}
