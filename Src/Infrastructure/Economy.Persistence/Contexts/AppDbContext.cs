using Economy.Domain.Entites.EntityAppLanguage;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Economy.Persistence.Contexts
{

    public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
	{

        public DbSet<AppLanguage> AppLanguages { get; set; }
       
        

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			base.OnConfiguring(optionsBuilder);

		}
		protected override void OnModelCreating(ModelBuilder builder)
		{

            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly()); // Diğer tüm konfigürasyonları otomatik olarak uygular
            base.OnModelCreating(builder);
		}

	}
}

