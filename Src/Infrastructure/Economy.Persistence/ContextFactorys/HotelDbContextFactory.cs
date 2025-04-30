using Economy.Core.Services.Providers;
using Economy.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Economy.Base.Persistence.ContextFactorys
{
    public class HotelDbContextFactory : IDesignTimeDbContextFactory<HotelDbContext>
    {
        public HotelDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<HotelDbContext>();
            optionsBuilder.UseSqlServer("Data Source=MSI;Initial Catalog=BaseHotelDb;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False; MultipleActiveResultSets=True; Application Intent=ReadWrite;Multi Subnet Failover=False");

            return new HotelDbContext(optionsBuilder.Options);
        }
    }

    //public class HotelDbContextFactory:IDesignTimeDbContextFactory<HotelDbContext>
    //{
    //    private readonly TenantProvider _tenantProvider;

    //    public HotelDbContextFactory(TenantProvider tenantProvider)
    //    {
    //        _tenantProvider = tenantProvider;
    //    }

    //    public async Task<HotelDbContext> CreateDbContextAsync()
    //    {
    //        var connectionString = await _tenantProvider.GetConnectionStringAsync();
    //        var optionsBuilder = new DbContextOptionsBuilder<HotelDbContext>();
    //        optionsBuilder.UseSqlServer(connectionString);

    //        return new HotelDbContext(optionsBuilder.Options);
    //    }
    //}
}
