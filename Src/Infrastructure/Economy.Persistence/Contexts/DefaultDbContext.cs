using Economy.Base.Persistence.Configurations.ConfigurationApps;
using Economy.Domain.Entites.AppEntities;
using Economy.Domain.Entites.Identities;
using Economy.Domain.Entities.Identity;
using Economy.Persistence.Configurations.ConfigurationApps;
using Economy.Persistence.Configurations.ConfigurationAppSlide;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;

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
        public DbSet<AppManager> AppManagers { get; set; }
        public DbSet<AppGeneralSetting> AppGeneralSettings { get; set; }
        public DbSet<AppSecuritySetting> AppSecuritySettings { get; set; }
        public DbSet<AppAuditLog> AppAuditLogs { get; set; }
        public DbSet<AppErrorLog> AppErrorLogs { get; set; }
        public DbSet<AppDatabaseBackupLog> AppDatabaseBackupLogs { get; set; }

        
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
            optionsBuilder.EnableSensitiveDataLogging();
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new App_Configuration()); // ← Burası önemli
            modelBuilder.ApplyConfiguration(new App_AppManagerConfiguration()); // ← Burası önemli
            modelBuilder.ApplyConfiguration(new App_GeneralSettingsConfiguration()); // ← Burası önemli
            modelBuilder.ApplyConfiguration(new App_AppSecuritySettingConfiguration()); // ← Burası önemli
            modelBuilder.ApplyConfiguration(new App_AppAuditLogConfiguration()); // ← Burası önemli
            modelBuilder.ApplyConfiguration(new App_AppErrorLogConfiguration()); // ← Burası önemli

            


            modelBuilder.ApplyConfiguration(new AppRoleConfiguration()); // ← Burası önemli
            modelBuilder.ApplyConfiguration(new AppUserConfiguration()); // ← Burası önemli
            modelBuilder.ApplyConfiguration(new RoleClaimConfiguration()); // ← Burası önemli
            modelBuilder.ApplyConfiguration(new UserClaimConfiguration()); // ← Burası önemli
            modelBuilder.ApplyConfiguration(new UserLoginConfiguration()); // ← Burası önemli
            modelBuilder.ApplyConfiguration(new UserRoleConfiguration()); // ← Burası önemli
            modelBuilder.ApplyConfiguration(new UserTokenConfiguration()); // ← Burası önemli


            modelBuilder.Entity<AppManager>().HasKey(am => am.Id); // Primary key

            modelBuilder.Entity<AppManager>()
                .HasOne(am => am.App)
                .WithMany()
                .HasForeignKey(am => am.AppId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<AppManager>()
                .HasOne(am => am.User)
                .WithMany()
                .HasForeignKey(am => am.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            base.OnModelCreating(modelBuilder);
		}

	}
}

