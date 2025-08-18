using Economy.Domain.Entites.AppEntities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Economy.Persistence.Configurations.ConfigurationApps
{
    public class App_AppSecuritySettingConfiguration : IEntityTypeConfiguration<AppSecuritySetting>
    {
        public void Configure(EntityTypeBuilder<AppSecuritySetting> b)
        {
            b.ToTable("AppSecuritySettings");
            b.HasKey(x => x.Id);

            b.Property(x => x.PasswordRequiredLength).HasDefaultValue(6);
            b.Property(x => x.PasswordRequireDigit).HasDefaultValue(true);
            b.Property(x => x.PasswordRequireLowercase).HasDefaultValue(true);
            b.Property(x => x.PasswordRequireUppercase).HasDefaultValue(false);
            b.Property(x => x.PasswordRequireNonAlphanumeric).HasDefaultValue(false);

            b.Property(x => x.LockoutTimeSpanMinutes).HasDefaultValue(30);
            b.Property(x => x.LockoutMaxFailedAccessAttempts).HasDefaultValue(5);
            b.Property(x => x.LockoutAllowedForNewUsers).HasDefaultValue(true);

            b.Property(x => x.SignInRequireConfirmedEmail).HasDefaultValue(false);
            b.Property(x => x.SignInRequireConfirmedPhoneNumber).HasDefaultValue(false);

            b.Property(x => x.TwoFactorRequired).HasDefaultValue(false);
        }
    }
}