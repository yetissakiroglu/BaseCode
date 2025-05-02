using Economy.Base.Persistence.Configurations.ConfigurationApps;
using Economy.Domain.Entites.AppEntities;
using Economy.Domain.Entites.Identities;
using Economy.Domain.Entities.Identity;
using Economy.Persistence.Configurations.ConfigurationAppSlide;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Economy.Persistence.Contexts
{
    public class DefaultDbContext : IdentityDbContext<AppUser, AppRole, int, AppUserClaim, AppUserRole, AppUserLogin, AppRoleClaim, AppUserToken>
    {
        public DefaultDbContext(DbContextOptions<DefaultDbContext> options) : base(options)
        {


        }

        public DbSet<AppUser> AppUsers { get; set; }
        public DbSet<AppRole> Roles { get; set; }
        public DbSet<AppRoleClaim> RoleClaims { get; set; }
        public DbSet<AppUserRole> UserRoles { get; set; }
        public DbSet<AppUserClaim> UserClaims { get; set; }
        public DbSet<AppUserLogin> UserLogins { get; set; }
        public DbSet<AppUserToken> UserTokens { get; set; }
        public DbSet<App> Apps { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
            optionsBuilder.EnableSensitiveDataLogging();
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfiguration(new App_Configuration()); // ← Burası önemli
            builder.ApplyConfiguration(new AppRoleConfiguration()); // ← Burası önemli
            builder.ApplyConfiguration(new AppUserConfiguration()); // ← Burası önemli
            builder.ApplyConfiguration(new RoleClaimConfiguration()); // ← Burası önemli
            builder.ApplyConfiguration(new UserClaimConfiguration()); // ← Burası önemli
            builder.ApplyConfiguration(new UserLoginConfiguration()); // ← Burası önemli
            builder.ApplyConfiguration(new UserRoleConfiguration()); // ← Burası önemli
            builder.ApplyConfiguration(new UserTokenConfiguration()); // ← Burası önemli

            base.OnModelCreating(builder);
		}

	}
}

